﻿using Quan.Word.Core;
using System.Windows;

namespace Quan.Views
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        #region Private Members

        /// <summary>
        /// The view model for this window
        /// </summary>
        private DialogWindowViewModel mViewModel;

        #endregion

        #region Public properties

        /// <summary>
        /// The view model for this window
        /// </summary>
        public DialogWindowViewModel ViewModel
        {
            get => mViewModel;
            set
            {
                //Set new value
                mViewModel = value;

                // Update DataContext
                DataContext = mViewModel;
            }
        }

        #endregion
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DialogWindow()
        {
            InitializeComponent();
        }
        #endregion
    }
}