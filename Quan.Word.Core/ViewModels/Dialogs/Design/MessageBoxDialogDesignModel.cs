namespace Quan.Word.Core
{
    /// <summary>
    /// The design-time data for a <see cref="MessageBoxDialogDesignModel"/>
    /// </summary>
    public class MessageBoxDialogDesignModel : MessageBoxDialogViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static MessageBoxDialogDesignModel Instance => new MessageBoxDialogDesignModel();

        #endregion

        #region Contructor

        public MessageBoxDialogDesignModel()
        {
            Title = "Send Message";
            Message = "Thank you for writing a nice message :)";
            OkText = "OK";
        }

        #endregion
    }
}
