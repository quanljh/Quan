namespace Quan.Word.Core
{
    /// <summary>
    /// The application state as a view model
    /// </summary>
    public class ApplicationViewModel : ViewModelBase
    {
        /// <summary>
        /// The current page of the application
        /// </summary>
        public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.login;

        public bool SideMenuVisible { get; set; }

        /// <summary>
        /// Navigates to the specified page
        /// </summary>
        /// <param name="chat">The page to go to</param>
        public void GoToPage(ApplicationPage page)
        {
            //Set the current page
            CurrentPage = page;

            //Show Side menu or not
            SideMenuVisible = page == ApplicationPage.chat;

        }
    }
}
