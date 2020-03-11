namespace Quan.Word.Core
{
    /// <summary>
    /// A view model for each Chat message thread items attachment
    /// (in this case an image) in the a Chat thread
    /// </summary>
    public class ChatMessageListItemImageAttachmentModel : ViewModelBase
    {
        #region Private Members

        /// <summary>
        /// The thumbnail URL of this attachment
        /// </summary>
        private string mThumbnailUrl;

        #endregion

        /// <summary>
        /// The title of this image file
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The orginal file name of the attachment
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The file size in bytes of this attchment
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// The thumbnail URL of this attachment
        /// </summary>
        public string ThumbnaiUrl
        {
            get => mThumbnailUrl;
            set
            {
                // If value hasn't changed, return
                if (value != null && value == mThumbnailUrl)
                    return;

                //Update value
                mThumbnailUrl = value;

                //TODO: Download image from website
                //      Save file to local storage/cache
                //      Set LocalFilePath value
                //
                //      For now, just set the file path directory
                LocalFilePath = "/Images/Samples/aijiang.jpg";
            }
        }

        public string LocalFilePath { get; set; }
    }
}
