using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Quan.Word.Core
{
    /// <summary>
    /// A view model for the overview chat list
    /// </summary>
    public class ChatListModel : BindableBase
    {
        /// <summary>
        /// The chat list items for the list 
        /// </summary>
        public ObservableCollection<ChatListItemModel> Items { get; set; }
    }
}
