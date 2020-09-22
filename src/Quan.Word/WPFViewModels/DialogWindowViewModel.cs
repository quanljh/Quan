using System.Windows;
using System.Windows.Controls;

namespace Quan.Word
{
    public class DialogWindowViewModel : MainWindowViewModel
    {
        #region Public Properties

        /// <summary>
        /// The title of this dialog window
        /// </summary>
        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// The content to host inside the dialog
        /// </summary>
        private Control _content;

        public Control Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DialogWindowViewModel(Window window) : base(window)
        {
            //Make minimum size smaller
            WindowMinimumHeight = 100;
            WindowMinimumWidth = 250;

            //Make title bar smaller
            TitleHeight = 30;
        }

        #endregion
    }
}



