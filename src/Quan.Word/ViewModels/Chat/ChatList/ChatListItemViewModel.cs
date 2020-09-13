using Quan.Word.Core;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static Quan.Word.DI;

namespace Quan.Word
{
    /// <summary>
    /// A view model for each Chat list item in the overview Chat list
    /// </summary>
    public class ChatListItemViewModel : ViewModelBase
    {
        #region Public Properties

        /// <summary>
        /// The display name of this Chat list
        /// </summary>
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
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
        /// True if there are unread messages in this Chat
        /// </summary>
        private bool _newContentAvailable;

        public bool NewContentAvailable
        {
            get => _newContentAvailable;
            set => SetProperty(ref _newContentAvailable, value);
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

        #endregion

        #region Public Commands

        public ICommand OpenMessageCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChatListItemViewModel()
        {
            // Create commands
            OpenMessageCommand = new RelayCommand(OpenMessage);
        }

        #endregion

        #region Command Methods

        public void OpenMessage()
        {
            if (Name == "Jesse")
            {
                ApplicationVM.GoToPage(ApplicationPage.Login, new LoginViewModel
                {
                    Email = "jesse@helloworld.com"
                });
                return;
            }

            ApplicationVM.GoToPage(ApplicationPage.Chat, new ChatMessageListViewModel
            {
                DisplayTitle = "Quanljh",

                Items = new ObservableCollection<ChatMessageListItemModel>()
                {
                    new ChatMessageListItemModel
                    {
                        Message = Message,
                        Initials = Initials,
                        MessageSentTime = DateTime.UtcNow,
                        ProfilePictureRGB = "FF00FF",
                        SenderName = "Luke",
                        SentByMe = true,
                    },
                    new ChatMessageListItemModel
                    {
                        Message = "A received message",
                        Initials = Initials,
                        MessageSentTime = DateTime.UtcNow,
                        ProfilePictureRGB = "FF0000",
                        SenderName = "Parnell",
                        SentByMe = false,
                    },
                    new ChatMessageListItemModel
                    {
                        Message = "A received message",
                        Initials = Initials,
                        MessageSentTime = DateTime.UtcNow,
                        ProfilePictureRGB = "FF0000",
                        SenderName = "Parnell",
                        SentByMe = false,
                    },
                    new ChatMessageListItemModel
                    {
                        Message = Message,
                        Initials = Initials,
                        MessageSentTime = DateTime.UtcNow,
                        ProfilePictureRGB = "FF00FF",
                        SenderName = "Luke",
                        SentByMe = true,
                    },
                    new ChatMessageListItemModel
                    {
                        Message = "A received message",
                        Initials = Initials,
                        MessageSentTime = DateTime.UtcNow,
                        ProfilePictureRGB = "FF0000",
                        SenderName = "Parnell",
                        SentByMe = false,
                    },
                    new ChatMessageListItemModel
                    {
                        Message = "A received message",
                        Initials = Initials,
                        ImageAttachment = new ChatMessageListItemImageAttachmentModel()
                        {
                            ThumbnaiUrl = "http://www.quanljh.com"
                        },
                        MessageSentTime = DateTime.UtcNow,
                        ProfilePictureRGB = "FF0000",
                        SenderName = "Parnell",
                        SentByMe = false,
                    }
                }
            });
        }

        #endregion
    }
}
