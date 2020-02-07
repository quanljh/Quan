using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Quan
{
    /// <summary>
    /// Helpers to animate framework elements in specific ways
    /// </summary>
    public static class FrameworkElementAnimations
    {
        /// <summary>
        /// Slides a element in from right
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation</param>
        /// <param name="width">The animation width to animate to.If not specified the elements width is used</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromRight(this FrameworkElement element, float seconds = 0.3f, bool keepMargin = true, int width = 0)
        {
            var sb = new Storyboard();

            //Add slide from right animation
            sb.AddSlideFromRight(seconds, width == 0 ? element.ActualWidth : width, keepMargin: keepMargin);

            //Add fade in animation
            sb.AddFadeIn(seconds);

            //Start animating
            sb.Begin(element);

            //Make element visible
            element.Visibility = Visibility.Visible;

            //Wait for it finish
            await Task.Delay((int)(seconds * 1000));
        }


        /// <summary>
        /// Slides a element in from left
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation</param>
        /// <param name="width">The animation width to animate to.If not specified the elements width is used</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromLeft(this FrameworkElement element, float seconds = 0.3f, bool keepMargin = true, int width = 0)
        {
            var sb = new Storyboard();

            //Add slide from Left animation
            sb.AddSlideFromLeft(seconds, width == 0 ? element.ActualWidth : width, keepMargin: keepMargin);

            //Add fade in animation
            sb.AddFadeIn(seconds);

            //Start animating
            sb.Begin(element);

            //Make element visible
            element.Visibility = Visibility.Visible;

            //Wait for it finish
            await Task.Delay((int)(seconds * 1000));
        }


        /// <summary>
        /// Slides a element out to the left
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation</param>
        /// <param name="width">The animation width to animate to.If not specified the elements width is used</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToLeft(this FrameworkElement element, float seconds = 0.3f, bool keepMargin = true, int width = 0)
        {
            var sb = new Storyboard();

            //Add slide from right animation
            sb.AddSlideToLeft(seconds, width == 0 ? element.ActualWidth : width, keepMargin: keepMargin);

            //Add fade in animation
            sb.AddFadeOut(seconds);

            //Start animating
            sb.Begin(element);

            //Make element visible
            element.Visibility = Visibility.Visible;

            //Wait for it finish
            await Task.Delay((int)(seconds * 1000));
        }

        /// <summary>
        /// Slides a element out to the right
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation</param>
        /// <param name="width">The animation width to animate to.If not specified the elements width is used</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToRight(this FrameworkElement element, float seconds = 0.3f, bool keepMargin = true, int width = 0)
        {
            var sb = new Storyboard();

            //Add slide from right animation
            sb.AddSlideToRight(seconds, width == 0 ? element.ActualWidth : width, keepMargin: keepMargin);

            //Add fade in animation
            sb.AddFadeOut(seconds);

            //Start animating
            sb.Begin(element);

            //Make element visible
            element.Visibility = Visibility.Visible;

            //Wait for it finish
            await Task.Delay((int)(seconds * 1000));
        }

    }
}
