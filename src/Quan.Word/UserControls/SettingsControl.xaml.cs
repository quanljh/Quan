using System.Windows.Controls;
using static Quan.Word.DI;

namespace Quan.Word
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();

            // Set data context to settings view model
            DataContext = SettingsVM;
        }
    }
}
