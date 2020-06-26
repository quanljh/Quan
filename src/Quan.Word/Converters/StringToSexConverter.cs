using System;
using System.Globalization;

namespace Quan.Word
{
    /// <summary>
    /// A converter that takes in string and converts it to a PatientSex
    /// </summary>
    public class StringToSexConverter : BaseValueConverter<string, string>
    {
        public override string Convert(string value, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "1":
                    return "男";
                case "2":
                    return "女";

                default: return value;
            }
        }

        public override string ConvertBack(string value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
