using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Quan.Word.Core
{
    /// <summary>
    /// A view model for a chat message thread list
    /// </summary>
    public class ChatMessageListModel : BindableBase
    {
        /// <summary>
        /// The chat thread items for the list 
        /// </summary>
        public ObservableCollection<ChatMessageListItemModel> Items { get; set; }
    }
}
