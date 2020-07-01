using System.Windows;
using System.Windows.Controls;

namespace Quan.ControlLibrary
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Quan.Word.CustomControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Quan.Word.CustomControls;assembly=Quan.Word.CustomControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:QuanTextBox/>
    ///
    /// </summary>
    public class QuanTextBox : TextBox
    {

        #region Dependency Properties


        public string GuideText
        {
            get => (string)GetValue(GuideTextProperty);
            set => SetValue(GuideTextProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="GuideText"/> dependency property to display some guide text when TextBox is empty
        /// </summary>
        public static readonly DependencyProperty GuideTextProperty =
            DependencyProperty.Register("GuideText", typeof(string), typeof(QuanTextBox), new PropertyMetadata(default(string)));



        #endregion

        #region Constructor

        static QuanTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(QuanTextBox), new FrameworkPropertyMetadata(typeof(QuanTextBox)));
        }

        #endregion
    }
}
