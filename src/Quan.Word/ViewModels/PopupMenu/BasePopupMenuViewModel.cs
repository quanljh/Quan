using Quan.Word.Core;

namespace Quan.Word
{

    /// <summary>
    /// A view model for any popup menus
    /// </summary>
    public class BasePopupMenuViewModel : ViewModelBase
    {
        #region Public Properties

        /// <summary>
        /// The background color of the bubble in ARGB value
        /// </summary>
        private string _bubbleBackground;

        public string BubbleBackground
        {
            get => _bubbleBackground;
            set => SetProperty(ref _bubbleBackground, value);
        }

        /// <summary>
        /// The alignment of the bubble arrow
        /// </summary>
        private ElementHorizontalAlignment _arrowAlignment;

        public ElementHorizontalAlignment ArrowAlignment
        {
            get => _arrowAlignment;
            set => SetProperty(ref _arrowAlignment, value);
        }

        /// <summary>
        /// The content inside of this popup menu
        /// </summary>
        private MenuViewModel _content;

        public MenuViewModel Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BasePopupMenuViewModel()
        {
            // Set default values
            // TODO: Move colors into Core and make use of it here
            BubbleBackground = "ffffff";
            ArrowAlignment = ElementHorizontalAlignment.Left;
        }

        #endregion
    }

}
