using System.Windows;
using System.Windows.Controls;

namespace Quan
{
    /// <summary>
    /// The IsBusy attached property for a anything that wants to flag if the control is busy
    /// </summary>
    public class IsFocusedProperty : BaseAttachedProperty<IsFocusedProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Control control)
            {
                //Focus this control once loaded
                control.Loaded += (ss, ee) => control.Focus();
            }
        }
    }
}
