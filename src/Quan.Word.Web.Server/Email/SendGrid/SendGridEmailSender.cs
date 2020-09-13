using Newtonsoft.Json;
using Quan.Word.Core;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Quan.FrameworkDI;

namespace Quan.Word.Web.Server
{
    /// <summary>
    /// Sends emails using the SendGrid service
    /// </summary>
    public class SendGridEmailSender : IEmailSender
    {
        public async Task<SendEmailResponse> SendEmailAsync(SendEmailDetails details)
        {
            // Get the SendGrid key
            var apiKey = Configuration["SendGridKey"];

            // Create a new SendGrid client
            var client = new SendGridClient(apiKey);

            // From
            var from = new EmailAddress(details.FromEmail, details.FromName);

            // To
            var to = new EmailAddress(details.ToEmail, details.ToName);

            // Subject
            var subject = details.Subject;

            // Content
            var content = details.Content;

            // Create Email class ready to send
            var msg = MailHelper.CreateSingleEmail(
                from,
                to,
                subject,
                details.IsHTML ? null : details.Content,
                details.IsHTML ? content : null);

            // Finally, send the email...
            var response = await client.SendEmailAsync(msg);

            // If we succeeded
            if (response.StatusCode == HttpStatusCode.Accepted)
                return new SendEmailResponse();

            // Otherwise, it failed
            try
            {
                // Get the result in the body
                var bodyResult = await response.Body.ReadAsStringAsync();

                // Deserialize the response
                var sendGridResponse = JsonConvert.DeserializeObject<SendGridResponse>(bodyResult);

                // Add any errors to the response
                var errorResponse = new SendEmailResponse
                {
                    Errors = sendGridResponse?.Errors.Select(f => f.Message).ToList()
                };

                // Make sure we have at least one error
                if (errorResponse.Errors == null || errorResponse.Errors.Count == 0)
                    // Add an unknown error
                    // TODO:
                    errorResponse.Errors = new List<string>(new[] { "Unknown error from email sending service. Please contact Quan support" });


                return errorResponse;
            }
            catch (Exception e)
            {
                // TODO: Localization

                // Break if we are debugging
                if (Debugger.IsAttached)
                    Debugger.Break();

                // If something unexpected happened, return message
                return new SendEmailResponse
                {
                    Errors = new List<string>(new[] { "Unknown error occurred" })
                };

            }
        }
    }
}
