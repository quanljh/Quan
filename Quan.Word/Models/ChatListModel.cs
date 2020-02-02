using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace Quan.Models
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
