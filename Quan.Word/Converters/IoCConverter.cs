using Quan.Word.Core;
using System;
using System.Diagnostics;
using System.Globalization;

namespace Quan.Converters
{
    /// <summary>
    /// Converts a string name to a service pulled from the IoC container
    /// </summary>
    public class IoCConverter : BaseValueConverter<string, object>
    {
        public override object Convert(string value, object parameter, CultureInfo culture)
        {
            //Find the appropriate page
            switch (value)
            {
                case nameof(ApplicationViewModel):
                    return IoC.Application;

                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override string ConvertBack(object value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
