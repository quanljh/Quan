using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quan.ViewModels.Base;

// ReSharper disable once CheckNamespace
namespace Quan.ViewModels
{
    /// <summary>
    /// A view model for each chat list item in the overview chat list
    /// </summary>
    public class ChatListItemViewModel:ViewModelBase
    {
        /// <summary>
        /// The display name of this chat list
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The latest message from this chat
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The initials to show for the profile picture backgroud
        /// </summary>
        public string Initials { get; set; }

        /// <summary>
        /// The RGB values (in hex) for the background color of the profile picture
        /// For example FF00FF for Red and Blue mixed
        /// </summary>
        public string ProfilePictureRGB { get; set; }
    }
}
