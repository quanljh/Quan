using Quan.Word.Core;
using System.Threading.Tasks;
using System.Windows.Input;
using static Quan.Word.DI;


namespace Quan.Word
{
    /// <summary>
    ///
    /// </summary>
    public class SettingsViewModel : ViewModelBase
    {
        #region Public Properties

        /// <summary>
        /// The current users name
        /// </summary>
        private TextEntryViewModel _name;

        public TextEntryViewModel Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// The current users username
        /// </summary>
        private TextEntryViewModel _username;

        public TextEntryViewModel Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        /// <summary>
        /// The current users password
        /// </summary>
        private PasswordEntryViewModel _password;

        public PasswordEntryViewModel Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        /// <summary>
        /// The current users email
        /// </summary>
        private TextEntryViewModel _email;

        public TextEntryViewModel Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        /// <summary>
        /// The text for the logout button
        /// </summary>
        private string _logoutButtonText;

        public string LogoutButtonText
        {
            get => _logoutButtonText;
            set => SetProperty(ref _logoutButtonText, value);
        }

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to open the settings menu
        /// </summary>
        public ICommand OpenCommand { get; set; }

        /// <summary>
        /// The command to close the settings menu
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// The command to logout of the application
        /// </summary>
        public ICommand LogoutCommand { get; set; }

        /// <summary>
        /// The command to clear the users data from the view model
        /// </summary>
        public ICommand ClearUserDataCommand { get; set; }

        /// <summary>
        /// Loads the settings data from the client data store
        /// </summary>
        public ICommand LoadCommand { get; set; }


        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SettingsViewModel()
        {
            // Create commands
            OpenCommand = new RelayCommand(Open);
            CloseCommand = new RelayCommand(Close);
            LogoutCommand = new RelayCommand(Logout);
            ClearUserDataCommand = new RelayCommand(ClearUserData);
            LoadCommand = new RelayCommand(async () => await LoadAsync());



            // TODO: Get from localization
            LogoutButtonText = "Logout";
        }

        #endregion

        /// <summary>
        /// Open the settings menu
        /// </summary>
        public void Open()
        {
            // Close settings menu
            ApplicationVM.SettingsMenuVisible = true;
        }

        /// <summary>
        /// Closes the settings menu
        /// </summary>
        public void Close()
        {
            // Close settings menu
            ApplicationVM.SettingsMenuVisible = false;
        }

        /// <summary>
        /// Logs the user out
        /// </summary>
        public void Logout()
        {
            // TODO: Confirm the user wants to logout

            // TODO: Clear any user data/cache

            // Clean all application level view models that contain
            // any information about the current user
            ClearUserData();

            // Go to Login page
            ApplicationVM.GoToPage(ApplicationPage.Login);
        }

        /// <summary>
        /// Clears any data specific to the current user
        /// </summary>
        public void ClearUserData()
        {
            // Clear all view models containing the users info
            Name = null;
            Username = null;
            Password = null;
            Email = null;
        }

        /// <summary>
        /// Sets the settings view model properties based on the data in the client data store
        /// </summary>
        /// <returns></returns>
        public async Task LoadAsync()
        {
            var storedCredentials = await ClientDataStore.GetLoginCredetntialsAsync();

            Name = new TextEntryViewModel { Label = "Name", OriginalText = $"{storedCredentials?.FirstName} {storedCredentials?.LastName}" };
            Username = new TextEntryViewModel { Label = "Username", OriginalText = storedCredentials?.UserName };
            Password = new PasswordEntryViewModel { Label = "Password", FakePassword = "********" };
            Email = new TextEntryViewModel { Label = "Email", OriginalText = storedCredentials?.Email };
        }

    }
}
