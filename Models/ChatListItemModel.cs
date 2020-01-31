using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace Quan.Models
{
    /// <summary>
    /// A view model for each chat list item in the overview chat list
    /// </summary>
    public class ChatListItemModel : BindableBase
    {
        /// <summary>
        /// The display name of this chat list
        /// </summary>
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// The latest message from this chat
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
        /// True if there are unread messages in this chat
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

    }
}
