﻿using System;
using System.Collections;
using System.Linq;
using System.Windows;

namespace Quan.Word
{
    /// <summary>
    /// The default drag handler for GongSolutions.Wpf.DragDrop.
    /// </summary>
    public class DefaultDragHandler : IDragSource
    {
        /// <inheritdoc />
        public virtual void StartDrag(IDragInfo dragInfo)
        {
            var items = TypeUtilities.CreateDynamicallyTypedList(dragInfo.SourceItems).Cast<object>().ToList();
            if (items.Count > 1)
            {
                dragInfo.Data = items;
            }
            else
            {
                // special case: if the single item is an enumerable then we can not drop it as single item
                var singleItem = items.FirstOrDefault();
                if (singleItem is IEnumerable && !(singleItem is string))
                {
                    dragInfo.Data = items;
                }
                else
                {
                    dragInfo.Data = singleItem;
                }
            }

            dragInfo.Effects = dragInfo.Data != null ? DragDropEffects.Copy | DragDropEffects.Move : DragDropEffects.None;
        }

        /// <inheritdoc />
        public virtual bool CanStartDrag(IDragInfo dragInfo)
        {
            return true;
        }

        /// <inheritdoc />
        public virtual void Dropped(IDropInfo dropInfo)
        {
        }

        /// <inheritdoc />
        public virtual void DragDropOperationFinished(DragDropEffects operationResult, IDragInfo dragInfo)
        {
            // nothing here
        }

        /// <inheritdoc />
        public virtual void DragCancelled()
        {
        }

        /// <inheritdoc />
        public virtual bool TryCatchOccurredException(Exception exception)
        {
            return false;
        }
    }
}