
using Quan.Word.Core;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Quan.Word
{
    /// <summary>
    /// The base class for any content that is being used inside of a <see cref="DialogWindow"/>
    /// </summary>
    public class BaseDialogUserControl : UserControl
    {
        #region Private Members

        /// <summary>
        /// The dialog window we will be contained within
        /// </summary>
        private DialogWindow mDialogWindow;

        #endregion

        #region Public Properties

        /// <summary>
        /// The minimum width of this dialog
        /// </summary>
        public int WindowMinimumWidth { get; set; } = 250;

        /// <summary>
        /// The minimum height of this dialog
        /// </summary>
        public int WindowMinimumHeight { get; set; } = 100;

        /// <summary>
        /// The height of title bar
        /// </summary>
        public int TitleHeight { get; set; } = 30;

        /// <summary>
        /// The Title for this dialog
        /// </summary>
        public string Title { get; set; } = "Default Title";

        #endregion

        #region Public Commands

        public ICommand OKCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BaseDialogUserControl()
        {
            //Create a new dialog window
            mDialogWindow = new DialogWindow();
            mDialogWindow.ViewModel = new DialogWindowViewModel(mDialogWindow);

            //Create a new Command
            OKCommand = new RelayCommand(() => mDialogWindow.Close());
        }

        #endregion

        #region Public Dialog Show Methods

        /// <summary>
        /// Displays a single message box to the user
        /// </summary>
        /// <param name="viewModel">The view model</param>
        /// <typeparam name="T">The view model type for this control</typeparam>
        /// <returns></returns>
        public Task ShowDialog<T>(T viewModel)
            where T : BaseDialogViewModel
        {
            // Create a task to await the dialog closing
            var tcs = new TaskCompletionSource<bool>();

            //Run on UI thread
            Application.Current.Dispatcher?.Invoke(() =>
            {
                try
                {
                    //Match controls expected sizes to the dialog window view model
                    mDialogWindow.ViewModel.WindowMinimumHeight = WindowMinimumHeight;
                    mDialogWindow.ViewModel.WindowMinimumWidth = WindowMinimumWidth;
                    mDialogWindow.ViewModel.TitleHeight = TitleHeight;
                    mDialogWindow.ViewModel.Title = string.IsNullOrEmpty(viewModel.Title) ? Title : viewModel.Title;

                    //Set this control to the dialog window content
                    mDialogWindow.ViewModel.Content = this;

                    //Setup this controls data context binding to the view model
                    DataContext = viewModel;

                    //Show in the center of the parent
                    mDialogWindow.Owner = Application.Current.MainWindow;
                    mDialogWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                    //Show dialog
                    mDialogWindow.ShowDialog();
                }
                finally
                {
                    //Let caller know we finished
                    tcs.SetResult(true);
                }

            });

            return tcs.Task;
        }

        #endregion
    }
}
