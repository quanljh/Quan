﻿using Quan.Word.Core;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Quan
{

    public class BasePage : UserControl
    {
        #region Public Properties

        /// <summary>
        /// The animation to play when the page is first loaded
        /// </summary>
        public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.SlideAndFadeInFromRight;

        /// <summary>
        /// The animation to play when the page is unloaded
        /// </summary>
        public PageAnimation PageUnloadAnimation { get; set; } = PageAnimation.SlideAndFadeOutToleft;

        /// <summary>
        /// The time any slide animation take to complete
        /// </summary>
        public float SlideSeconds { get; set; } = 0.4f;

        /// <summary>
        /// A flag to indicate if this page should animate out on load
        /// Usefull for when we are move the page to another frame
        /// </summary>
        public bool ShouldAnimateOut { get; set; }
        #endregion

        #region Construct

        public BasePage()
        {
            //Don't bother animating in design time
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            if (PageLoadAnimation != PageAnimation.none)
                Visibility = Visibility.Collapsed;

            Loaded += BasePage_Loaded;
        }

        #endregion

        #region Animation Loaded / Unloaded

        private async void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            //If we are setup to animate out on load
            if (ShouldAnimateOut)
                await AnimateOut();
            else
                await AnimateIn();
        }

        public async Task AnimateIn()
        {
            if (PageLoadAnimation == PageAnimation.none)
                return;

            switch (PageLoadAnimation)
            {
                case PageAnimation.SlideAndFadeInFromRight:

                    //Start the Animation
                    if (Application.Current.MainWindow != null)
                        await this.SlideAndFadeIn(AnimationSlideInDirection.Right, false, SlideSeconds, size: (int)Application.Current.MainWindow.Width);

                    break;
            }
        }


        public async Task AnimateOut()
        {
            if (PageUnloadAnimation == PageAnimation.none)
                return;

            switch (PageUnloadAnimation)
            {
                case PageAnimation.SlideAndFadeOutToleft:

                    //Start the Animation
                    await this.SlideAndFadeOut(AnimationSlideInDirection.Left, SlideSeconds);

                    break;
            }
        }

        #endregion

    }

    /// <summary>
    /// The base page with added viewmodel support
    /// </summary>
    /// <typeparam name="VM"></typeparam>
    public class BasePage<VM> : BasePage
        where VM : ViewModelBase, new()
    {
        #region Private Properties

        private VM _viewModel;

        #endregion

        #region Public Properties

        public VM ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel != null && _viewModel == value)
                    return;
                _viewModel = value;

                DataContext = _viewModel;
            }
        }

        #endregion

        #region Construct

        public BasePage() : base()
        {
            //Creat a default view model
            ViewModel = new VM();
        }

        #endregion



    }
}
