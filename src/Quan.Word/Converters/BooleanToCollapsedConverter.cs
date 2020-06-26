using System;
using System.Globalization;
using System.Windows;

namespace Quan.Word
{
    public class BooleanToCollapsedConverter : BaseValueConverter<bool, Visibility>
    {
        public override Visibility Convert(bool value, object parameter, CultureInfo culture)
        {
            if (parameter != null)
                return value ? Visibility.Collapsed : Visibility.Visible;
            return value ? Visibility.Visible : Visibility.Collapsed;
        }

        public override bool ConvertBack(Visibility value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
