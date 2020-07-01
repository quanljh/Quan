namespace Quan.Word
{
    /// <summary>
    /// Interaction logic for DataGridPage.xaml
    /// </summary>
    public partial class TextBoxPage : BasePage<TextBoxPageViewModel>
    {
        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TextBoxPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public TextBoxPage(TextBoxPageViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }

        #endregion
    }
}
