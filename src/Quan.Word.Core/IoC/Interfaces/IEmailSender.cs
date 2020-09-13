using System.Threading.Tasks;

namespace Quan.Word.Core
{
    /// <summary>
    /// A service that handles sending emails on behalf of the caller
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sends an email message with the give information
        /// </summary>
        /// <param name="details">The details about the email to send</param>
        /// <returns></returns>
        Task<SendEmailResponse> SendEmailAsync(SendEmailDetails details);
    }
}
