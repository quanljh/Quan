using Quan.Word.Core;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Quan.Word
{

    public class BasePage : UserControl
    {
        #region Private Member

        /// <summary>
        /// The View Model associated with this page
        /// </summary>
        private object mViewModel;

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
        public float SlideSeconds { get; set; } = 0.4f;

        /// <summary>
        /// A flag to indicate if this page should animate out on load
        /// Usefull for when we are move the page to another frame
        /// </summary>
        public bool ShouldAnimateOut { get; set; }

        public object ViewModelObject
        {
            get => mViewModel;
            set
            {
                // If nothing has changed, return
                if (mViewModel == value)
                    return;

                // Update the value
                mViewModel = value;

                // Fire the view model changed method
                OnViewModelChanged();

                // Set the data context for this page
                DataContext = mViewModel;
            }
        }

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

        /// <summary>
        /// Fired when the view model changed
        /// </summary>
        protected virtual void OnViewModelChanged()
        {

        }

    }

    /// <summary>
    /// The base page with added viewmodel support
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasePage<T> : BasePage
        where T : ViewModelBase, new()
    {
        #region Public Properties

        /// <summary>
        /// The view model associated with this page
        /// </summary>
        public T ViewModel
        {
            get => (T)ViewModelObject;
            set => ViewModelObject = value;
        }

        #endregion

        #region Construct

        /// <summary>
        /// Default Constructor
        /// </summary>

        public BasePage()
        {
            // If in design time mode...
            if (DesignerProperties.GetIsInDesignMode(this))
                // Just use a new instance of the VM
                ViewModel = new T();
            else
                //Create a default view model
                ViewModel = Framework.Service<T>();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use, if any</param>
        public BasePage(T specificViewModel = null) : base()
        {
            if (specificViewModel != null)
                ViewModel = specificViewModel;
            else
            {
                // If in design time mode...
                if (DesignerProperties.GetIsInDesignMode(this))
                    // Just use a new instance of the VM
                    ViewModel = new T();
                else
                    //Create a default view model
                    ViewModel = Framework.Service<T>() ?? new T();
            }
        }

        #endregion

    }
}
