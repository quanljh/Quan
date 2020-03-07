using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Quan
{
    /// <summary>
    /// Scorll an items control to the bottom when the data context changes
    /// </summary>
    public class ScrollToBottomOnLoadProperty : BaseAttachedProperty<ScrollToBottomOnLoadProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //Don't do this in design time
            if (DesignerProperties.GetIsInDesignMode(sender))
                return;

            if (!(sender is ScrollViewer control))
                return;

            // Scroll content to bottom when context changes
            control.DataContextChanged -= ControlOnDataContextChanged;
            control.DataContextChanged += ControlOnDataContextChanged;
        }

        private void ControlOnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Scroll to bottom
            (sender as ScrollViewer)?.ScrollToBottom();
        }
    }


}
