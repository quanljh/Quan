using Quan.Animation;
using Quan.Word.Core;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Quan.Pages
{
    public class BasePage<VM> : Page
        where VM : ViewModelBase, new()
    {
        #region Private Properties

        private VM _viewModel;

        #endregion

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
        public float SlideSeconds { get; set; } = 0.8f;

        public VM ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel != null && _viewModel == value) return;
                _viewModel = value;

                DataContext = _viewModel;
            }
        }

        #endregion

        #region Construct

        public BasePage()
        {
            if (PageLoadAnimation != PageAnimation.none)
                Visibility = Visibility.Collapsed;

            Loaded += BasePage_Loaded;

            ViewModel = new VM();
        }

        #endregion

        #region Animation Loaded / Unloaded

        private async void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
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
                    await this.SlideAndFadeInFromRight(SlideSeconds);

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
                    await PageAnimations.SlideAndFadeOutToLeft(this, SlideSeconds);

                    break;
            }
        }

        #endregion


    }
}
