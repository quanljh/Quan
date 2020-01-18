using Quan.DataModels;
using Quan.Pages;
using System;
using System.Diagnostics;
using System.Globalization;

namespace Quan.Converters
{
    public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Find the appropriate page
            if (value is ApplicationPage page)
            {
                switch (page)
                {
                    case ApplicationPage.login:
                        return new LoginPage();
                    default:
                        Debugger.Break();
                        return null;
                }
            }

            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
