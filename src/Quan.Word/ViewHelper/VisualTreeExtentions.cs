using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Quan.Word.ViewHelper
{

    public static class VisualTreeExtentions
    {
        /// <summary>
        /// Find the first parent element within specific type from source element's visual tree
        /// </summary>
        /// <typeparam name="T">The type of parent element</typeparam>
        /// <param name="obj">The source element</param>
        /// <param name="name">The name of parent element</param>
        /// <returns></returns>
        public static T FindVisualParent<T>(this DependencyObject obj, string name = null) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is T element && (element.Name == name || string.IsNullOrEmpty(name)))
                {
                    return element;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }


        /// <summary>
        /// Find the first child element within specific type from source element's visual tree
        /// </summary>
        /// <typeparam name="TChildItem">The child element's type</typeparam>
        /// <param name="obj">The source element</param>
        /// <returns>The first Child element</returns>
        public static TChildItem FindVisualChild<TChildItem>(this DependencyObject obj) where TChildItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is TChildItem item)
                {
                    return item;
                }

                TChildItem childOfChild = FindVisualChild<TChildItem>(child);
                if (childOfChild != null) return childOfChild;
            }
            return null;
        }


        /// <summary>
        /// Find all child elements within specific type from source element's visual tree
        /// </summary>
        /// <typeparam name="TChildItem">The child element's type</typeparam>
        /// <param name="obj">The source element</param>
        /// <returns>The Child elements</returns>
        public static List<TChildItem> FindVisualChilds<TChildItem>(this DependencyObject obj) where TChildItem : DependencyObject
        {
            List<TChildItem> childItems = new List<TChildItem>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is TChildItem item)
                {
                    childItems.Add(item);

                    List<TChildItem> childs = FindVisualChilds<TChildItem>(child);
                    if (childs != null) childItems.AddRange(childs);
                }
                else
                {
                    List<TChildItem> childs = FindVisualChilds<TChildItem>(child);
                    if (childs != null) childItems.AddRange(childs);
                }
            }
            return childItems;
        }
    }
}
