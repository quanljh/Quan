using Quan.Word.Core;

namespace Quan.Word
{
    /// <summary>
    /// A view model for a menu item
    /// </summary>
    public class MenuItemViewModel : ViewModelBase
    {
        /// <summary>
        /// The text to display for the menu item
        /// </summary>
        private string _text;

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        /// <summary>
        /// The icon for this menu item
        /// </summary>
        private IconType _icon;

        public IconType Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        /// <summary>
        /// The type of this menu item
        /// </summary>
        private MenuItemType _type;

        public MenuItemType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }
    }
}
