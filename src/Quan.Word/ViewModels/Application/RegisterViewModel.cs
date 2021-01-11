using Quan.Web;
using Quan.Word.Core;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using static Quan.Word.DI;

namespace Quan.Word
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
        private string _username;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        /// <summary>
        /// The email of the user
        /// </summary>
        private string _email;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        /// <summary>
        /// A flag indication if the Register command is running
        /// </summary>
        private bool _registerIsRunning;

        public bool RegisterIsRunning
        {
            get => _registerIsRunning;
            set => SetProperty(ref _registerIsRunning, value);
        }

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
            await RunCommandAsync(() => RegisterIsRunning, async () =>
            {
                // Call the server and attempt to login with credentials
                // TODO: Move all URLs and API routes to static class in core
                var result =
                    await WebRequests.PostAsync<ApiResponse<UserProfileDetailsApiModel>>(
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
                await ApplicationVM.HandleSuccessfulLoginAsync(loginResult);
            });
        }

        /// <summary>
        /// Takes the user to the Login page
        /// </summary>
        /// <returns></returns>
        private async Task LoginAsync()
        {
            //Go to Login page
            ApplicationVM.GoToPage(ApplicationPage.Login);

            await Task.Delay(1);
        }

        #endregion
    }
}



