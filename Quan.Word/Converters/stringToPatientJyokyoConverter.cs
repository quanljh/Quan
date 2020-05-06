using System;
using System.Diagnostics;
using System.Globalization;

namespace Quan.Word
{
    public class stringToPatientJyokyoConverter : BaseValueConverter<string, string>
    {
        public override string Convert(string value, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "1":
                    return "来院待ち";
                case "2":
                    return "診察待ち";
                case "3":
                    return "診察中";
                case "4":
                    return "会計待ち";
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override string ConvertBack(string value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
