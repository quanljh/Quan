namespace Quan.Word
{
    /// <summary>
    /// A base view model for any dialogs
    /// </summary>
    public abstract class BaseDialogViewModel : ViewModelBase
    {
        /// <summary>
        /// The title of the message box
        /// </summary>
        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
