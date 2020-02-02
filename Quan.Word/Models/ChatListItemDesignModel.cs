using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quan.Models
{
    public class ChatListItemDesignModel : ChatListItemModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static ChatListItemDesignModel Instance => new ChatListItemDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChatListItemDesignModel()
        {
            Initials = "JH";
            Name = "Quan";
            Message = "This new chat app is awesome! I bet it will be fast too";
            ProfilePictureRGB = "#3099c5";
        }

        #endregion

    }
}
