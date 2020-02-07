using System;
using System.Globalization;

namespace Quan.Converters
{
    /// <summary>
    /// A converter that takes in date and converts it to a user friendly time
    /// </summary>
    public class TimeToDisplayTimeConverter : BaseValueConverter<DateTimeOffset, string>
    {
        public override string Convert(DateTimeOffset value, object parameter, CultureInfo culture)
        {
            //If it is today, reture just time. Otherwise, return a full date
            return value.ToLocalTime().ToString(value.Date == DateTimeOffset.UtcNow.Date ? "HH:mm" : "HH:mm,yyyy/MM/dd");
        }

        public override DateTimeOffset ConvertBack(string value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
