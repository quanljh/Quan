namespace Quan.Word.Core
{
    /// <summary>
    /// A base view model for any dialogs
    /// </summary>
    public abstract class BaseDialogViewModel : ViewModelBase
    {
        /// <summary>
        /// The title of the message box
        /// </summary>
        public string Title { get; set; }
    }
}
