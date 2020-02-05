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
        /// A flag indication if the login command is running
        /// </summary>
        public bool LoginIsRunning { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to login
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// The command to register for a new account
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
                await Task.Delay(2000);

                IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.chat);

                //Go to chat page

                //if (parameter is IHavePassword loginPage)
                //{
                //    var passwd = loginPage.SecureString.Unsecure();
                //}
            });
        }

        /// <summary>
        /// Takes the user to the register page
        /// </summary>
        /// <returns></returns>
        private async Task Register()
        {
            //TODO: Go to register page?
            //if (Application.Current.MainWindow is BrowserView browserView &&
            //    browserView.DataContext is BrowserViewModel vm)
            //{
            //    vm.CurrentPage = ApplicationPage.register;
            //}
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.register);

            await Task.Delay(1);
        }

        #endregion
    }
}



