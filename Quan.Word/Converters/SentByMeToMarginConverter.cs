using System;
using System.Globalization;
using System.Windows;

namespace Quan.Converters
{
    /// <summary>
    /// A converter that takes in a boolean if a message was sent by me, then set left margin
    /// If not sent by me, set right margin
    /// </summary>
    public class SentByMeToMarginConverter : BaseValueConverter<bool, Thickness>
    {
        public override Thickness Convert(bool value, object parameter, CultureInfo culture)
        {
            return value ? new Thickness(100, 0, 0, 0) : new Thickness(0, 0, 100, 0);
        }

        public override bool ConvertBack(Thickness value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
