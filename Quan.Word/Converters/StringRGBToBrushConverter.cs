using System;
using System.Globalization;
using System.Windows.Media;

namespace Quan.Converters
{
    public class StringRGBToBrushConverter : BaseValueConverter<string, SolidColorBrush>
    {
        public override SolidColorBrush Convert(string value, object parameter, CultureInfo culture)
        {
            return new BrushConverter().ConvertFrom($"#{value}") as SolidColorBrush;
        }

        public override string ConvertBack(SolidColorBrush value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
