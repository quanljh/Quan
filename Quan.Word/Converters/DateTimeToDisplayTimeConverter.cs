using System;
using System.Globalization;

namespace Quan.Word
{
    /// <summary>
    /// A converter that takes in datetime and converts it to a user friendly time
    /// </summary>
    public class DateTimeToDisplayTimeConverter : BaseValueConverter<DateTime, string>
    {
        public override string Convert(DateTime value, object parameter, CultureInfo culture)
        {
            //If it is today, reture just time. Otherwise, return a full date
            return value.ToString("yyyy年MM月dd日");
        }

        public override DateTime ConvertBack(string value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
