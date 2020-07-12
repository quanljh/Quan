using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Quan.Word
{
    /// <summary>
    /// Interaction logic for DataGridPage.xaml
    /// </summary>
    public partial class DataGridPage : BasePage<DataGridPageViewModel>
    {
        private BooleanToCollapsedConverter booleanToCollapsedConverter = new BooleanToCollapsedConverter();

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

        private void PatientDataGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is DataGrid dataGrid))
                return;

            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                var widthBinding = new Binding()
                {
                    Source = Resources["BindingProxy"],
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.TwoWay,
                    Path = new PropertyPath($"Data.DataGridColumnSettings[{i}].Width")
                };

                var displayIndexBinding = new Binding()
                {
                    Source = Resources["BindingProxy"],
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.TwoWay,
                    Path = new PropertyPath($"Data.DataGridColumnSettings[{i}].DisplayIndex"),
                    FallbackValue = i,
                    TargetNullValue = i
                };

                var visibilityBinding = new Binding()
                {
                    Source = Resources["BindingProxy"],
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.TwoWay,
                    Path = new PropertyPath($"Data.DataGridColumnSettings[{i}].IsDisplay"),
                    Converter = booleanToCollapsedConverter
                };

                BindingOperations.SetBinding(dataGrid.Columns[i], DataGridColumn.WidthProperty, widthBinding);
                BindingOperations.SetBinding(dataGrid.Columns[i], DataGridColumn.DisplayIndexProperty,
                    displayIndexBinding);
                BindingOperations.SetBinding(dataGrid.Columns[i], DataGridColumn.VisibilityProperty, visibilityBinding);
            }
        }
    }
}
