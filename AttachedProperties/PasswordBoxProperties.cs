using System.Windows;
using System.Windows.Controls;

namespace Quan.AttachedProperties
{
    public class PasswordBoxProperties
    {
        public static readonly DependencyProperty MonitorPasswordProperty =
            DependencyProperty.RegisterAttached("MonitorPassword", typeof(bool), typeof(PasswordBoxProperties), new PropertyMetadata(false, OnMonitorPasswordChanged));

        private static void OnMonitorPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= PasswordBoxOnPasswordChanged;
                if (e.NewValue is bool value && value)
                {
                    SetHasText(passwordBox, passwordBox.SecurePassword.Length > 0);
                    passwordBox.PasswordChanged += PasswordBoxOnPasswordChanged;
                }
            }
        }

        private static void PasswordBoxOnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                SetHasText(passwordBox, passwordBox.SecurePassword.Length > 0);
            }

        }

        public static void SetMonitorPassword(PasswordBox element, bool value)
        {
            element.SetValue(MonitorPasswordProperty, value);
        }

        public static bool GetMonitorPassword(PasswordBox element)
        {
            return (bool)element.GetValue(MonitorPasswordProperty);
        }


        public static readonly DependencyProperty HasTextProperty =
            DependencyProperty.RegisterAttached("HasText", typeof(bool), typeof(PasswordBoxProperties), new PropertyMetadata(false));

        public static void SetHasText(PasswordBox element, bool value)
        {
            element.SetValue(HasTextProperty, value);
        }

        //private static void SetHasText(PasswordBox element)
        //{
        //    element.SetValue(HasTextProperty, element.SecurePassword.Length > 0);
        //}

        public static bool GetHasText(PasswordBox element)
        {
            return (bool)element.GetValue(HasTextProperty);
        }
    }
}
