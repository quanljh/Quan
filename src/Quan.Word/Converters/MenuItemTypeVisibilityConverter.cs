using Quan.Word.Core;
using System;
using System.Globalization;
using System.Windows;

namespace Quan.Word
{
    /// <summary>
    /// A converter that takes in a <see cref="MenuItemType"/>and returns a <see cref="Visibility"/>
    /// based on the given Parameter being the same as the menu item type
    /// </summary>
    public class MenuItemTypeVisibilityConverter : BaseValueConverter<MenuItemType, Visibility>
    {
        public override Visibility Convert(MenuItemType value, object parameter, CultureInfo culture)
        {
            if (parameter is string para && Enum.TryParse(para, out MenuItemType type))
                return value == type ? Visibility.Visible : Visibility.Collapsed;
            return Visibility.Collapsed;
        }

        public override MenuItemType ConvertBack(Visibility value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
