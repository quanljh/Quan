using Quan.ViewModels;
using Quan.ViewModels.Base;
using System.Security;

namespace Quan.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : BasePage<LoginViewModel>, IHavePassword
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The secure password for this login page
        /// </summary>
        public SecureString SecureString => PasswordBox.SecurePassword;
    }
}
