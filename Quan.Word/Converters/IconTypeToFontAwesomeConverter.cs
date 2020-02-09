using Quan.Word.Core;
using System;
using System.Globalization;

namespace Quan.Converters
{
    /// <summary>
    /// A converter that takes in a <see cref="MenuItemType"/>and returns
    /// the FontAwesome string for that Icon
    /// </summary>
    public class IconTypeToFontAwesomeConverter : BaseValueConverter<IconType, string>
    {
        public override string Convert(IconType value, object parameter, CultureInfo culture)
        {
            return value.ToFontAwesome();
        }

        public override IconType ConvertBack(string value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
