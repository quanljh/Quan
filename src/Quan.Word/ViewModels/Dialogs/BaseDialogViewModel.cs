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
        public string Title { get; set; }
    }
}
