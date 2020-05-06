using System;
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
                // TODO: Fake a Login...
                await Task.Delay(1000);

                // OK successfully logged in... now get users data
                // TODO: Ask server for users info

                // TODO: Remove this with real information pulled from our database in future
                IoC.Settings.Name = new TextEntryViewModel { Label = "Name", OriginalText = $"quanljh {DateTime.Now.ToLocalTime()}" };
                IoC.Settings.Username = new TextEntryViewModel { Label = "Username", OriginalText = "quan" };
                IoC.Settings.Password = new PasswordEntryViewModel { Label = "Password", FakePassword = "********" };
                IoC.Settings.Email = new TextEntryViewModel { Label = "Email", OriginalText = "quanljh@gmail.com" };


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



