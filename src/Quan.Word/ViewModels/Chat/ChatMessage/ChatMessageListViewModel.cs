using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Quan.Word
{
    /// <summary>
    /// A view model for a Chat message thread list
    /// </summary>
    public class ChatMessageListViewModel : ViewModelBase
    {
        #region Protected Members

        /// <summary>
        /// The last searched text in this list
        /// </summary>
        protected string mLastSearchText;

        #endregion

        #region Public Properties

        /// <summary>
        /// The title of this chat list
        /// </summary>
        private string _displayTitle;

        public string DisplayTitle
        {
            get => _displayTitle;
            set => SetProperty(ref _displayTitle, value);
        }

        /// <summary>
        /// The Chat thread items for the list 
        /// </summary>
        private ObservableCollection<ChatMessageListItemModel> _items;

        public ObservableCollection<ChatMessageListItemModel> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        /// <summary>
        /// True to show the attachment menu, false to hide it
        /// </summary>
        private bool _attachmentMenuVisible;

        public bool AttachmentMenuVisible
        {
            get => _attachmentMenuVisible;
            set
            {
                if (SetProperty(ref _attachmentMenuVisible, value))
                    RaisePropertyChanged(nameof(AnyPopupVisible));
            }
        }

        /// <summary>
        /// True if any popup menus are visible
        /// </summary>
        public bool AnyPopupVisible => AttachmentMenuVisible;

        /// <summary>
        /// The view model for the attachment menu
        /// </summary>
        private ChatAttachmentPopupMenuViewModel _attachmentMenu;

        public ChatAttachmentPopupMenuViewModel AttachmentMenu
        {
            get => _attachmentMenu;
            set => SetProperty(ref _attachmentMenu, value);
        }

        /// <summary>
        /// The text for the current message being written
        /// </summary>
        private string _pendingMessageText;

        public string PendingMessageText
        {
            get => _pendingMessageText;
            set => SetProperty(ref _pendingMessageText, value);
        }

        /// <summary>
        /// The text to search for when we do a search
        /// </summary>
        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                // Check value is different
                if (_searchText == value)
                    return;

                // Update value
                SetProperty(ref _searchText, value);

                // If the search text is empty...
                if (string.IsNullOrEmpty(SearchText))
                    // Search to restore messages
                    Search();
            }
        }

        /// <summary>
        /// A flag indicating if the search dialog is open
        /// </summary>
        private bool _searchIsOpen;

        public bool SearchIsOpen
        {
            get => _searchIsOpen;
            set
            {
                // Check value has changed
                if (_searchIsOpen == value)
                    return;

                // Update value
                SetProperty(ref _searchIsOpen, value);

                // if dialog closes...
                if (!_searchIsOpen)
                    // Clear search text
                    SearchText = string.Empty;
            }
        }

        #endregion

        #region Public Commands

        /// <summary>
        /// The command for when the attachment button is clicked
        /// </summary>
        public ICommand AttachButtonCommand { get; set; }

        /// <summary>
        /// The command for when the area outside of any popup is clicked
        /// </summary>
        public ICommand PopupClickawayCommand { get; set; }

        /// <summary>
        /// The command for when user clicks the send button
        /// </summary>
        public ICommand SendCommand { get; set; }

        /// <summary>
        /// The command for when user wants to search
        /// </summary>
        public ICommand SearchCommand { get; set; }

        /// <summary>
        /// The command for when user wants to open the search dialog
        /// </summary>
        public ICommand OpenSearchCommand { get; set; }

        /// <summary>
        /// The command for when user wants to close the search dialog
        /// </summary>
        public ICommand CloseSearchCommand { get; set; }

        /// <summary>
        /// The command for when user wants to clear the search text
        /// </summary>
        public ICommand ClearSearchCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChatMessageListViewModel()
        {
            // Create commands
            AttachButtonCommand = new RelayCommand(AttachmentButton);
            PopupClickawayCommand = new RelayCommand(PopupClickaway);
            SendCommand = new RelayCommand(Send);
            SearchCommand = new RelayCommand(Search);
            OpenSearchCommand = new RelayCommand(OpenSearch);
            CloseSearchCommand = new RelayCommand(CloseSearch);
            ClearSearchCommand = new RelayCommand(ClearSearch);

            // Make a default menu
            AttachmentMenu = new ChatAttachmentPopupMenuViewModel();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// When the attachment button is clicked, show/hide the attachment popup
        /// </summary>
        public void AttachmentButton()
        {
            //Toggle menu visibility
            AttachmentMenuVisible ^= true;

        }

        /// <summary>
        /// When the popup clickaway area is clicked, hide any popups
        /// </summary>
        public void PopupClickaway()
        {
            //Hide attachment menu
            AttachmentMenuVisible = false;
        }

        /// <summary>
        /// When the user clicks the send button,send the message
        /// </summary>
        public void Send()
        {
            if (Items == null)
                Items = new ObservableCollection<ChatMessageListItemModel>();

            // Fake send a new message
            Items.Add(new ChatMessageListItemModel
            {
                Initials = "Quan",
                Message = PendingMessageText,
                MessageSentTime = DateTime.UtcNow,
                SentByMe = true,
                SenderName = "quanljh",
                NewItem = true
            });



            // Clear the pending message text
            PendingMessageText = string.Empty;
        }

        /// <summary>
        /// Searches the current message list and scroll the view
        /// </summary>
        public void Search()
        {
            // Make sure we don't research the same text
            if ((string.IsNullOrEmpty(mLastSearchText) && string.IsNullOrEmpty(SearchText)) ||
                string.Equals(mLastSearchText, SearchText))
                return;

            // If we have no search text, or no items
            if (string.IsNullOrEmpty(SearchText) || Items == null || Items.Count <= 0)
            {
                // Set Last search
                mLastSearchText = SearchText;
                return;
            }


        }

        /// <summary>
        /// Clears the search text
        /// </summary>
        public void ClearSearch()
        {
            // If there is some search text...
            if (!string.IsNullOrEmpty(SearchText))
            {
                // Clear the text
                SearchText = string.Empty;
            }
            // Otherwise
            else
                // Close search dialog
                SearchIsOpen = false;


        }

        /// <summary>
        /// Opens the search dialog
        /// </summary>
        public void OpenSearch() => SearchIsOpen ^= true;

        /// <summary>
        /// Closes the search dialog
        /// </summary>
        public void CloseSearch() => SearchIsOpen = false;
        #endregion
    }
}
