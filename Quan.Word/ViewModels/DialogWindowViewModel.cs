using System.Windows;
using System.Windows.Controls;

namespace Quan.Word.Core
{
    public class DialogWindowViewModel : MainWindowViewModel
    {
        #region Public Properties

        /// <summary>
        /// The title of this dialog window
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The content to host inside the dialog
        /// </summary>
        public Control Content { get; set; }

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



