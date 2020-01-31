using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quan.Models
{
    public class ChatListDesignModel : ChatListModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static ChatListDesignModel Instance => new ChatListDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChatListDesignModel()
        {
            Items = new ObservableCollection<ChatListItemModel>()
            {
                new ChatListItemModel
                {
                    Initials = "JH",
                    Name = "Quan",
                    Message = "This new chat app is awesome! I bet it will be fast too",
                    ProfilePictureRGB = "#3099c5",
                    NewContentAvailable = true
                },
                new ChatListItemModel
                {
                    Name = "Jesse",
                    Initials = "JA",
                    Message = "Hey dude, here are the new icons",
                    ProfilePictureRGB = "#fe4503"
                },
                new ChatListItemModel
                {
                    Name = "Parnell",
                    Initials = "PL",
                    Message = "The new server is up, got 192.168.1.1",
                    ProfilePictureRGB = "#00d405",
                    IsSelected = true,
                },
                new ChatListItemModel
                {
                    Name = "Luke",
                    Initials = "LM",
                    Message = "This chat app is awesome! I bet it will be fast too",
                    ProfilePictureRGB = "#3099c5"
                },
                new ChatListItemModel
                {
                    Name = "Jesse",
                    Initials = "JA",
                    Message = "Hey dude, here are the new icons",
                    ProfilePictureRGB = "#fe4503"
                },
                new ChatListItemModel
                {
                    Name = "Parnell",
                    Initials = "PL",
                    Message = "The new server is up, got 192.168.1.1",
                    ProfilePictureRGB = "#00d405"
                },
                new ChatListItemModel
                {
                    Name = "Luke",
                    Initials = "LM",
                    Message = "This chat app is awesome! I bet it will be fast too",
                    ProfilePictureRGB = "#3099c5"
                },
                new ChatListItemModel
                {
                    Name = "Jesse",
                    Initials = "JA",
                    Message = "Hey dude, here are the new icons",
                    ProfilePictureRGB = "#fe4503"
                },
                new ChatListItemModel
                {
                    Name = "Parnell",
                    Initials = "PL",
                    Message = "The new server is up, got 192.168.1.1",
                    ProfilePictureRGB = "#00d405"
                }
            };
        }

        #endregion
    }
}
