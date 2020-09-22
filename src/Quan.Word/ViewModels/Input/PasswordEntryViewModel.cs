using Quan.Word.Core;
using System.Security;
using System.Windows.Input;
using static Quan.Word.DI;


namespace Quan.Word
{
    /// <summary>
    /// The view model for a password entry to edit a password 
    /// </summary>
    public class PasswordEntryViewModel : ViewModelBase
    {
        #region Public Properties

        /// <summary>
        /// The label to identify what this value is for
        /// </summary>
        private string _label;

        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        /// <summary>
        /// The fake password display string
        /// </summary>
        private string _fakePassword;

        public string FakePassword
        {
            get => _fakePassword;
            set => SetProperty(ref _fakePassword, value);
        }

        /// <summary>
        /// The current password hint text
        /// </summary>
        private string _currentPasswordHintText;

        public string CurrentPasswordHintText
        {
            get => _currentPasswordHintText;
            set => SetProperty(ref _currentPasswordHintText, value);
        }

        /// <summary>
        /// The new password hint text
        /// </summary>
        private string _newPasswordHintText;

        public string NewPasswordHintText
        {
            get => _newPasswordHintText;
            set => SetProperty(ref _newPasswordHintText, value);
        }

        /// <summary>
        /// The confirm password hint text
        /// </summary>
        private string _confirmPasswordHintText;

        public string ConfirmPasswordHintText
        {
            get => _confirmPasswordHintText;
            set => SetProperty(ref _confirmPasswordHintText, value);
        }

        /// <summary>
        /// The current saved password
        /// </summary>
        private SecureString _currentPassword;

        public SecureString CurrentPassword
        {
            get => _currentPassword;
            set => SetProperty(ref _currentPassword, value);
        }

        /// <summary>
        /// The current non-commit edited password
        /// </summary>
        private SecureString _newPassword;

        public SecureString NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        /// <summary>
        /// The current non-commit edited confirmed password
        /// </summary>
        private SecureString _confirmPassword;

        public SecureString ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        /// <summary>
        /// Indicates if the current text is in edit mode
        /// </summary>
        private bool _editing;

        public bool Editing
        {
            get => _editing;
            set => SetProperty(ref _editing, value);
        }

        #endregion

        #region Public Commands

        /// <summary>
        /// Puts the control into edit mode
        /// </summary>
        public ICommand EditCommand { get; set; }

        /// <summary>
        /// Cancels out of edit mode
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// Commits the edits and saves the value
        /// as well as goes back to non-edit mode
        /// </summary>
        public ICommand SaveCommand { get; set; }

        #endregion

        #region Constructor 

        /// <summary>
        /// Default constructor
        /// </summary>
        public PasswordEntryViewModel()
        {
            // Create commands
            EditCommand = new RelayCommand(Edit);
            CancelCommand = new RelayCommand(Cancel);
            SaveCommand = new RelayCommand(Save);

            // Set default hints
            // TODO: Replace with localization text
            CurrentPasswordHintText = "Current Password";
            NewPasswordHintText = "New Password";
            ConfirmPasswordHintText = "Confirm Password";
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Puts the control into edit mode
        /// </summary>
        public void Edit()
        {
            // Clear all password
            NewPassword = new SecureString();
            ConfirmPassword = new SecureString();

            // Go into edit mode
            Editing = true;
        }

        /// <summary>
        /// Cancels out of edit mode
        /// </summary>
        public void Cancel()
        {
            Editing = false;
        }

        /// <summary>
        /// Commits the content and exits out of edit mode
        /// </summary>
        public void Save()
        {
            // Make sure current password is correct
            // TODO: This will come from the real back-end store of this users password
            //       or via asking the web server to confirm it
            var storedPassword = "Testing";

            // Confirm current password is a match
            // NOTE: Typically this isn't done here, it's done on the server
            if (storedPassword != CurrentPassword.Unsecure())
            {
                // Let user know
                UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Wrong password",
                    Message = "The current password is invalid"
                });

                return;
            }

            // Now check that the new and confirm password match
            if (NewPassword.Unsecure() != ConfirmPassword.Unsecure())
            {
                // Let user know
                UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Password mismatch",
                    Message = "The new and confirm password do not match"
                });

                return;
            }

            // Check we actually have a password
            if (NewPassword.Unsecure().Length == 0)
            {
                // Let user know
                UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Password too short",
                    Message = "You must enter a password!"
                });

                return;
            }

            // Set the edited password to the current value
            CurrentPassword = new SecureString();
            foreach (var c in NewPassword.Unsecure().ToCharArray())
                CurrentPassword.AppendChar(c);

            Editing = false;
        }

        #endregion
    }
}
