using Quan.Word.Core;
using System.Threading.Tasks;

namespace Quan.Word.Web.Server
{
    /// <summary>
    /// Handles sending emails specific to the Quan Word server
    /// </summary>
    public static class QuanEmailSender
    {
        /// <summary>
        /// Sends a verification email to the specified user
        /// </summary>
        /// <param name="displayName">The users display name (typically first name)</param>
        /// <param name="email">The users email to be verified</param>
        /// <param name="verificationUrl">The URL the user needs to click to verify their email</param>
        /// <returns></returns>
        public static async Task<SendEmailResponse> SendUserVerificationEmailAsync(string displayName, string email, string verificationUrl)
        {
            return await DI.EmailTemplateSender.SendGeneralEmailAsync(new SendEmailDetails
            {
                Content = "This is our first HTML email <b>with some bold text</b>",
                IsHTML = true,
                FromEmail = "contact@quanljh.com",
                FromName = "quanljh",
                ToEmail = email,
                ToName = displayName,
                Subject = "Verify Your Email - Quan Word"
            },
                "Verify Email",
                $"Hi {displayName ?? "stranger"},",
                "Thanks for creating an account with us.<br>To continue please verify your email with us.",
                "Verify Email",
                verificationUrl);
        }
    }
}
