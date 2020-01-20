using Quan.DataModels;
using Quan.Pages;
using System;
using System.Diagnostics;
using System.Globalization;

namespace Quan.Converters
{
    public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPage, LoginPage>
    {
        public override LoginPage Convert(ApplicationPage value, object parameter, CultureInfo culture)
        {
            //Find the appropriate page
            switch (value)
            {
                case ApplicationPage.login:
                    return new LoginPage();
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override ApplicationPage ConvertBack(LoginPage value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
