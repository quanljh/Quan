using Quan.DataModels;
using Quan.Pages;
using System;
using System.Diagnostics;
using System.Globalization;

namespace Quan.Converters
{
    public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPage, object>
    {
        public override object Convert(ApplicationPage value, object parameter, CultureInfo culture)
        {
            //Find the appropriate page
            switch (value)
            {
                case ApplicationPage.login:
                    return new LoginPage();

                case ApplicationPage.Chat:
                    return new ChatPage();
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override ApplicationPage ConvertBack(object value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
