using Quan.Security;
using Quan.ViewModels.Base;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quan.ViewModels
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

        #endregion

        #region Constructor

        public LoginViewModel()
        {
            LoginCommand = new RelayParameterizedCommand(async parameter => await Login(parameter));
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
                await Task.Delay(5000);

                if (parameter is IHavePassword loginPage)
                {
                    var passwd = loginPage.SecureString.Unsecure();
                }
            });


        }

        #endregion
    }
}



