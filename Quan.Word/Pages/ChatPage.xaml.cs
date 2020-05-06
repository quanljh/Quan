using Quan.Word.Core;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Quan.Word
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : BasePage<ChatMessageListViewModel>
    {
        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ChatPage()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for this page</param>
        public ChatPage(ChatMessageListViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Fired when the view model changes
        /// </summary>
        protected override void OnViewModelChanged()
        {
            //Make sure UI exist first
            if (ChatMessageList == null)
                return;

            //Fade in chat message list
            base.OnViewModelChanged();
            var storyboard = new Storyboard();
            storyboard.AddFadeIn(1);
            storyboard.Begin(ChatMessageList);

            //Make the message box focused
            MessageText.Focus();
        }

        #endregion

        /// <summary>
        /// Preview the input into message box and respond as required
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageText_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Get the textbox
            if (!(sender is TextBox textBox))
                return;

            // Check if we have pressed enter
            if (e.Key == Key.Enter)
            {
                // If we are have control or shift pressed...
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) || Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                {
                    // Add a new line at the point where the cursor is
                    var index = textBox.CaretIndex;

                    // Insert the new line
                    textBox.Text = textBox.Text.Insert(index, Environment.NewLine);

                    // Shift the caret forward to the newline
                    textBox.CaretIndex = index + Environment.NewLine.Length;

                }
                else
                    // Send the message
                    ViewModel.Send();

                // Mark this key as handled by us
                e.Handled = true;

            }
        }
    }
}