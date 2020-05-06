using System.Collections.ObjectModel;

namespace Quan.Word.Core
{
    /// <summary>
    /// A view model for a menu item
    /// </summary>
    public class MenuViewModel : ViewModelBase
    {
        /// <summary>
        /// The items in this menu
        /// </summary>
        public ObservableCollection<MenuItemViewModel> Items { get; set; }
    }
}
