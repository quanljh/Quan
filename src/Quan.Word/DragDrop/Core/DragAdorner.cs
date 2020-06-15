using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Quan.Word
{
    internal class DragAdorner : Adorner
    {
        public DragAdorner(UIElement adornedElement, UIElement adornment, Point translation, DragDropEffects effects = DragDropEffects.None)
            : base(adornedElement)
        {
            Translation = translation;
            // An adorner layer is guaranteed to be at higher Z-order than the element being adorned
            // so adorners are always rendered on top of the adorned element
            m_AdornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
            if (m_AdornerLayer == null) throw new NullReferenceException("Can't find AdornerLayer on your app window, try to add it to your custom window");
            m_AdornerLayer?.Add(this);
            m_Adornment = adornment;
            IsHitTestVisible = false;
            Effects = effects;
        }

        public Point Translation { get; private set; }

        public DragDropEffects Effects { get; private set; }

        public Point MousePosition
        {
            get { return m_MousePosition; }
            set
            {
                if (m_MousePosition != value)
                {
                    m_MousePosition = value;
                    m_AdornerLayer.Update(AdornedElement);
                }
            }
        }

        public void Detatch()
        {
            m_AdornerLayer.Remove(this);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            m_Adornment.Arrange(new Rect(finalSize));
            return finalSize;
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            var result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(MousePosition.X + Translation.X, MousePosition.Y + Translation.Y));

            return result;
        }

        internal void Move(Point newAdornerPosition, Point anchorPoint, ref Point adornerMousePosition, ref Size adornerSize)
        {
            if (newAdornerPosition.X >= 0 && newAdornerPosition.Y >= 0)
            {
                adornerMousePosition = newAdornerPosition;
            }

            if (RenderSize.Width > 0 && RenderSize.Height > 0)
            {
                adornerSize = RenderSize;
            }

            var offsetX = adornerSize.Width * -anchorPoint.X;
            var offsetY = adornerSize.Height * -anchorPoint.Y;
            adornerMousePosition.Offset(offsetX, offsetY);

            if (adornerMousePosition.X < 0)
            {
                adornerMousePosition.X = 0;
            }
            else
            {
                var maxAdornerPosX = AdornedElement.RenderSize.Width;
                var adornerPosRightX = (adornerMousePosition.X + Translation.X + adornerSize.Width);
                if (adornerPosRightX > maxAdornerPosX)
                {
                    adornerMousePosition.Offset(-adornerPosRightX + maxAdornerPosX, 0);
                }
            }

            if (adornerMousePosition.Y < 0)
            {
                adornerMousePosition.Y = 0;
            }
            else
            {
                var maxAdornerPosY = AdornedElement.RenderSize.Height;
                var adornerPosRightY = (adornerMousePosition.Y + Translation.Y + adornerSize.Height);
                if (adornerPosRightY > maxAdornerPosY)
                {
                    adornerMousePosition.Offset(0, -adornerPosRightY + maxAdornerPosY);
                }
            }

            // Update layout of adorner
            MousePosition = adornerMousePosition;
            // Move to new position
            InvalidateVisual();
        }

        protected override Visual GetVisualChild(int index)
        {
            return m_Adornment;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            m_Adornment.Measure(constraint);
            return m_Adornment.DesiredSize;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        private readonly AdornerLayer m_AdornerLayer;
        private readonly UIElement m_Adornment;
        private Point m_MousePosition;
    }
}
