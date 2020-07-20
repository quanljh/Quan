using Quan.Web;
using Quan.Word.Core.ApiModels;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;

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
            RegisterCommand = new RelayCommand(async parameter => await RegisterAsync(parameter));

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
                    await WebRequests.PostAsync<ApiResponse<RegisterResultApiModel>>(
                        "http://localhost:5000/api/register",
                        new RegisterCredentialsApiModel
                        {
                            Username = Username,
                            Email = Email,
                            Password = (parameter as IHavePassword)?.SecurePassword.Unsecure()
                        });

                // If the response has an error...
                if (await result.DisplayErrorIfFailedAsync("Register Failed"))
                    // We are done
                    return;

                // OK successfully registered (and logged in )... now get users data
                var loginResult = result.ServerResponse.Response;

                // Let the application view model handle what happens
                // with the successful login
                await IoC.Application.HandleSuccessfulLoginAsync(loginResult);
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



