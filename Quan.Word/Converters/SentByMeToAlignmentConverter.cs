using System;
using System.Globalization;
using System.Windows;

namespace Quan.Converters
{
    /// <summary>
    /// A converter that takes in a boolean if a message was sent by me, then aligns to the right
    /// If not sent by me, aligns to the left
    /// </summary>
    public class SentByMeToAlignmentConverter : BaseValueConverter<bool, HorizontalAlignment>
    {
        public override HorizontalAlignment Convert(bool value, object parameter, CultureInfo culture)
        {
            return value ? HorizontalAlignment.Right : HorizontalAlignment.Left;
        }

        public override bool ConvertBack(HorizontalAlignment value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
