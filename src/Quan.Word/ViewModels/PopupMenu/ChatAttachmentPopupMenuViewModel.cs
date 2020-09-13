using Quan.Word.Core;
using System.Collections.ObjectModel;

namespace Quan.Word
{
    /// <summary>
    /// A view model for any popup menus
    /// </summary>
    public class ChatAttachmentPopupMenuViewModel : BasePopupMenuViewModel
    {
        #region Public Properties

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChatAttachmentPopupMenuViewModel()
        {
            Content = new MenuViewModel()
            {
                Items = new ObservableCollection<MenuItemViewModel>()
                {
                    new MenuItemViewModel{ Text = "Attach a file...",Type = MenuItemType.Header},
                    new MenuItemViewModel{ Text = "From Computer", Icon = IconType.File},
                    new MenuItemViewModel{ Text = "From Pictures", Icon = IconType.Picture},
                }
            };
        }

        #endregion
    }
}
