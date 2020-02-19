using Quan.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Quan
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
                    if (!(child is TextEntryControl control))
                        continue;

                    // Set it's margin to the given value
                    control.Label.SizeChanged += (sss, eee) =>
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
                if (!(child is TextEntryControl control))
                    continue;

                // Find if this value is larger than the other controls
                maxSize = Math.Max(maxSize, control.Label.RenderSize.Width + control.Label.Margin.Left + control.Label.Margin.Right);
            }

            // Create a grid length converter
            var gridLength = (GridLength)new GridLengthConverter().ConvertFromString(maxSize.ToString());

            // For each child...
            foreach (var child in panel.Children)
            {
                // Ignore any non-text entry controls
                if (!(child is TextEntryControl control))
                    continue;

                // Set each controls LabelWidth value to the max size
                control.LabelWidth = gridLength;
            }
        }
    }
}
