namespace Quan.Word
{
    /// <summary>
    /// Interaction logic for DataGridPage.xaml
    /// </summary>
    public partial class DataGridPage : BasePage<DataGridPageViewModel>
    {
        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataGridPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public DataGridPage(DataGridPageViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }

        #endregion

        /// <inheritdoc />
        protected override void OnViewModelChanged()
        {
            if (DataContext is DataGridPageViewModel vm)
                vm.DataGridPage = this;
        }
    }
}
