using System.Threading.Tasks;

namespace Quan.Word.Core
{
    /// <summary>
    /// Send emails using the <see cref="IEmailSender"/> and creating the HTML
    /// email from specific templates
    /// </summary>
    public interface IEmailTemplateSender
    {
        /// <summary>
        /// Sends an email with the given details using the General templates
        /// </summary>
        /// <param name="details">The email message details. Note the Content property is ignored and replaced with the template</param>
        /// <param name="title">The title</param>
        /// <param name="content1">The first line contents</param>
        /// <param name="content2">The second line contents</param>
        /// <param name="buttonContent">The button content</param>
        /// <param name="buttonUrl">The button URL</param>
        /// <returns></returns>
        Task<SendEmailResponse> SendGeneralEmailAsync(SendEmailDetails details, string title, string content1, string content2, string buttonContent, string buttonUrl);
    }
}
