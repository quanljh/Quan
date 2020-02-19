using System;
using System.Globalization;

namespace Quan.Converters
{
    /// <summary>
    /// A converter that takes in a boolean and inverts it
    /// </summary>
    public class BooleanInvertConverter : BaseValueConverter<bool, bool>
    {
        public override bool Convert(bool value, object parameter, CultureInfo culture)
            => !value;

        public override bool ConvertBack(bool value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
