using System;
using System.Windows;

namespace Quan.AttachedProperties
{
    /// <summary>
    /// A base attached property to replace the vanilla WPF attached property
    /// </summary>
    /// <typeparam name="Parent">The parent class to be the attached property</typeparam>
    /// <typeparam name="Property">The type of this attached property</typeparam>
    /// where T : [baseTypeName] The type argument must be or derive from the specified base class
    public abstract class BaseAttachedProperty<Parent, Property>
        where Parent : BaseAttachedProperty<Parent, Property>, new()
    {
        #region Public Events

        /// <summary>
        /// Fired when the value changes
        /// </summary>
        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (sender, e) => { };

        #endregion

        #region Public Properties

        public static Parent Instansce { get; private set; } = new Parent();

        #endregion

        #region Attached Property Definitions

        /// <summary>
        /// The attach property for this class
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached("Value", typeof(Property), typeof(BaseAttachedProperty<Parent, Property>)
                , new PropertyMetadata(new PropertyChangedCallback(OnValuePropertyChanged)));

        /// <summary>
        /// The callback event when the <see cref="ValueProperty"/> is changed
        /// </summary>
        /// <param name="d">The UI Element that had it's property changed</param>
        /// <param name="e">The arguments for the event</param>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Call the parent function
            Instansce.OnValueChanged(d, e);

            //Call event listners
            Instansce.ValueChanged(d, e);
        }

        /// <summary>
        /// Gets the attached property
        /// </summary>
        /// <param name="ob">The element to get the property from</param>
        /// <returns></returns>
        public static Property GetValue(DependencyObject ob) => (Property)ob.GetValue(ValueProperty);

        /// <summary>
        /// Sets the attached property
        /// </summary>
        /// <param name="ob">The element to get the property from</param>
        /// <param name="value">The value to set the property</param>
        public static void SetValue(DependencyObject ob, Property value) => ob.SetValue(ValueProperty, value);

        #endregion

        #region Event Methods

        /// <summary>
        /// The method that is called when any attached property of this type is changed
        /// </summary>
        /// <param name="sender">The UI element that this property was changed for</param>
        /// <param name="e">The arguments for this event</param>
        public virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion
    }
}
