﻿using Quan.Web;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using Quan.Word.Core.ApiModels;

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
            LoginCommand = new RelayParameterizedCommand(async parameter => await LoginAsync(parameter));

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

                // If the response has an error...
                if (await result.DisplayErrorIfFailedAsync("Login Failed"))
                    // We are done
                    return;

                // OK successfully logged in... now get users data
                var loginResult = result.ServerResponse.Response;

                // Let the application view model handle what happens
                // with the successful login
                await IoC.Application.HandleSuccessfulLoginAsync(loginResult);
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



