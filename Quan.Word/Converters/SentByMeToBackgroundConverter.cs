using System;
using System.Globalization;
using System.Windows;

namespace Quan.Word
{
    /// <summary>
    /// A converter that takes in a boolean if a message was sent by me, and returns
    /// the correct background color
    /// </summary>
    public class SentByMeToBackgroundConverter : BaseValueConverter<bool, object>
    {
        public override object Convert(bool value, object parameter, CultureInfo culture)
        {
            return value ? Application.Current.FindResource("WordVeryLightBlueBrush") : Application.Current.FindResource("ForegroundLightBrush");
        }

        public override bool ConvertBack(object value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
