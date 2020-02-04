using System.Windows;
using System.Windows.Controls;

namespace Quan.Controls
{
    /// <summary>
    /// Interaction logic for PageHost.xaml
    /// </summary>
    public partial class PageHost : UserControl
    {

        #region Dependency Properties
        /// <summary>
        /// The current page to show in the page host
        /// </summary>
        public BasePage CurrentPage
        {
            get => (BasePage)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        /// <summary>
        /// Registers <see cref="CurrentPage"/> as a dependecy property
        /// </summary>
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(nameof(CurrentPage), typeof(BasePage), typeof(PageHost), new UIPropertyMetadata(CurrentPagePropertyChanged));

        #endregion

        #region Constructor

        /// <summary>
        /// Defaut constructor
        /// </summary>
        public PageHost()
        {
            InitializeComponent();
        }
        #endregion

        #region Property Changed Events

        /// <summary>
        /// Called when the <see cref="CurrentPage"/> value has changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void CurrentPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Get the frames
            if (d is PageHost pageHost
                && pageHost.NewPage is Frame newPageFrame
                && pageHost.OldPage is Frame oldPageFrame)
            {
                //Store the current page content as the old page
                var oldPageContent = newPageFrame.Content;

                //Remove current page from new page frame
                newPageFrame.Content = null;

                //Move the previous page into the old page frame
                oldPageFrame.Content = oldPageContent;

                //Animate out previous page
                if (oldPageContent is BasePage oldPage)
                    oldPage.ShouldAnimateOut = true;

                //Set the new page content
                newPageFrame.Content = e.NewValue;
            }

        }
        #endregion
    }
}
