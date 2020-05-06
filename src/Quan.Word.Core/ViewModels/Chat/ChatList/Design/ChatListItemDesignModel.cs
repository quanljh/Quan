namespace Quan.Word.Core
{
    public class ChatListItemDesignModel : ChatListItemViewModel
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
            Message = "This new Chat app is awesome! I bet it will be fast too";
            ProfilePictureRGB = "#3099c5";
        }

        #endregion

    }
}
