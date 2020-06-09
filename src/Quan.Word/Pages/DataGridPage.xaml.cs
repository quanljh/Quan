using System.Windows;
using System.Windows.Controls;
using Quan.Word.ViewHelper;

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

        private void ScrollOnScrollChanged(object sender, ScrollChangedEventArgs e)
        {

        }

        private void DataGridPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is DataGridPageViewModel vm)
                vm.DataGridPage = this;
        }
    }
}
