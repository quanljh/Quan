using System;
using System.Windows;
using System.Windows.Controls;

namespace Quan.Word
{
    /// <summary>
    /// Match the label width of all text entry controls inside this panel
    /// </summary>
    public class TextEntryWidthMathcherProperty : BaseAttachedProperty<TextEntryWidthMathcherProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //Get the panel (grid typically)
            if (!(sender is Panel panel))
                return;


            SetWidths(panel);

            // Wait for panel to load
            RoutedEventHandler onLoaded = null;

            onLoaded += (ss, ee) =>
            {
                //Unhook
                panel.Loaded -= onLoaded;

                // Set widths
                SetWidths(panel);

                //loop each child
                foreach (var child in panel.Children)
                {
                    // Ignore any non-text entry controls
                    if (!(child is TextEntryControl control) && !(child is PasswordEntryControl))
                        continue;

                    // Get the label from the text entry or password entry
                    var label = child is TextEntryControl ? (child as TextEntryControl).Label : (child as PasswordEntryControl).Label;

                    // Set it's margin to the given value
                    label.SizeChanged += (sss, eee) =>
                    {
                        // Update widths
                        SetWidths(panel);
                    };
                }


            };

            // Hook into the Loaded event
            panel.Loaded += onLoaded;

        }

        private void SetWidths(Panel panel)
        {
            var maxSize = 0d;

            // For each child...
            foreach (var child in panel.Children)
            {
                // Ignore any non-text entry controls
                if (!(child is TextEntryControl) && !(child is PasswordEntryControl))
                    continue;

                // Get the label from the text entry or password entry
                var label = child is TextEntryControl ? (child as TextEntryControl).Label : (child as PasswordEntryControl).Label;


                // Find if this value is larger than the other controls
                maxSize = Math.Max(maxSize, label.RenderSize.Width + label.Margin.Left + label.Margin.Right);
            }

            // Create a grid length converter
            var gridLength = (GridLength)new GridLengthConverter().ConvertFromString(maxSize.ToString());

            // For each child...
            foreach (var child in panel.Children)
            {
                if (child is TextEntryControl text)
                    // Set each controls LabelWidth value to the max size
                    text.LabelWidth = gridLength;
                else if (child is PasswordEntryControl pass)
                    // Set each controls LabelWidth value to the max size
                    pass.LabelWidth = gridLength;
            }
        }
    }
}
