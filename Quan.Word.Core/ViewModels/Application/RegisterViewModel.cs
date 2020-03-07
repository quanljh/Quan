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
        /// The Email of the user
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
            RegisterCommand = new RelayParameterizedCommand(async parameter => await Register(parameter));

            LoginCommand = new RelayCommand(async () => await Login());
        }

        #endregion

        #region Method

        /// <summary>
        /// Attempts to Register a new user
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the users password </param>
        /// <returns></returns>
        public async Task Register(object parameter)
        {
            await RunCommand(() => RegisterIsRunning, async () =>
            {
                await Task.Delay(5000);
            });
        }

        /// <summary>
        /// Takes the user to the Login page
        /// </summary>
        /// <returns></returns>
        private async Task Login()
        {
            //Go to Login page
            IoC.Application.GoToPage(ApplicationPage.Login);

            await Task.Delay(1);
        }

        #endregion
    }
}



