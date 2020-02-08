using System;
using System.Globalization;
using System.Windows;

namespace Quan.Converters
{
    public class HorizontalAlignmentConverter : BaseValueConverter<object, HorizontalAlignment>
    {
        public override HorizontalAlignment Convert(object value, object parameter, CultureInfo culture)
        {
            return (HorizontalAlignment)value;
        }

        public override object ConvertBack(HorizontalAlignment value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
