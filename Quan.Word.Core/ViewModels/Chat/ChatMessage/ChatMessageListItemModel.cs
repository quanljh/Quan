using Prism.Mvvm;
using System;

namespace Quan.Word.Core
{
    /// <summary>
    /// A view model for each Chat message thread item in the a Chat thread
    /// </summary>
    public class ChatMessageListItemModel : BindableBase
    {
        /// <summary>
        /// The display name of the sender of the message
        /// </summary>
        private string _sendername;

        public string SenderName
        {
            get => _sendername;
            set => SetProperty(ref _sendername, value);
        }

        /// <summary>
        /// The latest message from this Chat
        /// </summary>
        private string _message;

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        /// <summary>
        /// The initials to show for the profile picture backgroud
        /// </summary>
        private string _initials;

        public string Initials
        {
            get => _initials;
            set => SetProperty(ref _initials, value);
        }

        /// <summary>
        /// The RGB values (in hex) for the background color of the profile picture
        /// For example FF00FF for Red and Blue mixed
        /// </summary>
        private string _profilePictureRGB;

        public string ProfilePictureRGB
        {
            get => _profilePictureRGB;
            set => SetProperty(ref _profilePictureRGB, value);
        }

        /// <summary>
        /// True if this item is currently selected
        /// </summary>
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        /// <summary>
        /// True if the message was sent by the signed user
        /// </summary>
        private bool _sentByMe;

        public bool SentByMe
        {
            get => _sentByMe;
            set => SetProperty(ref _sentByMe, value);
        }


        /// <summary>
        /// The time the message was read, or <see cref="DateTimeOffset.MinValue"/> if not read
        /// </summary>
        private DateTimeOffset _messageReadTime;

        public DateTimeOffset MessageReadTime
        {
            get => _messageReadTime;
            set => SetProperty(ref _messageReadTime, value);
        }

        /// <summary>
        /// True if the message has been read
        /// </summary>
        public bool MessageRead => MessageReadTime > DateTimeOffset.MinValue;

        /// <summary>
        /// The time the message was sent, or <see cref="DateTimeOffset.MinValue"/> if not read
        /// </summary>
        private DateTimeOffset _messageSentTime;

        public DateTimeOffset MessageSentTime
        {
            get => _messageSentTime;
            set => SetProperty(ref _messageSentTime, value);
        }

        /// <summary>
        /// A flag indicating if this item was added since the first main list of items was created
        /// Used as a flag fo animating in
        /// </summary>
        private bool _newItem;

        public bool NewItem
        {
            get => _newItem;
            set => SetProperty(ref _newItem, value);
        }

    }
}
