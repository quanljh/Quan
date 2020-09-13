using Quan.Word.Core;
using System.Threading.Tasks;
using static Quan.Word.DI;

namespace Quan.Word
{
    /// <summary>
    /// Extension methods for the <see cref="WebRequestresponseExtensions"/> class
    /// </summary>
    public static class WebRequestresponseExtensions
    {
        /// <summary>
        /// Checks the web request for any errors, displaying them if there are any
        /// </summary>
        /// <typeparam name="T">The type if Api Response</typeparam>
        /// <param name="response">The response to check</param>
        /// <param name="title">The title of the error dialog if there is an error</param>
        /// <returns>Returns true if there was an error, of false if all was ok</returns>
        public static async Task<bool> DisplayErrorIfFailedAsync<T>(this WebRequestResult<ApiResponse<T>> response, string title)
        {
            // If there was no response, bad data, or a response with a error message...
            if (response?.ServerResponse == null || !response.ServerResponse.Successful)
            {
                // Default error message
                // TODO: Localize strings
                var message = "Unknown error from server call";

                // If we got a response from the server..
                if (response?.ServerResponse != null)
                    // Set message to servers response
                    message = response.ServerResponse.ErrorMessage;
                // If we have a response but deserialize failed...
                else if (!string.IsNullOrWhiteSpace(response?.RawServerResponse))
                    // Set error message
                    message = $"Unexpected response from server. {response.RawServerResponse}";
                // If we have a response but no server response details at all...
                else if (response != null)
                    // Set message to standard HTTP server response details
                    message =
                        $"Failed to communicate with server. Status code {response.StatusCode}. {response.StatusDescription}";

                // Display error
                await UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    // TODO: Localize strings
                    Title = title,
                    Message = message
                });

                // Return that we had an error
                return true;
            }

            // All was OK, so return false for no error
            return false;
        }
    }
}
