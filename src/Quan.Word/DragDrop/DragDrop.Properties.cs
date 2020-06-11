using Quan.Word.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Quan.Word
{
    public partial class DragDrop
    {
        #region Attached Properties

        /// <summary>
        /// Gets or Sets whether the control can be used as drop source
        /// </summary>
        public static readonly DependencyProperty IsDragSourceProperty =
            DependencyProperty.RegisterAttached(
                "IsDragSource",
                typeof(bool),
                typeof(DragDrop),
                new UIPropertyMetadata(false, IsDropSourcePropertyChanged));

        /// <summary>
        /// Gets or Sets whether the control can be used as drop target
        /// </summary>
        public static readonly DependencyProperty IsDropTargetProperty =
            DependencyProperty.RegisterAttached(
                "IsDropTarget",
                typeof(bool),
                typeof(DragDrop),
                new UIPropertyMetadata(false, IsDropTargetPropertyChanged));

        /// <summary>
        /// Gets or Sets the events which are subscribed for the drag and drop events
        /// </summary>
        public static readonly DependencyProperty DropEventTypeProperty =
            DependencyProperty.RegisterAttached(
                "DropEventType",
                typeof(EventType),
                typeof(DragDrop),
                new PropertyMetadata(EventType.Auto, DropEventTypeChanged));

        #region Attached Properties accessors

        /// <summary>
        /// Gets whether the control can be used as drop source
        /// </summary>
        /// <param name="soucre">The soucre control</param>
        /// <returns></returns>
        public static bool GetIsDropSource(UIElement soucre)
        {
            return (bool)soucre.GetValue(IsDropTargetProperty);
        }

        /// <summary>
        /// Sets whether the control can be used as drop source
        /// </summary>
        /// <param name="soucre">The soucre control</param>
        /// <param name="value">The new value</param>
        public static void SetIsDropSource(UIElement soucre, bool value)
        {
            soucre.SetValue(IsDropTargetProperty, value);
        }

        /// <summary>
        /// Gets whether the control can be used as drop target
        /// </summary>
        /// <param name="target">The target control</param>
        /// <returns></returns>
        public static bool GetIsDropTarget(UIElement target)
        {
            return (bool)target.GetValue(IsDropTargetProperty);
        }

        /// <summary>
        /// Sets whether the control can be used as drop target
        /// </summary>
        /// <param name="target">The target control</param>
        /// <param name="value">The new value</param>
        public static void SetIsDropTarget(UIElement target, bool value)
        {
            target.SetValue(IsDropTargetProperty, value);
        }

        /// <summary>
        /// Gets whether the control can be used as drop target
        /// </summary>
        /// <param name="obj">The target control</param>
        /// <returns></returns>
        public static EventType GetDropEventType(DependencyObject obj)
        {
            return (EventType)obj.GetValue(DropEventTypeProperty);
        }

        /// <summary>
        /// Sets whether the control can be used as drop target
        /// </summary>
        /// <param name="target">The target control</param>
        /// <param name="value">The new value</param>
        public static void SetDropEventType(DependencyObject target, EventType value)
        {
            target.SetValue(DropEventTypeProperty, value);
        }

        #endregion

        #endregion

        #region Property Changed Events

        /// <summary>
        /// Raised when IsDropSource property changed
        /// </summary>
        /// <param name="d">The target control</param>
        /// <param name="e">The event args</param>
        private static void IsDropSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Get UIElement
            var uiElement = (UIElement)d;

            if ((bool)e.NewValue)
            {
                uiElement.PreviewMouseLeftButtonDown += DragSourceOnMouseLeftButtonDown;
                uiElement.PreviewMouseLeftButtonUp += DragSourceOnMouseLeftButtonUp;
                uiElement.PreviewMouseMove += DragSourceOnMouseMove;
                // Determine whether the drag-and-drop opration should be canceled.
                // For example, if drag operation moves outside the bounds of the form.
                uiElement.QueryContinueDrag += DragSourceOnQueryContinueDrag;
            }
            else
            {
                uiElement.PreviewMouseLeftButtonDown -= DragSourceOnMouseLeftButtonDown;
                uiElement.PreviewMouseLeftButtonUp -= DragSourceOnMouseLeftButtonUp;
                uiElement.PreviewMouseMove -= DragSourceOnMouseMove;
                // Determine whether the drag-and-drop opration should be canceled.
                // For example, if drag operation moves outside the bounds of the form.
                uiElement.QueryContinueDrag -= DragSourceOnQueryContinueDrag;
            }
        }


        /// <summary>
        /// Raised when IsDropTarget property changed
        /// </summary>
        /// <param name="d">The target control</param>
        /// <param name="e">The event args</param>
        private static void IsDropTargetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Get UIElement
            var uiElement = (UIElement)d;

            if ((bool)e.NewValue)
            {
                // Allows control to be droppabel
                uiElement.AllowDrop = true;

                RegisterDragDropEvents(uiElement, GetDropEventType(d));
            }
            else
            {
                uiElement.AllowDrop = false;

                UnregisterDragDropEvents(uiElement, GetDropEventType(d));

                // Clear drag-and-drop mouse cursor
                Mouse.OverrideCursor = null;
            }

        }

        /// <summary>
        /// Raised when DropEventType property changed
        /// </summary>
        /// <param name="d">The target control</param>
        /// <param name="e">The event args</param>
        private static void DropEventTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = (UIElement)d;

            // If the owner control is not drop target...
            if (!GetIsDropTarget(uiElement))
                return;

            UnregisterDragDropEvents(uiElement, (EventType)e.OldValue);
            RegisterDragDropEvents(uiElement, (EventType)e.NewValue);
        }

        #endregion

        #region Register / Unregister Drag-and-Drop Events


        /// <summary>
        /// Register drag and drop events to the control
        /// </summary>
        /// <param name="uiElement">The target control</param>
        /// <param name="eventType">the event type</param>
        private static void RegisterDragDropEvents(UIElement uiElement, EventType eventType)
        {
            switch (eventType)
            {
                case EventType.Auto:
                    if (uiElement is ItemsControl)
                    {
                        // use normal events for ItemsControls
                        uiElement.DragEnter += DropTargetOnDragEnter;
                        uiElement.DragLeave += DropTargetOnDragLeave;
                        uiElement.DragOver += DropTargetOnDragOver;
                        uiElement.Drop += DropTargetOnDrop;
                        uiElement.GiveFeedback += DropTargetOnGiveFeedback;
                    }
                    else
                    {
                        // using preview events for all other elements than ItemsControls
                        uiElement.PreviewDragEnter += DropTargetOnPreviewDragEnter;
                        uiElement.PreviewDragLeave += DropTargetOnDragLeave;
                        uiElement.PreviewDragOver += DropTargetOnPreviewDragOver;
                        uiElement.PreviewDrop += DropTargetOnPreviewDrop;
                        uiElement.PreviewGiveFeedback += DropTargetOnGiveFeedback;
                    }
                    break;

                case EventType.Tunneled:
                    uiElement.PreviewDragEnter += DropTargetOnPreviewDragEnter;
                    uiElement.PreviewDragLeave += DropTargetOnDragLeave;
                    uiElement.PreviewDragOver += DropTargetOnPreviewDragOver;
                    uiElement.PreviewDrop += DropTargetOnPreviewDrop;
                    uiElement.PreviewGiveFeedback += DropTargetOnGiveFeedback;
                    break;

                case EventType.Bubbled:
                    // use normal events for ItemsControls
                    uiElement.DragEnter += DropTargetOnDragEnter;
                    uiElement.DragLeave += DropTargetOnDragLeave;
                    uiElement.DragOver += DropTargetOnDragOver;
                    uiElement.Drop += DropTargetOnDrop;
                    uiElement.GiveFeedback += DropTargetOnGiveFeedback;
                    break;

                case EventType.TunneledBubbled:
                    uiElement.PreviewDragEnter += DropTargetOnPreviewDragEnter;
                    uiElement.PreviewDragLeave += DropTargetOnDragLeave;
                    uiElement.PreviewDragOver += DropTargetOnPreviewDragOver;
                    uiElement.PreviewDrop += DropTargetOnPreviewDrop;
                    uiElement.PreviewGiveFeedback += DropTargetOnGiveFeedback;
                    uiElement.DragEnter += DropTargetOnDragEnter;
                    uiElement.DragLeave += DropTargetOnDragLeave;
                    uiElement.DragOver += DropTargetOnDragOver;
                    uiElement.Drop += DropTargetOnDrop;
                    uiElement.GiveFeedback += DropTargetOnGiveFeedback;
                    break;

                default:
                    throw new ArgumentException($"Unknow value for eventType {eventType}");
            }
        }

        /// <summary>
        /// Unregister drag and drop events to the control
        /// </summary>
        /// <param name="uiElement">The target control</param>
        /// <param name="eventType">the event type</param>
        private static void UnregisterDragDropEvents(UIElement uiElement, EventType eventType)
        {
            switch (eventType)
            {
                case EventType.Auto:
                    if (uiElement is ItemsControl)
                    {
                        // use normal events for ItemsControls
                        uiElement.DragEnter -= DropTargetOnDragEnter;
                        uiElement.DragLeave -= DropTargetOnDragLeave;
                        uiElement.DragOver -= DropTargetOnDragOver;
                        uiElement.Drop -= DropTargetOnDrop;
                        uiElement.GiveFeedback -= DropTargetOnGiveFeedback;
                    }
                    else
                    {
                        // using preview events for all other elements than ItemsControls
                        uiElement.PreviewDragEnter -= DropTargetOnPreviewDragEnter;
                        uiElement.PreviewDragLeave -= DropTargetOnDragLeave;
                        uiElement.PreviewDragOver -= DropTargetOnPreviewDragOver;
                        uiElement.PreviewDrop -= DropTargetOnPreviewDrop;
                        uiElement.PreviewGiveFeedback -= DropTargetOnGiveFeedback;
                    }
                    break;

                case EventType.Tunneled:
                    uiElement.PreviewDragEnter -= DropTargetOnPreviewDragEnter;
                    uiElement.PreviewDragLeave -= DropTargetOnDragLeave;
                    uiElement.PreviewDragOver -= DropTargetOnPreviewDragOver;
                    uiElement.PreviewDrop -= DropTargetOnPreviewDrop;
                    uiElement.PreviewGiveFeedback -= DropTargetOnGiveFeedback;
                    break;

                case EventType.Bubbled:
                    // use normal events for ItemsControls
                    uiElement.DragEnter -= DropTargetOnDragEnter;
                    uiElement.DragLeave -= DropTargetOnDragLeave;
                    uiElement.DragOver -= DropTargetOnDragOver;
                    uiElement.Drop -= DropTargetOnDrop;
                    uiElement.GiveFeedback -= DropTargetOnGiveFeedback;
                    break;

                case EventType.TunneledBubbled:
                    uiElement.PreviewDragEnter -= DropTargetOnPreviewDragEnter;
                    uiElement.PreviewDragLeave -= DropTargetOnDragLeave;
                    uiElement.PreviewDragOver -= DropTargetOnPreviewDragOver;
                    uiElement.PreviewDrop -= DropTargetOnPreviewDrop;
                    uiElement.PreviewGiveFeedback -= DropTargetOnGiveFeedback;
                    uiElement.DragEnter -= DropTargetOnDragEnter;
                    uiElement.DragLeave -= DropTargetOnDragLeave;
                    uiElement.DragOver -= DropTargetOnDragOver;
                    uiElement.Drop -= DropTargetOnDrop;
                    uiElement.GiveFeedback -= DropTargetOnGiveFeedback;
                    break;

                default:
                    throw new ArgumentException($"Unknow value for eventType {eventType}");
            }
        }

        #endregion
    }
}
