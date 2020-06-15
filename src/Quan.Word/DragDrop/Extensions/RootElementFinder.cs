using Quan.Word.ViewHelper;
using System.Windows;
using System.Windows.Controls;

namespace Quan.Word
{
    public static class RootElementFinder
    {
        public static UIElement FindRoot(DependencyObject visual)
        {
            var parentWindow = Window.GetWindow(visual);
            var rootElement = parentWindow?.Content as UIElement;
            if (rootElement == null)
            {
                if (Application.Current != null && Application.Current.MainWindow != null)
                {
                    rootElement = Application.Current.MainWindow.Content as UIElement;
                }
                if (rootElement == null)
                {
                    rootElement = visual.FindVisualParent<Page>() ?? visual.FindVisualParent<UserControl>() as UIElement;
                }
            }
            //      i don't want the fu... windows forms reference
            //      if (rootElement == null) {
            //          var elementHost = m_DragInfo.VisualSource.GetVisualAncestor<ElementHost>();
            //          rootElement = elementHost != null ? elementHost.Child : null;
            //      }
            return rootElement;
        }
    }
}
