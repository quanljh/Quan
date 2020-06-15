using Quan.Word.ViewHelper;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Quan.Word
{

    public class DragInfo : IDragInfo
    {
        #region Constructor

        /// <summary>
        /// Default Constructor
        /// Initializes a new instance of the DragInfo class.
        /// <param name="sender">The sender of the mouse event that initiated the drag.</param>
        /// <param name="e">The mouse event args that initiated the drag.</param>
        /// </summary>
        public DragInfo(object sender, MouseButtonEventArgs e)
        {
            // Set Default properties
            Effects = DragDropEffects.None;
            MouseButton = e.ChangedButton;
            VisualSource = sender as UIElement;
            DragStartPosition = e.GetPosition(VisualSource);
            DragDropCopyKeyState = DragDrop.GetDragDropCopyKeyState(VisualSource);

            // Set data format
            var dataFormat = DragDrop.GetDataFormat(VisualSource);
            if (dataFormat != null)
                DataFormat = dataFormat;

            var sourceElement = e.OriginalSource as UIElement;

            // If we can't cast object as a UIElement it might be a FrameworkContentElement, if so try and use its parent
            if (e.OriginalSource is FrameworkContentElement frameworkContentElement)
                sourceElement = frameworkContentElement.Parent as UIElement;

            // If source control is normal items control...
            if (sender is ItemsControl itemsControl)
            {
                SourceGroup = itemsControl.FindGroup(DragStartPosition);
                VisualSourceFlowDirection = itemsControl.GetItemsPanelFlowDirection();

                UIElement item = null;
                if (sourceElement != null)
                    item = itemsControl.GetItemContainer(sourceElement);

                if (item == null)
                {
                    item = DragDrop.GetDragDirectlySelectedOnly(VisualSource)
                        ? itemsControl.GetItemContainerAt(e.GetPosition(itemsControl))
                        : itemsControl.GetItemContainerAt(e.GetPosition(itemsControl), itemsControl.GetItemsPanelOrientation());
                }

                if (item != null)
                {
                    // Remember the relative position of the item being dragged
                    PositionInDraggedItem = e.GetPosition(item);

                    var itemParent = ItemsControl.ItemsControlFromItemContainer(item);

                    if (itemParent != null)
                    {
                        SourceCollection = itemParent.ItemsSource ?? itemParent.Items;
                        if (itemParent != itemsControl)
                        {
                            if (item is TreeViewItem tvItem)
                            {
                                var tv = tvItem.FindVisualParent<TreeView>();
                                if (tv != null && tv != itemsControl && !tv.IsDragSource())
                                    return;
                            }
                            else if (itemsControl.ItemContainerGenerator.IndexFromContainer(itemParent) < 0 && !itemParent.IsDragSource())
                                return;
                        }

                        SourceIndex = itemParent.ItemContainerGenerator.IndexFromContainer(item);
                        SourceItem = itemParent.ItemContainerGenerator.ItemFromContainer(item);
                    }
                    else
                        SourceIndex = -1;

                    var selectedItems = itemsControl.GetSelectedItems().OfType<object>().Where(i => i != CollectionView.NewItemPlaceholder).ToList();
                    SourceItems = selectedItems;

                    // Some controls (I'm looking at you TreeView!) haven't updated their
                    // SelectedItem by this point. Check to see if there 1 or less item in 
                    // the SourceItems collection, and if so, override the control's SelectedItems with the clicked item.
                    //
                    // The control has still the old selected items at the mouse down event, so we should check this and give only the real selected item to the user.
                    if (selectedItems.Count <= 1 || SourceItems != null && !selectedItems.Contains(SourceItem))
                        SourceItems = Enumerable.Repeat(SourceItem, 1);

                    VisualSourceItem = item;
                }
                else
                {
                    SourceCollection = itemsControl.ItemsSource ?? itemsControl.Items;
                }
            }

            // we clicked a item element
            else
            {
                SourceItem = (sender as FrameworkElement)?.DataContext;
                if (SourceItem != null)
                    SourceItems = Enumerable.Repeat(SourceItems, 1);

                VisualSourceItem = sourceElement;
                PositionInDraggedItem = sourceElement != null ? e.GetPosition(sourceElement) : DragStartPosition;
            }

            if (SourceItems == null)
                SourceItems = Enumerable.Empty<object>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refresh selected items while mouse moving in drag-and-drop actions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void RefreshSelectedItems(object sender, MouseEventArgs e)
        {
            if (sender is ItemsControl itemsControl)
            {
                var selectedItems = itemsControl.GetSelectedItems().OfType<object>().Where(i => i != CollectionView.NewItemPlaceholder).ToList();
                SourceItems = selectedItems;

                // Some controls (I'm looking at you TreeView!) haven't updated their
                // SelectedItem by this point. Check to see if there 1 or less item in 
                // the SourceItems collection, and if so, override the control's SelectedItems with the clicked item.
                //
                // The control has still the old selected items at the mouse down event, so we should check this and give only the real selected item to the user.
                if (selectedItems.Count <= 1 || this.SourceItem != null && !selectedItems.Contains(this.SourceItem))
                {
                    SourceItems = Enumerable.Repeat(this.SourceItem, 1);
                }
            }
        }

        #endregion

        #region Implements

        /// <inheritdoc />
        public DataFormat DataFormat { get; set; }

        /// <inheritdoc />
        public object DataObject { get; set; }

        /// <inheritdoc />
        public object Data { get; set; }

        /// <inheritdoc />
        public Point DragStartPosition { get; }

        /// <inheritdoc />
        public Point PositionInDraggedItem { get; }

        /// <inheritdoc />
        public DragDropEffects Effects { get; set; }

        /// <inheritdoc />
        public MouseButton MouseButton { get; set; }

        /// <inheritdoc />
        public IEnumerable SourceCollection { get; }

        /// <inheritdoc />
        public int SourceIndex { get; }

        /// <inheritdoc />
        public object SourceItem { get; }

        /// <inheritdoc />
        public IEnumerable SourceItems { get; private set; }

        /// <inheritdoc />
        public CollectionViewGroup SourceGroup { get; }

        /// <inheritdoc />
        public UIElement VisualSource { get; }

        /// <inheritdoc />
        public UIElement VisualSourceItem { get; }

        /// <inheritdoc />
        public FlowDirection VisualSourceFlowDirection { get; }

        /// <inheritdoc />
        public Func<DependencyObject, object, DragDropEffects, DragDropEffects> DragDropHandler { get; set; }

        /// <inheritdoc />
        public DragDropKeyStates DragDropCopyKeyState { get; }
        #endregion
    }
}
