﻿using Quan.Word.Core;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Quan.Word
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
        public ApplicationPage CurrentPage
        {
            get => (ApplicationPage)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        /// <summary>
        /// Registers <see cref="CurrentPage"/> as a dependecy property
        /// </summary>
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(nameof(CurrentPage), typeof(ApplicationPage), typeof(PageHost), new UIPropertyMetadata(default(ApplicationPage), null, CurrentPagePropertyChanged));


        /// <summary>
        /// The current page to show in the page host
        /// </summary>
        public ViewModelBase CurrentPageViewModel
        {
            get => (ViewModelBase)GetValue(CurrentPageViewModelProperty);
            set => SetValue(CurrentPageViewModelProperty, value);
        }

        /// <summary>
        /// Registers <see cref="CurrentPageViewModel"/> as a dependency property
        /// </summary>
        public static readonly DependencyProperty CurrentPageViewModelProperty =
            DependencyProperty.Register(nameof(CurrentPageViewModel), typeof(ViewModelBase), typeof(PageHost), new UIPropertyMetadata());

        #endregion

        #region Constructor

        /// <summary>
        /// Defaut constructor
        /// </summary>
        public PageHost()
        {
            InitializeComponent();

            //If we are in DesignMode, show the current page 
            //as the dependency property does not fire
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                NewPage.Content = IoC.Application.CurrentPage.ToBasePage();
            }

        }
        #endregion

        #region Property Changed Events

        /// <summary>
        /// Called when the <see cref="CurrentPage"/> value has changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        private static object CurrentPagePropertyChanged(DependencyObject d, object value)
        {
            //Get Current values 
            var currentPage = (ApplicationPage)d.GetValue(CurrentPageProperty);
            var currentPageViewModel = d.GetValue(CurrentPageViewModelProperty);

            //Get the frames
            var newPageFrame = (d as PageHost)?.NewPage;
            var oldPageFrame = (d as PageHost)?.OldPage;

            //If the current page hasn't changed
            //just update the view model
            if (newPageFrame?.Content is BasePage page &&
                page.ToApplicationPage() == currentPage)
            {
                //Just update the view model
                page.ViewModelObject = currentPageViewModel;

                return value;
            }

            if (newPageFrame == null || oldPageFrame == null)
                return value;

            //Store the current page content as the old page
            var oldPageContent = newPageFrame.Content;

            //Remove current page from new page frame
            newPageFrame.Content = null;

            //Move the previous page into the old page frame
            oldPageFrame.Content = oldPageContent;

            //Animate out previous page
            if (oldPageContent is BasePage oldPage)
            {
                //Tell old page to animate out 
                oldPage.ShouldAnimateOut = true;

                //Once it done,remove it
                Task.Delay((int)(oldPage.SlideSeconds * 1000)).ContinueWith(t =>
                {
                    Application.Current.Dispatcher?.Invoke(() => oldPageFrame.Content = null);
                });
            }

            //Set the new page content
            newPageFrame.Content = currentPage.ToBasePage(currentPageViewModel);


            return value;
        }
        #endregion
    }
}