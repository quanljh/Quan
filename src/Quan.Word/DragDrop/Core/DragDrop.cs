using Quan.Word.ViewHelper;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Quan.Word
{

    public partial class DragDrop
    {
        #region Private Members

        private static DragAdorner _DragAdorner;

        private static DragAdorner DragAdorner
        {
            get { return _DragAdorner; }
            set
            {
                _DragAdorner?.Detatch();
                _DragAdorner = value;
            }
        }

        private static DragAdorner _EffectAdorner;

        private static DragAdorner EffectAdorner
        {
            get { return _EffectAdorner; }
            set
            {
                _EffectAdorner?.Detatch();
                _EffectAdorner = value;
            }
        }

        private static DropTargetAdorner _DropTargetAdorner;

        private static DropTargetAdorner DropTargetAdorner
        {
            get { return _DropTargetAdorner; }
            set
            {
                _DropTargetAdorner?.Detatch();
                _DropTargetAdorner = value;
            }
        }

        private static DragInfo m_DragInfo;
        private static bool m_DragInProgress;

        // The current clicked item
        private static object m_ClickSupressItem;

        private static Point _adornerMousePosition;
        private static Size _adornerSize;

        private static Point _effectAdornerMousePosition;
        private static Size _effectAdornerSize;

        #endregion

        #region Drag Source Events

        /// <summary>
        /// Raised on the very first time of drag-and-drop actions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DragSourceOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DoMouseButtonDown(sender, e);
        }

        /// <summary>
        /// Raised on drag-and-drop actions ended
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DragSourceOnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DoMouseButtonUp(sender, e);
        }

        /// <summary>
        /// Raised if we specific mouse right button to do drag actions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DragSourceOnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            DoMouseButtonUp(sender, e);
        }

        /// <summary>
        /// Raise on mouse move at source ItemsControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DragSourceOnMouseMove(object sender, MouseEventArgs e)
        {
            var dragInfo = m_DragInfo;
            // if we not release the mouse button while we moving our mouse...
            if (dragInfo != null && !m_DragInProgress)
            {
                // the start from the source
                var dragStart = dragInfo.DragStartPosition;

                // do nothing if mouse left/right button is released or the pointer is captured
                if (dragInfo.MouseButton == MouseButton.Left && e.LeftButton == MouseButtonState.Released)
                {
                    m_DragInfo = null;
                    return;
                }
                if (GetCanDragWithMouseRightButton(dragInfo.VisualSource) && dragInfo.MouseButton == MouseButton.Right && e.RightButton == MouseButtonState.Released)
                {
                    m_DragInfo = null;
                    return;
                }

                // current mouse position
                var position = e.GetPosition((IInputElement)sender);

                // prevent selection changing while drag operation
                dragInfo.VisualSource?.ReleaseMouseCapture();

                // only if the sender is the source control and the mouse point differs from an offset
                if (dragInfo.VisualSource == sender
                    && (Math.Abs(position.X - dragStart.X) > GetMinimumHorizontalDragDistance(dragInfo.VisualSource) ||
                        Math.Abs(position.Y - dragStart.Y) > GetMinimumVerticalDragDistance(dragInfo.VisualSource)))
                {
                    // Ensure SourceItems is latest selected items
                    dragInfo.RefreshSelectedItems(sender, e);

                    var dragHandler = TryGetDragHandler(dragInfo, sender as UIElement);
                    if (dragHandler.CanStartDrag(dragInfo))
                    {
                        dragHandler.StartDrag(dragInfo);

                        if (dragInfo.Effects != DragDropEffects.None)
                        {
                            var dataObject = dragInfo.DataObject;

                            if (dataObject == null)
                            {
                                if (dragInfo.Data == null)
                                {
                                    // it's bad if the Data is null, cause the DataObject constructor will raise an ArgumentNullException
                                    m_DragInfo = null; // maybe not necessary or should not set here to null
                                    return;
                                }
                                dataObject = new DataObject(dragInfo.DataFormat.Name, dragInfo.Data);
                            }

                            try
                            {
                                m_DragInProgress = true;
                                var dragDropHandler = dragInfo.DragDropHandler ?? System.Windows.DragDrop.DoDragDrop;
                                var dragDropEffects = dragDropHandler(dragInfo.VisualSource, dataObject, dragInfo.Effects);
                                if (dragDropEffects == DragDropEffects.None)
                                {
                                    dragHandler.DragCancelled();
                                }
                                dragHandler.DragDropOperationFinished(dragDropEffects, dragInfo);
                            }
                            catch (Exception ex)
                            {
                                if (!dragHandler.TryCatchOccurredException(ex))
                                {
                                    throw;
                                }
                            }
                            finally
                            {
                                m_DragInProgress = false;
                                m_DragInfo = null;
                            }
                        }
                    }
                }
            }
        }

        private static void DragSourceOnQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (e.Action == DragAction.Cancel || e.EscapePressed)
            {
                DragAdorner = null;
                EffectAdorner = null;
                DropTargetAdorner = null;
                Mouse.OverrideCursor = null;
            }
        }

        private static void DragSourceOnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            DoMouseButtonDown(sender, e);
        }

        /// <summary>
        /// Set base drag info when mouse left button down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DoMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_DragInfo = null;

            // Gets the position of the mouse relative to source control
            var elementPosition = e.GetPosition((IInputElement)sender);

            // Ignore the click if clickCount != 1 or the user has clicked on a scrollbar or the control while editing.
            if (e.ClickCount != 1
                || (sender as UIElement).IsDragSourceIgnored()
                || (e.Source as UIElement).IsDragSourceIgnored()
                || (e.OriginalSource as UIElement).IsDragSourceIgnored()
                // Ignore TabControl's content presenter
                || (sender is TabControl) && !HitTestUtilities.HitTest4Type<TabPanel>(sender, elementPosition)
                // Ignore scroll bar
                || HitTestUtilities.HitTest4Type<RangeBase>(sender, elementPosition)
                // Ignore the control while editing.
                || HitTestUtilities.HitTest4Type<TextBoxBase>(sender, elementPosition)
                || HitTestUtilities.HitTest4Type<PasswordBox>(sender, elementPosition)
                || HitTestUtilities.HitTest4Type<ComboBox>(sender, elementPosition)
                // Ignore grid column header
                || HitTestUtilities.HitTest4GridViewColumnHeader(sender, elementPosition)
                // Ignore user reordering DataGrid columns and drag row header gripper to chang row header height
                || HitTestUtilities.HitTest4DataGridTypes(sender, elementPosition)
                || HitTestUtilities.IsNotPartOfSender(sender, e))
            {
                return;
            }

            var dragInfo = new DragInfo(sender, e);

            if (dragInfo.VisualSource is ItemsControl control && control.CanSelectMultipleItems())
            {
                control.Focus();
            }

            if (dragInfo.VisualSourceItem == null)
            {
                return;
            }

            var dragHandler = TryGetDragHandler(dragInfo, sender as UIElement);
            if (!dragHandler.CanStartDrag(dragInfo))
            {
                return;
            }

            // If the sender is a list box that allows multiple selections, ensure that clicking on an 
            // already selected item does not change the selection(didn't click a shift key or control key at the same time),
            // otherwise dragging multiple items is made impossible.
            if ((Keyboard.Modifiers & ModifierKeys.Shift) == 0 && (Keyboard.Modifiers & ModifierKeys.Control) == 0 && dragInfo.VisualSourceItem != null && sender is ItemsControl itemsControl && itemsControl.CanSelectMultipleItems())
            {
                var selectedItems = itemsControl.GetSelectedItems().OfType<object>().ToList();

                // we haven't pressed control key and shift key, set clicked current item
                if (selectedItems.Count > 1 && selectedItems.Contains(dragInfo.SourceItem))
                {
                    m_ClickSupressItem = dragInfo.SourceItem;
                    e.Handled = true;
                }
            }

            m_DragInfo = dragInfo;
        }

        /// <summary>
        /// Clear local variable and set selected items wh when mouse button up 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DoMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            var elementPosition = e.GetPosition((IInputElement)sender);
            if ((sender is TabControl) && !HitTestUtilities.HitTest4Type<TabPanel>(sender, elementPosition))
            {
                m_DragInfo = null;
                m_ClickSupressItem = null;
                return;
            }

            var dragInfo = m_DragInfo;

            // If we prevented the control's default selection handling in DragSource_PreviewMouseLeftButtonDown
            // by setting 'e.Handled = true' and a drag was not initiated, manually set the selection here.
            if (sender is ItemsControl itemsControl && dragInfo != null && m_ClickSupressItem != null && m_ClickSupressItem == dragInfo.SourceItem)
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
                {
                    itemsControl.SetItemSelected(dragInfo.SourceItem, false);
                }
                else if ((Keyboard.Modifiers & ModifierKeys.Shift) == 0)
                {
                    itemsControl.SetSelectedItem(dragInfo.SourceItem);
                }
            }

            m_DragInfo = null;
            m_ClickSupressItem = null;
        }


        private static void Scroll(DropInfo dropInfo, DragEventArgs e)
        {
            if (dropInfo == null || dropInfo.TargetScrollViewer == null)
            {
                return;
            }

            var scrollViewer = dropInfo.TargetScrollViewer;
            var scrollingMode = dropInfo.TargetScrollingMode;

            var position = e.GetPosition(scrollViewer);
            var scrollMargin = Math.Min(scrollViewer.FontSize * 2, scrollViewer.ActualHeight / 2);

            if (scrollingMode == ScrollingMode.Both || scrollingMode == ScrollingMode.HorizontalOnly)
            {
                if (position.X >= scrollViewer.ActualWidth - scrollMargin && scrollViewer.HorizontalOffset < scrollViewer.ExtentWidth - scrollViewer.ViewportWidth)
                {
                    scrollViewer.LineRight();
                }
                else if (position.X < scrollMargin && scrollViewer.HorizontalOffset > 0)
                {
                    scrollViewer.LineLeft();
                }
            }

            if (scrollingMode == ScrollingMode.Both || scrollingMode == ScrollingMode.VerticalOnly)
            {
                if (position.Y >= scrollViewer.ActualHeight - scrollMargin && scrollViewer.VerticalOffset < scrollViewer.ExtentHeight - scrollViewer.ViewportHeight)
                {
                    scrollViewer.LineDown();
                }
                else if (position.Y < scrollMargin && scrollViewer.VerticalOffset > 0)
                {
                    scrollViewer.LineUp();
                }
            }
        }

        #endregion

        #region Drop Target Events

        /// <summary>
        /// Raised while dragging item enter drop target control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DropTargetOnDragEnter(object sender, DragEventArgs e)
        {
            DropTargetOnDragOver(sender, e, EventType.Bubbled);
        }

        /// <summary>
        /// Raise when dragging item leave drop target control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DropTargetOnDragLeave(object sender, DragEventArgs e)
        {
            DragAdorner = null;
            EffectAdorner = null;
            DropTargetAdorner = null;
        }

        /// <summary>
        /// Raise when hovers over drop target control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DropTargetOnDragOver(object sender, DragEventArgs e)
        {
            DropTargetOnDragOver(sender, e, EventType.Bubbled);
        }


        /// <summary>
        /// Raise when drop item to drop target control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DropTargetOnDrop(object sender, DragEventArgs e)
        {
            DropTargetOnDrop(sender, e, EventType.Bubbled);
        }

        /// <summary>
        /// Give visual feedback during the drag-and-drop operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DropTargetOnGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (EffectAdorner != null)
            {
                e.UseDefaultCursors = false;
                e.Handled = true;
                if (Mouse.OverrideCursor != Cursors.Arrow)
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
            else
            {
                e.UseDefaultCursors = true;
                e.Handled = true;
                if (Mouse.OverrideCursor != null)
                {
                    Mouse.OverrideCursor = null;
                }
            }
        }


        private static void DropTargetOnPreviewDragEnter(object sender, DragEventArgs e)
        {
            DropTargetOnDragOver(sender, e, EventType.Tunneled);
        }

        private static void DropTargetOnPreviewDragOver(object sender, DragEventArgs e)
        {
            DropTargetOnDragOver(sender, e, EventType.Tunneled);
        }

        private static void DropTargetOnPreviewDrop(object sender, DragEventArgs e)
        {
            DropTargetOnDrop(sender, e, EventType.Tunneled);
        }

        private static void DropTargetOnDragOver(object sender, DragEventArgs e, EventType eventType)
        {
            var elementPosition = e.GetPosition((IInputElement)sender);

            var dragInfo = m_DragInfo;
            var dropInfo = new DropInfo(sender, e, dragInfo, eventType);
            var dropHandler = TryGetDropHandler(dropInfo, sender as UIElement);
            var itemsControl = dropInfo.VisualTarget;

            dropHandler.DragOver(dropInfo);

            if (DragAdorner == null && dragInfo != null)
            {
                CreateDragAdorner(dropInfo);
            }

            DragAdorner?.Move(e.GetPosition(DragAdorner.AdornedElement), dragInfo != null ? GetDragMouseAnchorPoint(dragInfo.VisualSource) : default(Point), ref _adornerMousePosition, ref _adornerSize);

            Scroll(dropInfo, e);

            if (HitTestUtilities.HitTest4Type<ScrollBar>(sender, elementPosition)
                || HitTestUtilities.HitTest4GridViewColumnHeader(sender, elementPosition)
                || HitTestUtilities.HitTest4DataGridTypesOnDragOver(sender, elementPosition))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }

            // If the target is an ItemsControl then update the drop target adorner.
            if (itemsControl != null)
            {
                // Display the adorner in the control's ItemsPresenter. If there is no 
                // ItemsPresenter provided by the style, try getting hold of a
                // ScrollContentPresenter and using that.
                UIElement adornedElement = null;
                if (itemsControl is TabControl)
                {
                    adornedElement = itemsControl.FindVisualParent<TabPanel>();
                }
                else if (itemsControl is DataGrid || (itemsControl as ListView)?.View is GridView)
                {
                    adornedElement = itemsControl.FindVisualParent<ScrollContentPresenter>() as UIElement ?? itemsControl.FindVisualParent<ItemsPresenter>() as UIElement ?? itemsControl;
                }
                else
                {
                    adornedElement = itemsControl.FindVisualParent<ItemsPresenter>() as UIElement ?? itemsControl.FindVisualParent<ScrollContentPresenter>() as UIElement ?? itemsControl;
                }

                if (adornedElement != null)
                {
                    if (dropInfo.DropTargetAdorner == null)
                    {
                        DropTargetAdorner = null;
                    }
                    else if (!dropInfo.DropTargetAdorner.IsInstanceOfType(DropTargetAdorner))
                    {
                        DropTargetAdorner = DropTargetAdorner.Create(dropInfo.DropTargetAdorner, adornedElement, dropInfo);
                    }

                    var adorner = DropTargetAdorner;
                    if (adorner != null)
                    {
                        var adornerBrush = GetDropTargetAdornerBrush(dropInfo.VisualTarget);
                        if (adornerBrush != null)
                        {
                            adorner.Pen.Brush = adornerBrush;
                        }
                        adorner.DropInfo = dropInfo;
                        adorner.InvalidateVisual();
                    }
                }
            }

            // Set the drag effect adorner if there is one
            if (dragInfo != null && (EffectAdorner == null || EffectAdorner.Effects != dropInfo.Effects))
            {
                CreateEffectAdorner(dropInfo);
            }

            EffectAdorner?.Move(e.GetPosition(EffectAdorner.AdornedElement), default(Point), ref _effectAdornerMousePosition, ref _effectAdornerSize);

            e.Effects = dropInfo.Effects;
            e.Handled = !dropInfo.NotHandled;

            if (!dropInfo.IsSameDragDropContextAsSource)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private static void DropTargetOnDrop(object sender, DragEventArgs e, EventType eventType)
        {
            var dragInfo = m_DragInfo;
            var dropInfo = new DropInfo(sender, e, dragInfo, eventType);
            var dropHandler = TryGetDropHandler(dropInfo, sender as UIElement);
            var dragHandler = TryGetDragHandler(dragInfo, sender as UIElement);

            DragAdorner = null;
            EffectAdorner = null;
            DropTargetAdorner = null;

            dropHandler.DragOver(dropInfo);
            dropHandler.Drop(dropInfo);
            dragHandler.Dropped(dropInfo);

            e.Effects = dropInfo.Effects;
            e.Handled = !dropInfo.NotHandled;

            Mouse.OverrideCursor = null;
        }

        #endregion

        #region Drag Handler

        /// <summary>
        /// Gets the drag handler from the drag info or from the sender, if the drag info is null
        /// </summary>
        /// <param name="dragInfo">the drag info object</param>
        /// <param name="sender">the sender from an event, e.g. mouse down, mouse move</param>
        /// <returns></returns>
        private static IDragSource TryGetDragHandler(DragInfo dragInfo, UIElement sender)
        {
            IDragSource dragHandler = null;

            if (dragInfo?.VisualSource != null)
                dragHandler = GetDragHandler(dragInfo.VisualSource);

            if (dragHandler == null && sender != null)
                dragHandler = GetDragHandler(sender);

            return dragHandler ?? DefaultDragHandler;
        }

        /// <summary>
        /// Gets the drop handler from the drop info or from the sender, if the drop info is null
        /// </summary>
        /// <param name="dropInfo">the drop info object</param>
        /// <param name="sender">the sender from an event, e.g. drag over</param>
        /// <returns></returns>
        private static IDropTarget TryGetDropHandler(DropInfo dropInfo, UIElement sender)
        {
            IDropTarget dropHandler = null;
            if (dropInfo != null && dropInfo.VisualTarget != null)
            {
                dropHandler = GetDropHandler(dropInfo.VisualTarget);
            }
            if (dropHandler == null && sender != null)
            {
                dropHandler = GetDropHandler(sender);
            }
            return dropHandler ?? DefaultDropHandler;
        }

        #endregion

        #region Adorner

        private static void CreateDragAdorner(DropInfo dropInfo)
        {
            var dragInfo = dropInfo.DragInfo;
            var template = GetDropAdornerTemplate(dropInfo.VisualTarget) ?? GetDragAdornerTemplate(dragInfo.VisualSource);
            var templateSelector = GetDropAdornerTemplateSelector(dropInfo.VisualTarget) ?? GetDragAdornerTemplateSelector(dragInfo.VisualSource);

            UIElement adornment = null;

            var useDefaultDragAdorner = template == null && templateSelector == null && GetUseDefaultDragAdorner(dragInfo.VisualSource);
            var useVisualSourceItemSizeForDragAdorner = GetUseVisualSourceItemSizeForDragAdorner(dragInfo.VisualSource);

            if (useDefaultDragAdorner)
            {
                // Get default data template by capture visual source item to image and make template
                template = dragInfo.VisualSourceItem.GetCaptureScreenDataTemplate(dragInfo.VisualSourceFlowDirection);
            }

            if (template != null || templateSelector != null)
            {
                if (dragInfo.Data is IEnumerable data && !(data is string))
                {
                    var dataList = data as object[] ?? data.Cast<object>().ToArray();
                    if (!useDefaultDragAdorner && dataList.Count() <= 10)
                    {
                        var itemsControl = new ItemsControl
                        {
                            ItemsSource = dataList,
                            ItemTemplate = template,
                            ItemTemplateSelector = templateSelector,
                            Tag = dragInfo
                        };

                        if (useVisualSourceItemSizeForDragAdorner)
                        {
                            var bounds = VisualTreeHelper.GetDescendantBounds(dragInfo.VisualSourceItem);
                            itemsControl.SetValue(FrameworkElement.MinWidthProperty, bounds.Width);
                        }

                        // The ItemsControl doesn't display unless we create a grid to contain it.
                        // Not quite sure why we need this...
                        var grid = new Grid();
                        grid.Children.Add(itemsControl);
                        adornment = grid;
                    }
                }
                else
                {
                    var contentPresenter = new ContentPresenter
                    {
                        Content = dragInfo.Data,
                        ContentTemplate = template,
                        ContentTemplateSelector = templateSelector,
                        Tag = dragInfo
                    };

                    if (useVisualSourceItemSizeForDragAdorner)
                    {
                        var bounds = VisualTreeHelper.GetDescendantBounds(dragInfo.VisualSourceItem);
                        contentPresenter.SetValue(FrameworkElement.MinWidthProperty, bounds.Width);
                        contentPresenter.SetValue(FrameworkElement.MinHeightProperty, bounds.Height);
                    }

                    adornment = contentPresenter;
                }
            }

            if (adornment != null)
            {
                if (useDefaultDragAdorner)
                {
                    adornment.Opacity = GetDefaultDragAdornerOpacity(dragInfo.VisualSource);
                }

                // Get root element to make sure adorner is always on top of the root element
                var rootElement = RootElementFinder.FindRoot(dropInfo.VisualTarget ?? dragInfo.VisualSource);
                DragAdorner = new DragAdorner(rootElement, adornment, GetDragAdornerTranslation(dragInfo.VisualSource));
            }
        }

        private static void CreateEffectAdorner(DropInfo dropInfo)
        {
            var dragInfo = m_DragInfo;
            var template = GetEffectAdornerTemplate(dragInfo.VisualSource, dropInfo.Effects, dropInfo.DestinationText, dropInfo.EffectText);

            if (template != null)
            {
                var rootElement = RootElementFinder.FindRoot(dropInfo.VisualTarget ?? dragInfo.VisualSource);

                var adornment = new ContentPresenter();
                adornment.Content = dragInfo.Data;
                adornment.ContentTemplate = template;

                EffectAdorner = new DragAdorner(rootElement, adornment, GetEffectAdornerTranslation(dragInfo.VisualSource), dropInfo.Effects);
            }
        }

        private static DataTemplate GetEffectAdornerTemplate(UIElement target, DragDropEffects effect, string destinationText, string effectText = null)
        {
            switch (effect)
            {
                case DragDropEffects.All:
                    // TODO: Add default template for EffectAll
                    return GetEffectAllAdornerTemplate(target);
                case DragDropEffects.Copy:
                    return GetEffectCopyAdornerTemplate(target) ?? CreateDefaultEffectDataTemplate(target, IconFactory.EffectCopy, effectText == null ? "Copy to" : effectText, destinationText);
                case DragDropEffects.Link:
                    return GetEffectLinkAdornerTemplate(target) ?? CreateDefaultEffectDataTemplate(target, IconFactory.EffectLink, effectText == null ? "Link to" : effectText, destinationText);
                case DragDropEffects.Move:
                    return GetEffectMoveAdornerTemplate(target) ?? CreateDefaultEffectDataTemplate(target, IconFactory.EffectMove, effectText == null ? "Move to" : effectText, destinationText);
                case DragDropEffects.None:
                    return GetEffectNoneAdornerTemplate(target) ?? CreateDefaultEffectDataTemplate(target, IconFactory.EffectNone, effectText == null ? "None" : effectText, destinationText);
                case DragDropEffects.Scroll:
                    // TODO: Add default template EffectScroll
                    return GetEffectScrollAdornerTemplate(target);
                default:
                    return null;
            }
        }

        private static DataTemplate CreateDefaultEffectDataTemplate(UIElement target, BitmapImage effectIcon, string effectText, string destinationText)
        {
            if (!GetUseDefaultEffectDataTemplate(target))
            {
                return null;
            }

            var fontSize = SystemFonts.MessageFontSize; // before 11d

            // The icon
            var imageFactory = new FrameworkElementFactory(typeof(Image));
            imageFactory.SetValue(Image.SourceProperty, effectIcon);
            imageFactory.SetValue(FrameworkElement.HeightProperty, 12d);
            imageFactory.SetValue(FrameworkElement.WidthProperty, 12d);

            // Only the icon for no effect
            if (Equals(effectIcon, IconFactory.EffectNone))
            {
                return new DataTemplate { VisualTree = imageFactory };
            }

            // Some margin for the icon
            imageFactory.SetValue(FrameworkElement.MarginProperty, new Thickness(0, 0, 3, 0));
            imageFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);

            // Add effect text
            var effectTextBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            effectTextBlockFactory.SetValue(TextBlock.TextProperty, effectText);
            effectTextBlockFactory.SetValue(TextBlock.FontSizeProperty, fontSize);
            effectTextBlockFactory.SetValue(TextBlock.ForegroundProperty, Brushes.Blue);
            effectTextBlockFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);

            // Add destination text
            var destinationTextBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            destinationTextBlockFactory.SetValue(TextBlock.TextProperty, destinationText);
            destinationTextBlockFactory.SetValue(TextBlock.FontSizeProperty, fontSize);
            destinationTextBlockFactory.SetValue(TextBlock.ForegroundProperty, Brushes.DarkBlue);
            destinationTextBlockFactory.SetValue(FrameworkElement.MarginProperty, new Thickness(3, 0, 0, 0));
            destinationTextBlockFactory.SetValue(TextBlock.FontWeightProperty, FontWeights.DemiBold);
            destinationTextBlockFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);

            // Create containing panel
            var stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            stackPanelFactory.SetValue(FrameworkElement.MarginProperty, new Thickness(2));
            stackPanelFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            stackPanelFactory.AppendChild(imageFactory);
            stackPanelFactory.AppendChild(effectTextBlockFactory);
            stackPanelFactory.AppendChild(destinationTextBlockFactory);

            // Add border
            var borderFactory = new FrameworkElementFactory(typeof(Border));
            var stopCollection = new GradientStopCollection
                                 {
                                     new GradientStop(Colors.White, 0.0),
                                     new GradientStop(Colors.AliceBlue, 1.0)
                                 };
            var gradientBrush = new LinearGradientBrush(stopCollection)
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)
            };
            borderFactory.SetValue(Panel.BackgroundProperty, gradientBrush);
            borderFactory.SetValue(Border.BorderBrushProperty, Brushes.DimGray);
            borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(3));
            borderFactory.SetValue(Border.BorderThicknessProperty, new Thickness(1));
            borderFactory.SetValue(UIElement.SnapsToDevicePixelsProperty, true);
            borderFactory.SetValue(TextOptions.TextFormattingModeProperty, TextFormattingMode.Display);
            borderFactory.SetValue(TextOptions.TextRenderingModeProperty, TextRenderingMode.ClearType);
            borderFactory.SetValue(TextOptions.TextHintingModeProperty, TextHintingMode.Fixed);
            borderFactory.AppendChild(stackPanelFactory);

            // Finally add content to template
            return new DataTemplate { VisualTree = borderFactory };
        }

        #endregion

    }
}
