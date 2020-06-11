using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Quan.Word
{
    public interface IDragInfo
    {
        /// <summary>
        /// Gets or sets the data format which will be used for the drag and drop actions
        /// <see cref="DataObject"/> uses the static formats for transferring data to and
        /// from the system clipboard, and in drag-and-drop operations.
        /// </summary>
        DataFormat DataFormat { get; set; }

        /// <summary>
        /// Gets the <see cref="IDataObject"/> which is used by the drag and drop operation. Set it to
        /// a custom instance if custom drag and drop behavior is needed.
        /// </summary>
        object DataObject { get; set; }

        /// <summary>
        /// Gets or sets the drag data
        /// </summary>
        /// <remarks>This must be set by a drag handler in order for a drag to start</remarks>
        object Data { get; set; }

        /// <summary>
        /// Gets the position of the click that initiated the drag, relative to <see cref="VisualSource"/>
        /// </summary>
        Point DragStartPosition { get; }

        /// <summary>
        /// Gets the point where the cursor was relative to the item being dragged when the drag was started
        /// </summary>
        Point PositionInDraggedItem { get; }

        /// <summary>
        /// Gets or sets the allowed effects for the drag
        /// </summary>
        /// <remarks>
        /// This must be set to a value other than <see cref="DragDropEffects.None"/> by a drag handler in order
        /// for a drag to start
        /// </remarks>
        DragDropEffects Effects { get; set; }

        /// <summary>
        /// Gets the mouse button that initiated the drag. For example,<see cref="MouseButton.Left"/> represents left mouse button.
        /// </summary>
        MouseButton MouseButton { get; set; }

        /// <summary>
        /// Gets the object that a source <see cref="ItemsControl"/> is bound to.
        /// </summary>
        /// <remarks>
        /// If the control that initiated the drag is unbound or not an <see cref="ItemsControl"/>, this will be null
        /// </remarks>
        IEnumerable SourceCollection { get; }

        /// <summary>
        /// Gets the position from where the item was dragged
        /// </summary>
        int SourceIndex { get; }

        /// <summary>
        /// Gets the object that a dragged item is bound to.
        /// </summary>
        /// <remarks>
        /// If the control that initiated the drag is unbound or not an <see cref="ItemsControl"/>, this will be null
        /// </remarks>
        object SourceItem { get; }

        /// <summary>
        /// Gets a collection of objects that the selected items in an <see cref="ItemsControl"/> are bound to.
        /// </summary>
        /// <remarks>
        /// If the control that initiated the drag is unbound or not an ItemsControl, this will be empty.
        /// </remarks>
        IEnumerable SourceItems { get; }

        /// <summary>
        /// Gets the group from a dragged item if the drag is currenty from an <see cref="ItemsControl"/> with groups.
        /// </summary>
        CollectionViewGroup SourceGroup { get; }

        /// <summary>
        /// Gets the control that initiated the drag.
        /// </summary>
        UIElement VisualSource { get; }

        /// <summary>
        /// Gets the controls item in an <see cref="ItemsControl"/> that started the drag.
        /// </summary>
        /// <remarks>
        /// If the control that initiated the drag is an <see cref="ItemsControl"/>, this property will hold the item
        /// container of the clicked item.
        /// For example, if <see cref="VisualSource"/> is a ListBox this will hold a ListBoxItem
        /// </remarks>
        UIElement VisualSourceItem { get; }

        /// <summary>
        /// Gets the FlowDirection of the current drag source.
        /// </summary>
        FlowDirection VisualSourceFlowDirection { get; }

        /// <summary>
        /// Initials a drag-and-drop opeartion.
        /// <param name="dragSource">A reference to the dependency object that is the source of the data being dragged.</param>
        /// <param name="data">A data object that contains the data being dragged.</param>
        /// <param name="allowedEffects">One of the <see cref="DragDropEffects"/> values that specifies permitted effects of the drag-and-drop operation.</param>
        /// <returns>One of the <see cref="DragDropEffects"/> values that specifies permitted effects of the drag-and-drop operation.</returns>
        /// </summary>
        Func<DependencyObject, object, DragDropEffects, DragDropEffects> DragDropHandler { get; set; }

        /// <summary>
        /// Gets the drag drop copy key state indicating the effect of the drag drop operation
        /// </summary>
        DragDropKeyStates DragDropCopyKeyStates { get; }
    }
}
