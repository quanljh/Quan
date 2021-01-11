using Quan.Web;
using Quan.Word.Core;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using static Quan.Word.DI;

namespace Quan.Word
{
    public class LoginViewModel : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// The Email of the user
        /// </summary>
        private string _email;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        /// <summary>
        /// A flag indication if the Login command is running
        /// </summary>
        private bool _loginIsRunning;

        public bool LoginIsRunning
        {
            get => _loginIsRunning;
            set => SetProperty(ref _loginIsRunning, value);
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

        public LoginViewModel()
        {
            // TODO:
            LoginCommand = new RelayCommand(async parameter => await LoginAsync(parameter));

            RegisterCommand = new RelayCommand(async () => await Register());
        }

        #endregion

        #region Method

        /// <summary>
        /// Attempts to log the user in
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the users password </param>
        /// <returns></returns>
        public async Task LoginAsync(object parameter)
        {
            await RunCommandAsync(() => LoginIsRunning, async () =>
            {
                // Call the server and attempt to login with credentials
                // TODO: Move all URLs and API routes to static class in core
                var result =
                    await WebRequests.PostAsync<ApiResponse<UserProfileDetailsApiModel>>(
                        "http://localhost:5000/api/login",
                        new LoginCredentialsApiModel
                        {
                            UsernameOrEmail = Email,
                            Password = (parameter as IHavePassword)?.SecurePassword.Unsecure()
                        });

                // If the response has an error...
                if (await result.DisplayErrorIfFailedAsync("Login Failed"))
                    // We are done
                    return;

                // OK successfully logged in... now get users data
                var loginResult = result.ServerResponse.Response;

                // Let the application view model handle what happens
                // with the successful login
                await ApplicationVM.HandleSuccessfulLoginAsync(loginResult);
            });
        }

        /// <summary>
        /// Takes the user to the Register page
        /// </summary>
        /// <returns></returns>
        private async Task Register()
        {
            //TODO: Go to Register page?
            ApplicationVM.GoToPage(ApplicationPage.Register);

            await Task.Delay(1);
        }

        #endregion
    }
}



