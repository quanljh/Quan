
using Quan.Word.Core;
using System.Threading.Tasks;
using System.Windows;

namespace Quan.Word
{
    /// <summary>
    /// The applications implementation of the <see cref="IUImanager"/>
    /// </summary>
    public class UIManager : IUImanager
    {
        /// <summary>
        /// Displays a single message box to the user
        /// </summary>
        /// <param name="viewModel">The view model</param>
        /// <returns></returns>
        public Task ShowMessage(MessageBoxDialogViewModel viewModel)
        {
            return new DialogMessageBox().ShowDialog(viewModel);
        }
    }
}
