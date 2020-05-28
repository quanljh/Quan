using Quan.Web;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quan.Word.Core
{
    public class LoginViewModel : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// The Email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// A flag indication if the Login command is running
        /// </summary>
        public bool LoginIsRunning { get; set; }

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

        public LoginViewModel()
        {
            LoginCommand = new RelayParameterizedCommand(async parameter => await Login(parameter));

            RegisterCommand = new RelayCommand(async () => await Register());
        }

        #endregion

        #region Method

        /// <summary>
        /// Attempts to log the user in
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the users password </param>
        /// <returns></returns>
        public async Task Login(object parameter)
        {
            await RunCommand(() => LoginIsRunning, async () =>
            {
                // Call the server and attempt to login with credentials
                // TODO: Move all URLs and API routes to static class in core
                var result =
                    await WebRequests.PostAsync<ApiResponse<LoginResultApiModel>>(
                        "http://localhost:5000/api/login",
                        new LoginCredentialsApiModel
                        {
                            UsernameOrEmail = Email,
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
        /// Takes the user to the Register page
        /// </summary>
        /// <returns></returns>
        private async Task Register()
        {
            //TODO: Go to Register page?
            //if (Application.Current.MainWindow is BrowserView browserView &&
            //    browserView.DataContext is BrowserViewModel vm)
            //{
            //    vm.CurrentPage = ApplicationPage.Register;
            //}
            IoC.Application.GoToPage(ApplicationPage.Register);

            await Task.Delay(1);
        }

        #endregion
    }
}



