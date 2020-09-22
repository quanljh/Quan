using System.Threading.Tasks;

namespace Quan.Word
{
    /// <summary>
    /// A view model for each Chat message thread items attachment
    /// (in this case an image) in the a Chat thread
    /// </summary>
    public class ChatMessageListItemImageAttachmentModel : ViewModelBase
    {
        /// <summary>
        /// The title of this image file
        /// </summary>
        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// The orginal file name of the attachment
        /// </summary>
        private string _fileName;

        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }

        /// <summary>
        /// The file size in bytes of this attchment
        /// </summary>
        private long _fileSize;

        public long FileSize
        {
            get => _fileSize;
            set => SetProperty(ref _fileSize, value);
        }

        /// <summary>
        /// The thumbnail URL of this attachment
        /// </summary>
        private string _thumbnailUrl;

        public string ThumbnaiUrl
        {
            get => _thumbnailUrl;
            set
            {
                // If value hasn't changed, return
                if (value != null && value == _thumbnailUrl)
                    return;

                //Update value
                SetProperty(ref _thumbnailUrl, value);

                //TODO: Download image from website
                //      Save file to local storage/cache
                //      Set LocalFilePath value
                //
                //      For now, just set the file path directory
                Task.Delay(2000).ContinueWith(t => LocalFilePath = "/Images/Samples/aijiang.jpg");
            }
        }


        /// <summary>
        /// The local file path on this machine to the downloaded thumbnail
        /// </summary>
        private string _localFilePath;

        public string LocalFilePath
        {
            get => _localFilePath;
            set
            {
                if (SetProperty(ref _localFilePath, value))
                    RaisePropertyChanged(nameof(ImageLoaded));
            }
        }

        /// <summary>
        /// Indicates if an image has loaded
        /// </summary>
        public bool ImageLoaded => LocalFilePath != null;
    }
}
