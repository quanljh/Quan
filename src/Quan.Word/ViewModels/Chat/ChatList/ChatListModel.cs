using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Quan.Word
{
    /// <summary>
    /// A view model for the overview Chat list
    /// </summary>
    public class ChatListModel : BindableBase
    {
        /// <summary>
        /// The Chat list items for the list 
        /// </summary>
        public ObservableCollection<ChatListItemViewModel> Items { get; set; }
    }
}
