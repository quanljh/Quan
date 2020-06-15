using Quan.Word.ViewHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Quan.Word
{
    public static class ItemsControlExtensions
    {
        /// <summary>
        /// Find Group by mouse start position
        /// </summary>
        /// <param name="itemsControl">The source items control</param>
        /// <param name="position">The drag start position</param>
        /// <returns>Group in mouse start position, if no groups return Null</returns>
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

                    switch (itemsControl.GetItemsPanelOrientation())
                    {
                        case Orientation.Horizontal:
                            // assume LeftToRight
                            // if mouse position's X-coordinate is greater than last item such as blank space at right, get last group from last item
                            groupItem = itemEndpoint.X <= positionToScreen.X ? lastItem.FindVisualParent<GroupItem>() : null;
                            break;

                        case Orientation.Vertical:
                            // if mouse position's Y-coordinate is greater than last item such as blank space at bottom, get last group from last item
                            groupItem = itemEndpoint.Y <= positionToScreen.Y ? lastItem.FindVisualParent<GroupItem>() : null;
                            break;
                    }
                }

                if (groupItem != null)
                    return groupItem.Content as CollectionViewGroup;
            }

            return null;
        }

        /// <summary>
        /// Get a <see cref="ItemsControl"/>'s orientation
        /// </summary>
        /// <param name="itemsControl">The ItemsControl</param>
        /// <returns>Return <see cref="Orientation.Vertical"/> if not found items panel</returns>
        public static Orientation GetItemsPanelOrientation(this ItemsControl itemsControl)
        {
            // Get itemsPanelOrientation
            var itemsPanelOrientation = DragDrop.GetItemsPanelOrientation(itemsControl);
            // If already set orientation
            if (itemsPanelOrientation.HasValue)
                return itemsPanelOrientation.Value;

            if (itemsControl is TabControl tabControl)
                // if tab headers align left or right then Orientation is Vertical 
                return tabControl.TabStripPlacement == Dock.Left || tabControl.TabStripPlacement == Dock.Right
                    ? Orientation.Vertical
                    : Orientation.Horizontal;

            // get ItemsPresenter or ScrollContentPresenter
            var itemsPresenter = itemsControl.FindVisualChild<ItemsPresenter>() ?? itemsControl.FindVisualChild<ScrollContentPresenter>() as UIElement;

            if (itemsPresenter != null && VisualTreeHelper.GetChildrenCount(itemsPresenter) > 0)
            {
                // Get ItemsPanel from ItemsPresenter first child
                var itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 0);
                // Get orientation property info from ItemsPanel use reflection
                var orientationProperty = itemsPanel.GetType().GetProperty(nameof(Orientation), typeof(Orientation));

                if (orientationProperty != null)
                    // Return orientation property of ItemsPanel
                    return (Orientation)orientationProperty.GetValue(itemsPanel);
            }

            // Return default orientation if we not found ItemsPanel or ScrollContentPresenter
            return Orientation.Vertical;
        }

        /// <summary>
        ///  Get a <see cref="ItemsControl"/>'s FlowDirection
        /// </summary>
        /// <param name="itemsControl">The ItemsControl</param>
        /// <returns>Return <see cref="FlowDirection.LeftToRight"/> If we not found items panel</returns>
        public static FlowDirection GetItemsPanelFlowDirection(this ItemsControl itemsControl)
        {
            // get ItemsPresenter or ScrollContentPresenter
            var itemsPresenter = itemsControl.FindVisualChild<ItemsPresenter>() ?? itemsControl.FindVisualChild<ScrollContentPresenter>() as UIElement;

            if (itemsPresenter != null && VisualTreeHelper.GetChildrenCount(itemsPresenter) > 0)
            {
                // Get ItemsPanel from ItemsPresenter first child
                var itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 0);
                // Get FlowDirection property info from ItemsPanel use reflection
                var flowDirectionProperty = itemsPanel.GetType().GetProperty(nameof(FlowDirection), typeof(FlowDirection));

                if (flowDirectionProperty != null)
                    // Return orientation property of ItemsPanel use GetValue
                    return (FlowDirection)flowDirectionProperty.GetValue(itemsPanel);
            }

            // Return default FlowDirection if we not found ItemsPanel or ScrollContentPresenter
            return FlowDirection.LeftToRight;
        }

        /// <summary>
        /// Get ItemContainer from child element
        /// </summary>
        /// <param name="itemsControl">The ItemsControl</param>
        /// <param name="child">The child element</param>
        /// <returns></returns>
        public static UIElement GetItemContainer(this ItemsControl itemsControl, DependencyObject child)
        {
            // Get item's type. if it's items control then get items container type
            // e.g. if items control is DataGrid, the item's type is DataGridRow
            var itemType = GetItemContainerType(itemsControl, out var isItemContainer);

            if (itemType != null)
            {
                return isItemContainer
                    ? (UIElement)child.FindVisualParent(itemType, itemsControl)
                    // Find item DependencyObject with specific item type from visual tree 
                    : (UIElement)child.FindVisualParent(itemType, itemsControl, itemsControl.GetType());
            }

            return null;
        }

        /// <summary>
        /// Get ItemContainer from mouse position
        /// </summary>
        /// <param name="itemsControl">The ItemsControl</param>
        /// <param name="position">The mouse position</param>
        /// <returns></returns>
        public static UIElement GetItemContainerAt(this ItemsControl itemsControl, Point position)
        {
            var inputElement = itemsControl.InputHitTest(position);

            if (inputElement is UIElement uiElement)
                return GetItemContainer(itemsControl, uiElement);

            // ContentElement's such as Run's within TextBlock's could not be used as drop target items, because they are not UIElement's
            if (inputElement is ContentElement contentElement)
                return GetItemContainer(itemsControl, contentElement);

            return null;
        }

        public static UIElement GetItemContainerAt(this ItemsControl itemsControl, Point position,
            Orientation searchDirection)
        {
            var itemContainerType = GetItemContainerType(itemsControl, out var isItemContainer);

            Geometry hitTestGeometry;

            if (typeof(TreeViewItem).IsAssignableFrom(itemContainerType))
                hitTestGeometry = new LineGeometry(new Point(0, position.Y), new Point(itemsControl.RenderSize.Width, position.Y));
            else
            {
                var geometryGroup = new GeometryGroup();
                geometryGroup.Children.Add(new LineGeometry(new Point(0, position.Y), new Point(itemsControl.RenderSize.Width, position.Y)));
                geometryGroup.Children.Add(new LineGeometry(new Point(position.X, 0), new Point(position.X, itemsControl.RenderSize.Height)));
                hitTestGeometry = geometryGroup;
            }

            var hits = new HashSet<DependencyObject>();

            VisualTreeHelper.HitTest(itemsControl, obj =>
            {
                // Viewport3D is not good for us
                // Stop on ScrollBar to improve performance (e.g. at DataGrid)
                if (obj is Viewport3D || (itemsControl is DataGrid && obj is ScrollBar))
                    return HitTestFilterBehavior.Stop;
                return HitTestFilterBehavior.Continue;
            }, result =>
            {
                var itemContainer = isItemContainer
                    ? result.VisualHit.FindVisualParent(itemContainerType, itemsControl)
                    : result.VisualHit.FindVisualParent(itemContainerType, itemsControl, itemsControl.GetType());

                if (itemContainer != null && ((UIElement)itemContainer).IsVisible)
                {
                    if (itemContainer is TreeViewItem tvItem)
                    {
                        var tv = tvItem.FindVisualParent<TreeView>();
                        if (tv == itemsControl)
                            hits.Add(itemContainer);
                    }
                    else
                    {
                        if (itemsControl.ItemContainerGenerator.IndexFromContainer(itemContainer) >= 0)
                            hits.Add(itemContainer);
                    }
                }

                return HitTestResultBehavior.Continue;
            }, new GeometryHitTestParameters(hitTestGeometry));

            return GetClosest(itemsControl, hits, position, searchDirection);
        }

        /// <summary>
        /// Get <see cref="ItemsControl"/>'s ItemContainer type
        /// </summary>
        /// <param name="itemsControl">The ItemsControl</param>
        /// <param name="isItemContainer">Whether ItemsControl has ItemContainer</param>
        /// <returns>Return Null if the control currently has no items or not has ItemContainer</returns>
        public static Type GetItemContainerType(this ItemsControl itemsControl, out bool isItemContainer)
        {
            // determines if the itemsControl is not a listView, ListBox or TreeView
            isItemContainer = false;

            if (itemsControl is TabControl)
                return typeof(TabItem);

            if (itemsControl is DataGrid)
                return typeof(DataGridRow);

            if (itemsControl is ListView)
                return typeof(ListViewItem);

            if (itemsControl is ListBox)
                return typeof(ListBoxItem);

            if (itemsControl is TreeView)
                return typeof(TreeViewItem);

            // Otherwise look for the control's ItemsPresenter, get it's child panel and the first
            // child of thad *should* be an item container.
            //
            // If the control currently has no items, we're out of luck.
            if (itemsControl.Items.Count > 0)
            {
                var itemsPresenters = itemsControl.FindVisualChildren<ItemsPresenter>();

                foreach (var itemsPresenter in itemsPresenters)
                {
                    if (VisualTreeHelper.GetChildrenCount(itemsPresenter) > 0)
                    {
                        var panel = VisualTreeHelper.GetChild(itemsPresenter, 0);
                        var itemContainer = VisualTreeHelper.GetChildrenCount(panel) > 0
                            ? VisualTreeHelper.GetChild(panel, 0)
                            : null;

                        // Ensure that this actually *is* an item container by checking it with ItemContainerGenerator
                        if (itemContainer != null && !(itemContainer is GroupItem) &&
                            itemsControl.ItemContainerGenerator.IndexFromContainer(itemContainer) != -1)
                        {
                            isItemContainer = true;
                            return itemContainer.GetType();
                        }
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// Get <see cref="ItemsControl"/>'s selected items
        /// </summary>
        /// <param name="itemsControl">The ItemsControl</param>
        /// <returns>Return empty enumerable if the itemsControl not a selector</returns>
        public static IEnumerable GetSelectedItems(this ItemsControl itemsControl)
        {
            if (itemsControl is MultiSelector multiSelector)
                return multiSelector.SelectedItems;

            if (itemsControl is ListBox listBox)
            {
                if (listBox.SelectionMode == SelectionMode.Single)
                    return Enumerable.Repeat(listBox.SelectedItem, 1);

                // Otherwise, listbox SelectionModel is Multiple
                return listBox.SelectedItems;
            }

            if (itemsControl is TreeView treeView)
                return Enumerable.Repeat(treeView.SelectedItem, 1);

            if (itemsControl is Selector selector)
                return Enumerable.Repeat(selector.SelectedItem, 1);

            return Enumerable.Empty<object>();
        }

        /// <summary>
        /// Detect whether the ItemsControl supports multiple selection
        /// </summary>
        /// <param name="itemsControl">The ItemsControl</param>
        /// <returns>Return false as default</returns>
        public static bool CanSelectMultipleItems(this ItemsControl itemsControl)
        {
            if (itemsControl is MultiSelector multiSelector)
            {
                // The CanSelectMultipleItems property is protected. Use reflection to
                // get its value anyway.
                var propertyInfo = multiSelector.GetType().GetProperty("CanSelectMultipleItems", BindingFlags.Instance | BindingFlags.NonPublic);
                return propertyInfo != null && (bool)propertyInfo.GetValue(multiSelector, null);
            }

            // Because of listbox is not inherit MultiSelector, detect it's selection model
            if (itemsControl is ListBox listBox)
                return listBox.SelectionMode != SelectionMode.Single;

            return false;
        }

        /// <summary>
        /// TODO: Investigate
        /// </summary>
        /// <param name="itemsControl"></param>
        /// <param name="items"></param>
        /// <param name="position"></param>
        /// <param name="searchDirection"></param>
        /// <returns></returns>
        private static UIElement GetClosest(ItemsControl itemsControl, IEnumerable<DependencyObject> items,
            Point position, Orientation searchDirection)
        {
            UIElement closest = null;
            var closestDistance = double.MaxValue;

            foreach (var i in items)
            {
                if (i is UIElement uiElement)
                {
                    // TODO: Investigate
                    var p = uiElement.TransformToAncestor(itemsControl).Transform(new Point(0, 0));
                    var distance = double.MaxValue;

                    if (itemsControl is TreeView)
                    {
                        var xDiff = position.X - p.X;
                        var yDiff = position.Y - p.Y;
                        var hyp = Math.Sqrt(Math.Pow(xDiff, 2d) + Math.Pow(yDiff, 2d));
                        distance = Math.Abs(hyp);
                    }
                    else
                    {
                        var itemParent = ItemsControl.ItemsControlFromItemContainer(uiElement);
                        if (itemParent != null && itemParent != itemsControl)
                            searchDirection = itemParent.GetItemsPanelOrientation();

                        switch (searchDirection)
                        {
                            case Orientation.Horizontal:
                                distance = position.X <= p.X ? p.X - position.X : position.X - uiElement.RenderSize.Width - p.X;
                                break;
                            case Orientation.Vertical:
                                distance = position.Y <= p.Y ? p.Y - position.Y : position.Y - uiElement.RenderSize.Height - p.Y;
                                break;
                        }
                    }

                    if (distance < closestDistance)
                    {
                        closest = uiElement;
                        closestDistance = distance;
                    }
                }
            }

            return closest;
        }


        /// <summary>
        /// Clears the selected items.
        /// </summary>
        /// <param name="itemsControl">The items control.</param>
        public static void ClearSelectedItems(this ItemsControl itemsControl)
        {
            if (itemsControl is MultiSelector multiSelector)
            {
                if (multiSelector.CanSelectMultipleItems())
                {
                    multiSelector.SelectedItems.Clear();
                }
                multiSelector.SelectedItem = null;
            }
            else if (itemsControl is ListBox listBox)
            {
                if (listBox.CanSelectMultipleItems())
                {
                    listBox.SelectedItems.Clear();
                    listBox.SelectedItem = null;
                }
            }
            else if (itemsControl is TreeViewItem treeViewItem)
            {
                var treeView = ItemsControl.ItemsControlFromItemContainer(treeViewItem);
                treeView?.ClearSelectedItems();
            }
            else if (itemsControl is TreeView treeView)
            {
                // clear old selected item
                var prevSelectedItem = treeView.GetValue(TreeView.SelectedItemProperty);
                if (prevSelectedItem != null)
                {
                    if (treeView.ItemContainerGenerator.ContainerFromItem(prevSelectedItem) is TreeViewItem prevSelectedTreeViewItem)
                    {
                        prevSelectedTreeViewItem.IsSelected = false;
                    }
                }
            }
            else if (itemsControl is Selector selector)
            {
                selector.SelectedItem = null;
            }
        }


        public static void SetItemSelected(this ItemsControl itemsControl, object item, bool itemSelected)
        {
            if (itemsControl is MultiSelector multiSelector)
            {
                if (multiSelector.CanSelectMultipleItems())
                {
                    if (itemSelected)
                    {
                        multiSelector.SelectedItems.Add(item);
                    }
                    else
                    {
                        multiSelector.SelectedItems.Remove(item);
                    }
                }
                else
                {
                    multiSelector.SelectedItem = null;
                    if (itemSelected)
                    {
                        multiSelector.SelectedItem = item;
                    }
                }
            }
            else if (itemsControl is ListBox listBox)
            {
                if (listBox.SelectionMode != SelectionMode.Single)
                {
                    if (itemSelected)
                    {
                        listBox.SelectedItems.Add(item);
                    }
                    else
                    {
                        listBox.SelectedItems.Remove(item);
                    }
                }
                else
                {
                    listBox.SelectedItem = null;
                    if (itemSelected)
                    {
                        listBox.SelectedItem = item;
                    }
                }
            }
            else
            {
                if (itemSelected)
                {
                    itemsControl.SetSelectedItem(item);
                }
                else
                {
                    itemsControl.SetSelectedItem(null);
                }
            }
        }


        /// <summary>
        /// Sets the given object as selected item at the ItemsControl.
        /// </summary>
        /// <param name="itemsControl">The ItemsControl which contains the item.</param>
        /// <param name="item">The object which should be selected.</param>
        public static void SetSelectedItem(this ItemsControl itemsControl, object item)
        {
            if (itemsControl is MultiSelector multiSelector)
            {
                multiSelector.SelectedItem = null;
                multiSelector.SelectedItem = item;
            }
            else if (itemsControl is ListBox listBox)
            {
                var selectionMode = listBox.SelectionMode;
                try
                {
                    // change SelectionMode for UpdateAnchorAndActionItem
                    listBox.SelectionMode = SelectionMode.Single;
                    listBox.SelectedItem = null;
                    listBox.SelectedItem = item;
                }
                finally
                {
                    listBox.SelectionMode = selectionMode;
                }
            }
            else if (itemsControl is TreeViewItem treeViewItem)
            {
                // clear old selected item
                var treeView = ItemsControl.ItemsControlFromItemContainer(treeViewItem);
                var prevSelectedItem = treeView?.GetValue(TreeView.SelectedItemProperty);
                if (prevSelectedItem != null)
                {
                    if (treeView.ItemContainerGenerator.ContainerFromItem(prevSelectedItem) is TreeViewItem prevSelectedTreeViewItem)
                    {
                        prevSelectedTreeViewItem.IsSelected = false;
                    }
                }
                // set new selected item
                // TreeView.SelectedItemProperty is a read only property, so we must set the selection on the TreeViewItem itself
                if (treeViewItem.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem subTreeViewItem)
                {
                    subTreeViewItem.IsSelected = true;
                }
            }
            else if (itemsControl is TreeView treeView)
            {
                // clear old selected item
                var prevSelectedItem = treeView.GetValue(TreeView.SelectedItemProperty);
                if (prevSelectedItem != null)
                {
                    if (treeView.ItemContainerGenerator.ContainerFromItem(prevSelectedItem) is TreeViewItem prevSelectedTreeViewItem)
                    {
                        prevSelectedTreeViewItem.IsSelected = false;
                    }
                }
                // set new selected item
                // TreeView.SelectedItemProperty is a read only property, so we must set the selection on the TreeViewItem itself
                if (treeView.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem subTreeViewItem)
                {
                    subTreeViewItem.IsSelected = true;
                }
            }
            else if (itemsControl is Selector selector)
            {
                selector.SelectedItem = null;
                selector.SelectedItem = item;
            }
        }
    }
}
