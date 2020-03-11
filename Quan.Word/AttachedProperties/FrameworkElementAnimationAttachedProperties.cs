﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
        /// True if this is the very first time the value has been updated
        /// Used to make sure we run the logic at least once during first load
        /// </summary>
        protected Dictionary<DependencyObject, bool> mAlreadyLoaded = new Dictionary<DependencyObject, bool>();

        /// <summary>
        /// The most recent value used if we get a value changed before we do the first load
        /// </summary>
        protected Dictionary<DependencyObject, bool> mFirstLoadValue = new Dictionary<DependencyObject, bool>();


        #endregion

        public override void OnValueUpdated(DependencyObject sender, object value)
        {
            //Get the framework element
            if (!(sender is FrameworkElement element))
                return;

            //Don't fire when the value doesn't change
            if ((bool)sender.GetValue(ValueProperty) == (bool)value && mAlreadyLoaded.ContainsKey(sender))
                return;

            //On first load...
            if (!mAlreadyLoaded.ContainsKey(sender))
            {
                // Flag that we are in first load but have not finished it
                mAlreadyLoaded[sender] = false;

                //Start off hidden before we decide how to animate
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
                    DoAnimation(element, mFirstLoadValue.ContainsKey(sender) ? mFirstLoadValue[sender] : (bool)value, true);

                    // Flag that we have finished first load
                    mAlreadyLoaded[sender] = true;
                };

                // Hook into the loaded event of the element
                element.Loaded += onLoaded;
            }
            // If we have started a first load but not fired the animation yet, update the property
            else if (mAlreadyLoaded[sender] == false)
                mFirstLoadValue[sender] = (bool)value;
            else
                //Do desired animation
                DoAnimation(element, (bool)value, false);

        }

        /// <summary>
        /// The animation method that is fired when the value changes
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="value">The new value</param>
        /// <param name="firstLoad">Ture if the element is in first load</param>
        protected virtual void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
        {

        }
    }


    /// <summary>
    /// Fade In an image once the source changes
    /// </summary>
    public class FadeInImageOnLoadProperty : AnimateBaseProperty<FadeInImageOnLoadProperty>
    {
        public override void OnValueUpdated(DependencyObject sender, object value)
        {
            base.OnValueUpdated(sender, value);

            // Make sure we have an image
            if (!(sender is Image image))
                return;

            // If we want to animate in...
            if ((bool)value)
                image.TargetUpdated += Image_TargetUpdated;
            else
                image.TargetUpdated -= Image_TargetUpdated;
        }

        private async void Image_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            await (sender as Image).FadeIn(false);
        }
    }

    /// <summary>
    /// Animates a framwork element sliding it in from the left on show
    /// and sliding out to the left on hide
    /// </summary>
    public class AnimateSlideInFromeLeftProperty : AnimateBaseProperty<AnimateSlideInFromeLeftProperty>
    {
        protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
        {
            if (value)
                // Animate in
                await element.SlideAndFadeIn(AnimationSlideInDirection.Left, firstLoad, firstLoad ? 0 : 0.3f, keepMargin: false);
            else
                // Animate out
                await element.SlideAndFadeOut(AnimationSlideInDirection.Left, firstLoad ? 0 : 0.3f, keepMargin: false);

        }
    }


    /// <summary>
    /// Animates a framwork element sliding up from the bottom on show
    /// and sliding out to the bottom on hide
    /// </summary>
    public class AnimateSlideInFromeBottomProperty : AnimateBaseProperty<AnimateSlideInFromeBottomProperty>
    {
        protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
        {
            if (value)
                // Animate in
                await element.SlideAndFadeIn(AnimationSlideInDirection.Bottom, firstLoad, firstLoad ? 0 : 0.3f, keepMargin: false);
            else
                // Animate out
                await element.SlideAndFadeOut(AnimationSlideInDirection.Bottom, firstLoad ? 0 : 0.3f, keepMargin: false);
        }
    }


    /// <summary>
    /// Animates a framwork element sliding up from the bottom on load
    /// if the value is true
    /// </summary>
    public class AnimateSlideInFromeBottomOnLoadProperty : AnimateBaseProperty<AnimateSlideInFromeBottomOnLoadProperty>
    {
        protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
        {
            // Animate in
            await element.SlideAndFadeIn(AnimationSlideInDirection.Bottom, !value, !value ? 0 : 0.3f, false);
        }
    }


    /// <summary>
    /// Animates a framework element sliding up from the bottom on show
    /// and sliding out to the bottom on hide
    /// NOTE: Keeps the margin
    /// </summary>
    public class AnimateSlideInFromBottomMarginProperty : AnimateBaseProperty<AnimateSlideInFromBottomMarginProperty>
    {
        protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
        {
            if (value)
                // Animate in
                await element.SlideAndFadeIn(AnimationSlideInDirection.Bottom, firstLoad, firstLoad ? 0 : 0.3f, keepMargin: true);
            else
                // Animate out
                await element.SlideAndFadeOut(AnimationSlideInDirection.Bottom, firstLoad ? 0 : 0.3f, keepMargin: true);
        }
    }


    /// <summary>
    /// Animates a framwork element fade in on show
    /// and fade out on hide
    /// </summary>
    public class AnimateFadeInProperty : AnimateBaseProperty<AnimateFadeInProperty>
    {
        protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
        {
            if (value)
                // Animate in
                await element.FadeIn(firstLoad, firstLoad ? 0 : 0.3f);
            else
                // Animate out
                await element.FadeOut(firstLoad ? 0 : 0.3f);
        }
    }
}
