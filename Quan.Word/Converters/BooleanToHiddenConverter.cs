﻿using System;
using System.Globalization;
using System.Windows;

namespace Quan.Word
{
    public class BooleanToHiddenConverter : BaseValueConverter<bool, Visibility>
    {
        public override Visibility Convert(bool value, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return value ? Visibility.Hidden : Visibility.Visible;
            return value ? Visibility.Visible : Visibility.Hidden;
        }

        public override bool ConvertBack(Visibility value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
