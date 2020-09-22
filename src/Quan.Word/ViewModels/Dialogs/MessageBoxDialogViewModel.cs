namespace Quan.Word
{
    /// <summary>
    /// Details for a message box dialog
    /// </summary>
    public class MessageBoxDialogViewModel : BaseDialogViewModel
    {
        /// <summary>
        /// The message to display
        /// </summary>
        private string _message;

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        /// <summary>
        /// The text to use for the OK button
        /// </summary>
        private string _okText = "OK";

        public string OkText
        {
            get => _okText;
            set => SetProperty(ref _okText, value);
        }

    }
}
