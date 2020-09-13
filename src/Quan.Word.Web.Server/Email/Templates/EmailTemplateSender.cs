﻿using Quan.Word.Core;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Quan.Word.Web.Server
{
    /// <summary>
    /// Handle sending templated emails
    /// </summary>
    public class EmailTemplateSender : IEmailTemplateSender
    {
        public async Task<SendEmailResponse> SendGeneralEmailAsync(SendEmailDetails details, string title, string content1, string content2, string buttonContent, string buttonUrl)
        {
            var templateText = default(string);
            // Read the general template from file
            // TODO: Replace with IoC Flat data provider
            using (var reader = new StreamReader(Assembly.GetEntryAssembly().GetManifestResourceStream("Quan.Word.Web.Server.Email.Templates.GeneralTemplate.html"), Encoding.UTF8))
            {
                // Read file contents
                templateText = await reader.ReadToEndAsync();
            }

            // Replace special values with those inside the template
            templateText = templateText.Replace("--Title--", title)
                .Replace("--Content1--", content1)
                .Replace("--Content2--", content2)
                .Replace("--ButtonContent--", buttonContent)
                .Replace("--ButtonUrl--", buttonUrl);

            // Set the details content to this template content
            details.Content = templateText;

            // Send Email
            return await new SendGridEmailSender().SendEmailAsync(details);
        }
    }
}
