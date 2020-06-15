using JetBrains.Annotations;
using Quan.Word.ViewHelper;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using FlowDirection = System.Windows.FlowDirection;

namespace Quan.Word
{
    /// <summary>
    /// Holds information about a the target of a drag drop operation.
    /// </summary>

    /// <remarks>
    /// The <see cref="DropInfo"/> class holds all of the framework's information about the current 
    /// target of a drag. It is used by <see cref="System.Windows.Forms.IDropTarget.DragOver"/> method to determine whether 
    /// the current drop target is valid, and by <see cref="System.Windows.Forms.IDropTarget.Drop"/> to perform the drop.
    /// </remarks>
    public class DropInfo : IDropInfo
    {
        #region Private Members

        private ItemsControl itemParent;
        private UIElement item;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the DropInfo class.
        /// <param name="sender">The sender of the drag event.</param>
        /// <param name="e">The drag event.</param>
        /// <param name="dragInfo">Information about the source of the drag, if the drag came from within the framework.</param>
        /// <param name="eventType">The type of the underlying event (tunneled or bubbled).</param>
        /// </summary>
        public DropInfo(object sender, DragEventArgs e, [CanBeNull] DragInfo dragInfo, EventType eventType)
        {
            DragInfo = dragInfo;
            KeyStates = e.KeyStates;
            EventType = eventType;
            var dataFormat = dragInfo?.DataFormat;
            Data = dataFormat != null && e.Data.GetDataPresent(dataFormat.Name) ? e.Data.GetData(dataFormat.Name) : e.Data;

            VisualTarget = sender as UIElement;
            // if there is no drop target, find another
            if (!VisualTarget.IsDropTarget())
            {
                // try to find next element
                var element = VisualTarget.TryGetNextAncestorDropTargetElement();
                if (element != null)
                {
                    VisualTarget = element;
                }
            }

            // try find ScrollViewer
            var dropTargetScrollViewer = DragDrop.GetDropTargetScrollViewer(VisualTarget);
            if (dropTargetScrollViewer != null)
            {
                TargetScrollViewer = dropTargetScrollViewer;
            }
            else if (VisualTarget is TabControl)
            {
                var tabPanel = VisualTarget.FindVisualChild<TabPanel>();
                TargetScrollViewer = tabPanel?.FindVisualParent<ScrollViewer>();
            }
            else
            {
                TargetScrollViewer = VisualTarget?.FindVisualChild<ScrollViewer>();
            }

            TargetScrollingMode = VisualTarget != null ? DragDrop.GetDropScrollingMode(VisualTarget) : ScrollingMode.Both;

            // visual target can be null, so give us a point...
            DropPosition = VisualTarget != null ? e.GetPosition(VisualTarget) : new Point();

            if (VisualTarget is TabControl)
            {
                if (!HitTestUtilities.HitTest4Type<TabPanel>(VisualTarget, DropPosition))
                {
                    return;
                }
            }

            if (VisualTarget is ItemsControl itemsControl)
            {
                //System.Diagnostics.Debug.WriteLine(">>> Name = {0}", itemsControl.Name);
                // get item under the mouse
                item = itemsControl.GetItemContainerAt(DropPosition);
                var directlyOverItem = item != null;

                TargetGroup = itemsControl.FindGroup(DropPosition);
                VisualTargetOrientation = itemsControl.GetItemsPanelOrientation();
                VisualTargetFlowDirection = itemsControl.GetItemsPanelFlowDirection();

                if (item == null)
                {
                    // ok, no item found, so maybe we can found an item at top, left, right or bottom
                    item = itemsControl.GetItemContainerAt(DropPosition, VisualTargetOrientation);
                    directlyOverItem = DropPosition.DirectlyOverElement(item, itemsControl);
                }

                if (item == null && TargetGroup != null && TargetGroup.IsBottomLevel)
                {
                    var itemData = TargetGroup.Items.FirstOrDefault();
                    if (itemData != null)
                    {
                        item = itemsControl.ItemContainerGenerator.ContainerFromItem(itemData) as UIElement;
                        directlyOverItem = DropPosition.DirectlyOverElement(item, itemsControl);
                    }
                }

                if (item != null)
                {
                    itemParent = ItemsControl.ItemsControlFromItemContainer(item);
                    VisualTargetOrientation = itemParent.GetItemsPanelOrientation();
                    VisualTargetFlowDirection = itemParent.GetItemsPanelFlowDirection();

                    InsertIndex = itemParent.ItemContainerGenerator.IndexFromContainer(item);
                    TargetCollection = itemParent.ItemsSource ?? itemParent.Items;

                    var tvItem = item as TreeViewItem;

                    if (directlyOverItem || tvItem != null)
                    {
                        VisualTargetItem = item;
                        TargetItem = itemParent.ItemContainerGenerator.ItemFromContainer(item);
                    }

                    var expandedTVItem = tvItem != null && tvItem.HasHeader && tvItem.HasItems && tvItem.IsExpanded;
                    var itemRenderSize = expandedTVItem ? tvItem.GetHeaderSize() : item.RenderSize;

                    if (VisualTargetOrientation == Orientation.Vertical)
                    {
                        var currentYPos = e.GetPosition(item).Y;
                        var targetHeight = itemRenderSize.Height;

                        var topGap = targetHeight * 0.25;
                        var bottomGap = targetHeight * 0.75;
                        if (currentYPos > targetHeight / 2)
                        {
                            if (expandedTVItem && (currentYPos < topGap || currentYPos > bottomGap))
                            {
                                VisualTargetItem = tvItem.ItemContainerGenerator.ContainerFromIndex(0) as UIElement;
                                TargetItem = VisualTargetItem != null ? tvItem.ItemContainerGenerator.ItemFromContainer(VisualTargetItem) : null;
                                TargetCollection = tvItem.ItemsSource ?? tvItem.Items;
                                InsertIndex = 0;
                                InsertPosition = RelativeInsertPosition.BeforeTargetItem;
                            }
                            else
                            {
                                InsertIndex++;
                                InsertPosition = RelativeInsertPosition.AfterTargetItem;
                            }
                        }
                        else
                        {
                            InsertPosition = RelativeInsertPosition.BeforeTargetItem;
                        }

                        if (currentYPos > topGap && currentYPos < bottomGap)
                        {
                            if (tvItem != null)
                            {
                                TargetCollection = tvItem.ItemsSource ?? tvItem.Items;
                                InsertIndex = TargetCollection != null ? TargetCollection.OfType<object>().Count() : 0;
                            }
                            InsertPosition |= RelativeInsertPosition.TargetItemCenter;
                        }
                        //System.Diagnostics.Debug.WriteLine("==> DropInfo: pos={0}, idx={1}, Y={2}, Item={3}", InsertPosition, InsertIndex, currentYPos, item);
                    }
                    else
                    {
                        var currentXPos = e.GetPosition(item).X;
                        var targetWidth = itemRenderSize.Width;

                        if (VisualTargetFlowDirection == FlowDirection.RightToLeft)
                        {
                            if (currentXPos > targetWidth / 2)
                            {
                                InsertPosition = RelativeInsertPosition.BeforeTargetItem;
                            }
                            else
                            {
                                InsertIndex++;
                                InsertPosition = RelativeInsertPosition.AfterTargetItem;
                            }
                        }
                        else if (VisualTargetFlowDirection == FlowDirection.LeftToRight)
                        {
                            if (currentXPos > targetWidth / 2)
                            {
                                InsertIndex++;
                                InsertPosition = RelativeInsertPosition.AfterTargetItem;
                            }
                            else
                            {
                                InsertPosition = RelativeInsertPosition.BeforeTargetItem;
                            }
                        }

                        if (currentXPos > targetWidth * 0.25 && currentXPos < targetWidth * 0.75)
                        {
                            if (tvItem != null)
                            {
                                TargetCollection = tvItem.ItemsSource ?? tvItem.Items;
                                InsertIndex = TargetCollection != null ? TargetCollection.OfType<object>().Count() : 0;
                            }
                            InsertPosition |= RelativeInsertPosition.TargetItemCenter;
                        }
                        //System.Diagnostics.Debug.WriteLine("==> DropInfo: pos={0}, idx={1}, X={2}, Item={3}", InsertPosition, InsertIndex, currentXPos, item);
                    }
                }
                else
                {
                    TargetCollection = itemsControl.ItemsSource ?? itemsControl.Items;
                    InsertIndex = itemsControl.Items.Count;
                    //System.Diagnostics.Debug.WriteLine("==> DropInfo: pos={0}, item=NULL, idx={1}", InsertPosition, InsertIndex);
                }
            }
            else
            {
                VisualTargetItem = VisualTarget;
            }
        }

        #endregion

        #region Implements

        /// <inheritdoc />
        public object Data { get; }

        /// <inheritdoc />
        public IDragInfo DragInfo { get; }

        /// <inheritdoc />
        public Point DropPosition { get; }

        /// <inheritdoc />
        public Type DropTargetAdorner { get; set; }

        /// <inheritdoc />
        public DragDropEffects Effects { get; set; }

        /// <inheritdoc />
        public int InsertIndex { get; }

        /// <inheritdoc />
        public int UnfilteredInsertIndex
        {
            get
            {
                var insertIndex = InsertIndex;
                var itemSourceAsList = itemParent?.ItemsSource.TryGetList();
                if (itemSourceAsList != null && itemParent.Items.Count != itemSourceAsList.Count)
                {
                    if (insertIndex >= 0 && insertIndex < itemParent.Items.Count)
                    {
                        var indexOf = itemSourceAsList.IndexOf(itemParent.Items[insertIndex]);
                        if (indexOf >= 0)
                        {
                            return indexOf;
                        }
                    }
                    else if (itemParent.Items.Count > 0 && insertIndex == itemParent.Items.Count)
                    {
                        var indexOf = itemSourceAsList.IndexOf(itemParent.Items[insertIndex - 1]);
                        if (indexOf >= 0)
                        {
                            return indexOf + 1;
                        }
                    }
                }
                return insertIndex;
            }
        }

        /// <inheritdoc />
        public IEnumerable TargetCollection { get; }

        /// <inheritdoc />
        public object TargetItem { get; }

        /// <inheritdoc />
        public CollectionViewGroup TargetGroup { get; }

        /// <summary>
        /// Gets the ScrollViewer control for the visual target.
        /// </summary>
        public ScrollViewer TargetScrollViewer { get; }

        /// <summary>
        /// Gets or Sets the ScrollingMode for the drop action.
        /// </summary>
        public ScrollingMode TargetScrollingMode { get; set; }

        /// <inheritdoc />
        public UIElement VisualTarget { get; }

        /// <inheritdoc />
        public UIElement VisualTargetItem { get; }

        /// <inheritdoc />
        public Orientation VisualTargetOrientation { get; }

        /// <inheritdoc />
        public FlowDirection VisualTargetFlowDirection { get; }

        /// <inheritdoc />
        public string DestinationText { get; set; }

        /// <inheritdoc />
        public string EffectText { get; set; }

        /// <inheritdoc />
        public RelativeInsertPosition InsertPosition { get; }

        /// <inheritdoc />
        public DragDropKeyStates KeyStates { get; }

        /// <inheritdoc />
        public bool NotHandled { get; set; }

        /// <inheritdoc />
        public bool IsSameDragDropContextAsSource
        {
            get
            {
                // Check if DragInfo stuff exists
                if (DragInfo == null || DragInfo.VisualSource == null)
                {
                    return true;
                }
                // A target should be exists
                if (VisualTarget == null)
                {
                    return true;
                }

                // Source element has a drag context constraint, we need to check the target property matches.
                var sourceContext = DragInfo.VisualSource.GetValue(DragDrop.DragDropContextProperty) as string;
                if (string.IsNullOrEmpty(sourceContext))
                {
                    return true;
                }
                var targetContext = VisualTarget.GetValue(DragDrop.DragDropContextProperty) as string;
                return string.Equals(sourceContext, targetContext);
            }
        }

        /// <inheritdoc />
        public EventType EventType { get; }
        #endregion
    }
}
