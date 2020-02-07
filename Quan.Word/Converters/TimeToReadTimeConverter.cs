using System;
using System.Globalization;

namespace Quan.Converters
{
    /// <summary>
    /// A converter that takes in date and converts it to a user friendly message read time
    /// </summary>
    public class TimeToReadTimeConverter : BaseValueConverter<DateTimeOffset, string>
    {
        public override string Convert(DateTimeOffset value, object parameter, CultureInfo culture)
        {
            //If it is not read...
            if (value == DateTimeOffset.MinValue)
                //Show nothing
                return string.Empty;

            //If it is today, reture just time. Otherwise, return a full date
            return value.Date == DateTimeOffset.UtcNow.Date ? $"Read {value.ToLocalTime():HH:mm}" : $"Read {value.ToLocalTime():HH:mm,yyyy/MM/dd}";
        }

        public override DateTimeOffset ConvertBack(string value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
