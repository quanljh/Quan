using System.Windows.Input;

namespace Quan.Word
{
    /// <summary>
    /// The view model for a text entry to edit a string value
    /// </summary>
    public class TextEntryViewModel : ViewModelBase
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
        /// The current saved value
        /// </summary>
        private string _originalText;

        public string OriginalText
        {
            get => _originalText;
            set => SetProperty(ref _originalText, value);
        }

        /// <summary>
        /// The current non-commit edited text
        /// </summary>
        private string _editedText;

        public string EditedText
        {
            get => _editedText;
            set => SetProperty(ref _editedText, value);
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
        /// Default Constructor
        /// </summary>
        public TextEntryViewModel()
        {

            // Create commands
            EditCommand = new RelayCommand(Edit);
            CancelCommand = new RelayCommand(Cancel);
            SaveCommand = new RelayCommand(Save);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Puts the control into edit mode
        /// </summary>
        public void Edit()
        {
            // Set the edited text to the current value
            EditedText = OriginalText;

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
            // TODO: Save content
            OriginalText = EditedText;

            Editing = false;
        }

        #endregion
    }
}
