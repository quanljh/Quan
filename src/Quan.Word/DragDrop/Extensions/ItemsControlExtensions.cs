using Quan.Word.ViewHelper;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Quan.Word
{
    public static class ItemsControlExtensions
    {
        public static CollectionViewGroup FindGroup(this ItemsControl itemsControl, Point position)
        {
            if (itemsControl.Items.Groups == null || itemsControl.Items.Groups.Count == 0)
                return null;

            // Get the child element of this items control that is located at given position
            if (!(itemsControl.InputHitTest(position) is DependencyObject element))
                return null;

            // Get the group item of this item control
            var groupItem = element.FindVisualParent<GroupItem>();

            // Drag after last item - get group of it
            if (groupItem == null && itemsControl.Items.Count > 0)
            {
                // Use ItemContainerGenerator Get the last item as element
                if (itemsControl
                    .ItemContainerGenerator
                    .ContainerFromItem(itemsControl.Items.GetItemAt(itemsControl.Items.Count - 1)) is FrameworkElement lastItem)
                {
                    // Point: Represents an x- and y-coordinate pair in two-dimensional space
                    // Get the end point of current screen coordinates
                    // from last item's lower right corner point using PointToScreen Method
                    var itemEndpoint = lastItem.PointToScreen(new Point(lastItem.ActualWidth, lastItem.ActualHeight));

                    // Get the point in current screen coordinates from mouse click position
                    var positionToScreen = itemsControl.PointToScreen(position);
                }
            }
        }
    }
}
