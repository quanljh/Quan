using Quan.Word.Core;
using System.Windows;

namespace Quan.Views
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
    }
}
