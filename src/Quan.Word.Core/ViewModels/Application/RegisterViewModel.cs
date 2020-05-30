using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using Quan.Web;

namespace Quan.Word.Core
{
    /// <summary>
    /// The view model for a Register page
    /// </summary>
    public class RegisterViewModel : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// The username of the user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// A flag indication if the Register command is running
        /// </summary>
        public bool RegisterIsRunning { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to Login
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// The command to Register for a new account
        /// </summary>
        public ICommand RegisterCommand { get; set; }

        #endregion

        #region Constructor

        public RegisterViewModel()
        {
            RegisterCommand = new RelayParameterizedCommand(async parameter => await RegisterAsync(parameter));

            LoginCommand = new RelayCommand(async () => await LoginAsync());
        }

        #endregion

        #region Method

        /// <summary>
        /// Attempts to Register a new user
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the users password </param>
        /// <returns></returns>
        public async Task RegisterAsync(object parameter)
        {
            await RunCommand(() => RegisterIsRunning, async () =>
            {
                // Call the server and attempt to login with credentials
                // TODO: Move all URLs and API routes to static class in core
                var result =
                    await WebRequests.PostAsync<ApiResponse<RegisterCredentialsApiModel>>(
                        "http://localhost:5000/api/register",
                        new RegisterCredentialsApiModel
                        {
                            Username = Username,
                            Email = Email,
                            Password = (parameter as IHavePassword)?.SecurePassword.Unsecure()
                        });

                // If there was no response, bad data, or a response with a error message...
                if (result?.ServerResponse == null || !result.ServerResponse.Successful)
                {
                    // Default error message
                    // TODO: Localize strings
                    var message = "Unknown error from server call";

                    // If we got a response from the server..
                    if (result?.ServerResponse != null)
                        // Set message to servers response
                        message = result.ServerResponse.ErrorMessage;
                    // If we have a result but deserialize failed...
                    else if (!string.IsNullOrWhiteSpace(result?.RawServerResponse))
                        // Set error message
                        message = $"Unexpected response from server. {result.RawServerResponse}";
                    // If we have a result but no server response details at all...
                    else if (result != null)
                        // Set message to standard HTTP server response details
                        message =
                            $"Failed to communicate with server. Status code {result.StatusCode}. {result.StatusDescription}";

                    // Display error
                    await IoC.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        // TODO: Localize strings
                        Title = "Login Failed",
                        Message = message
                    });

                    // We are done
                    return;
                }

                // OK successfully logged in... now get users data
                var userData = result.ServerResponse.Response;

                // Store this in the client data store
                await IoC.ClientDataStore.SaveLoginCredentialsAsync(Mapper.Map<LoginCredentialsDataModel>(userData));

                // Load new settings
                await IoC.Settings.LoadAsync();

                // Go to chat page
                IoC.Application.GoToPage(ApplicationPage.Chat);
            });
        }

        /// <summary>
        /// Takes the user to the Login page
        /// </summary>
        /// <returns></returns>
        private async Task LoginAsync()
        {
            //Go to Login page
            IoC.Application.GoToPage(ApplicationPage.Login);

            await Task.Delay(1);
        }

        #endregion
    }
}



