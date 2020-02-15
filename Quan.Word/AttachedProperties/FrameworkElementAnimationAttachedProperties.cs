using System.Threading.Tasks;
using System.Windows;

namespace Quan
{
    /// <summary>
    /// A base class to run any animation method when a boolean is set to true
    /// and a reverse animation when set to false
    /// </summary>
    /// <typeparam name="Parent">The parent class to be the attached property</typeparam>
    public abstract class AnimateBaseProperty<Parent> : BaseAttachedProperty<Parent, bool>
        where Parent : BaseAttachedProperty<Parent, bool>, new()
    {

        #region Protected Properties

        /// <summary>
        /// True if this is the very first time the value has been update
        /// Used to make sure we run the logic at least once during first load
        /// </summary>
        protected bool mFirstFire = true;

        #endregion

        #region Public Properties

        /// <summary>
        /// A flag indicating if this is the first time this property has been loaded
        /// </summary>
        public bool FirstLoad { get; set; } = true;

        #endregion

        public override void OnValueUpdated(DependencyObject sender, object value)
        {
            //Get the framework element
            if (!(sender is FrameworkElement element))
                return;

            //Don't fire when the value doesn't change
            if ((bool)sender.GetValue(ValueProperty) == (bool)value && !mFirstFire)
                return;

            // No longer first fire
            mFirstFire = false;

            //On first load...
            if (FirstLoad)
            {
                //Start off hidden before we decide how to animate
                //If we are to be animated out initially
                if (!(bool)value)
                    element.Visibility = Visibility.Hidden;

                //Creat a single self-unhookable event
                //for the elements Loaded event
                RoutedEventHandler onLoaded = null;
                onLoaded = async (ss, ee) =>
                {
                    //Unhook ourselves
                    element.Loaded -= onLoaded;

                    // Slight delay after load is needed for some elements to get laid out
                    // and their width/heights correctly calculated
                    await Task.Delay(5);

                    //Do desired animation
                    DoAnimation(element, (bool)value);

                    //No longer in first load
                    FirstLoad = false;
                };

                // Hook into the loaded event of the element
                element.Loaded += onLoaded;
            }
            else
                //Do desired animation
                DoAnimation(element, (bool)value);

        }

        /// <summary>
        /// The animation method that is fired when the value changes
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="value">The new value</param>
        protected virtual void DoAnimation(FrameworkElement element, bool value)
        {

        }
    }

    /// <summary>
    /// Animates a framwork element sliding it in from the left on show
    /// and sliding out to the left on hide
    /// </summary>
    public class AnimateSlideInFromeLeftProperty : AnimateBaseProperty<AnimateSlideInFromeLeftProperty>
    {
        protected override async void DoAnimation(FrameworkElement element, bool value)
        {
            ////Don't bother animating in design time
            //if (DesignerProperties.GetIsInDesignMode(element))
            //    return;

            if (value)
                //Animate in
                await element.SlideAndFadeInFromLeft(FirstLoad ? 0 : 0.3f, false);
            else
                //Animate out
                await element.SlideAndFadeOutToLeft(FirstLoad ? 0 : 0.3f, false);

        }
    }


    /// <summary>
    /// Animates a framwork element sliding up from the bottom on show
    /// and sliding out to the bottom on hide
    /// </summary>
    public class AnimateSlideInFromeBottomProperty : AnimateBaseProperty<AnimateSlideInFromeBottomProperty>
    {
        protected override async void DoAnimation(FrameworkElement element, bool value)
        {
            ////Don't bother animating in design time
            //if (DesignerProperties.GetIsInDesignMode(element))
            //    return;

            if (value)
                //Animate in
                await element.SlideAndFadeInFromBottom(FirstLoad ? 0 : 0.3f, false);
            else
                //Animate out
                await element.SlideAndFadeOutToBottom(FirstLoad ? 0 : 0.3f, false);

        }
    }


    /// <summary>
    /// Animates a framework element sliding up from the bottom on show
    /// and sliding out to the bottom on hide
    /// NOTE: Keeps the margin
    /// </summary>
    public class AnimateSlideInFromBottomMarginProperty : AnimateBaseProperty<AnimateSlideInFromBottomMarginProperty>
    {
        protected override async void DoAnimation(FrameworkElement element, bool value)
        {
            if (value)
                // Animate in
                await element.SlideAndFadeInFromBottom(FirstLoad ? 0 : 0.3f, keepMargin: true);
            else
                // Animate out
                await element.SlideAndFadeOutToBottom(FirstLoad ? 0 : 0.3f, keepMargin: true);
        }
    }


    /// <summary>
    /// Animates a framwork element fade in on show
    /// and fade out on hide
    /// </summary>
    public class AnimateFadeInProperty : AnimateBaseProperty<AnimateFadeInProperty>
    {
        protected override async void DoAnimation(FrameworkElement element, bool value)
        {
            ////Don't bother animating in design time
            //if (DesignerProperties.GetIsInDesignMode(element))
            //    return;

            if (value)
                //Animate in
                await element.FadeIn(FirstLoad ? 0 : 0.3f);
            else
                //Animate out
                await element.FadeOut(FirstLoad ? 0 : 0.3f);

        }
    }
}
