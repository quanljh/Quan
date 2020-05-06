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

            //SourceInitialized += OnSourceInitialized;

            DataContext = new MainWindowViewModel(this);
        }

        // Old version to resolve window maximize
        private void OnSourceInitialized(object sender, EventArgs e)
        {
            IntPtr handle = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(handle)?.AddHook(WindowProc);
        }

        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WindowMaximizeHelper.WmGetMinMaxInfo(hwnd, lParam, (int)MinWidth, (int)MinHeight);
                    break;
            }

            return (IntPtr)0;
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
