using System;
using System.Collections;
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
            DragDropCopyKeyStates = DragDrop.GetDragDropCopyKeyState(VisualSource);

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
                SourceGroup = itemsControl.FindGroup()
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
        public IEnumerable SourceItems { get; }

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
        public DragDropKeyStates DragDropCopyKeyStates { get; }
        #endregion
    }
}
