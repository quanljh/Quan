using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Quan.Word
{

    public abstract class DropTargetAdorner : Adorner
    {
        #region Private Members

        private readonly AdornerLayer m_AdornerLayer;

        #endregion

        #region Public Properties

        public DropInfo DropInfo { get; set; }

        /// <summary>
        /// Gets or Sets the pen which can be used for the render process.
        /// </summary>
        public Pen Pen { get; set; } = new Pen(Brushes.Gray, 2);

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="adornedElement">The root element of window to get a adorner layer</param>
        /// <param name="dropInfo">The underlying drop information of drag-and-drop actions</param>
        protected DropTargetAdorner(UIElement adornedElement, DropInfo dropInfo)
            : base(adornedElement)
        {
            DropInfo = dropInfo;
            IsHitTestVisible = false;
            AllowDrop = false;
            SnapsToDevicePixels = true;
            m_AdornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
            if (m_AdornerLayer == null) throw new NullReferenceException("Can't find AdornerLayer on your app window, try to add it to your custom window");
            m_AdornerLayer.Add(this);
        }

        #endregion

        #region Methods

        internal static DropTargetAdorner Create(Type type, UIElement adornedElement, IDropInfo dropInfo)
        {
            if (!typeof(DropTargetAdorner).IsAssignableFrom(type))
            {
                throw new InvalidOperationException("The requested adorner class does not derive from DropTargetAdorner.");
            }
            return type.GetConstructor(new[] { typeof(UIElement), typeof(DropInfo) })?.Invoke(new object[] { adornedElement, dropInfo }) as DropTargetAdorner;
        }

        /// <summary>
        /// Clear the <see cref="AdornerLayer"/>
        /// </summary>
        public void Detach()
        {
            m_AdornerLayer.Remove(this);
        }

        #endregion
    }
}
