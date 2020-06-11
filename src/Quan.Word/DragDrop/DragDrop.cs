using System.Windows;
using System.Windows.Input;

namespace Quan.Word
{

    public partial class DragDrop
    {
        #region Private Members

        private static DragInfo m_DragInfo;

        #endregion

        #region Drag Source Events

        /// <summary>
        /// Raise on the very first time of drag-and-drop operations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DragSourceOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DoMouseButtonDown(sender, e);
        }


        private static void DragSourceOnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }


        private static void DragSourceOnMouseMove(object sender, MouseEventArgs e)
        {

        }

        private static void DragSourceOnQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {

        }

        #endregion

        #region Drag Target Events

        /// <summary>
        /// Raised while draging item enter drop target control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DropTargetOnDragEnter(object sender, DragEventArgs e)
        {

        }

        /// <summary>
        /// Raise when draging item leave drop target control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DropTargetOnDragLeave(object sender, DragEventArgs e)
        {

        }

        /// <summary>
        /// Raise when hovers over drop target control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DropTargetOnDragOver(object sender, DragEventArgs e)
        {

        }


        /// <summary>
        /// Raise when drop item to drop target control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DropTargetOnDrop(object sender, DragEventArgs e)
        {

        }

        /// <summary>
        /// Give visual feedback during the drag-and-drop operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DropTargetOnGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }


        private static void DropTargetOnPreviewDragEnter(object sender, DragEventArgs e)
        {
        }

        private static void DropTargetOnPreviewDragOver(object sender, DragEventArgs e)
        {
        }

        private static void DropTargetOnPreviewDrop(object sender, DragEventArgs e)
        {

        }

        #endregion

        #region Actual Drag Events

        private static void DoMouseButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {

        }

        #endregion

    }
}
