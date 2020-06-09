using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Quan.Word.ViewHelper
{

    public static class VisualTreeExtentions
    {
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
