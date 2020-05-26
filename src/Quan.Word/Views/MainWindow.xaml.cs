using System;
using System.Windows;
using System.Windows.Interop;

namespace Quan.Word
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(this);
        }

        private void MainWindow_OnDeactivated(object sender, EventArgs e)
        {
            //Show overlay if we lose focus
            if (DataContext is MainWindowViewModel vm)
            {
                vm.DimmableOverlayVisible = true;
            }
        }

        private void MainWindow_OnActivated(object sender, EventArgs e)
        {
            //Hide overlay if we are focused
            if (DataContext is MainWindowViewModel vm)
            {
                vm.DimmableOverlayVisible = false;
            }
        }
    }
}
