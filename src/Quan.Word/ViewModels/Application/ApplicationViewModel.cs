using Quan.Word.Core;
using System.Threading.Tasks;
using static Quan.Word.DI;

namespace Quan.Word
{
    /// <summary>
    /// The application state as a view model
    /// </summary>
    public class ApplicationViewModel : ViewModelBase
    {
        /// <summary>
        /// The current page of the application
        /// </summary>
        private ApplicationPage _currentPage = ApplicationPage.Login;

        public ApplicationPage CurrentPage
        {
            get => _currentPage;
            private set => SetProperty(ref _currentPage, value);
        }

        /// <summary>
        /// True if the side menu should be shown
        /// </summary>
        private bool _sideMenuVisible;

        public bool SideMenuVisible
        {
            get => _sideMenuVisible;
            set => SetProperty(ref _sideMenuVisible, value);
        }

        /// <summary>
        /// True if the settings menu should be shown
        /// </summary>
        private bool _settingsMenuVisible;

        public bool SettingsMenuVisible
        {
            get => _settingsMenuVisible;
            set => SetProperty(ref _settingsMenuVisible, value);
        }

        /// <summary>
        /// The view model to use for the current page when the CurrentPage changes
        /// NOTE: This is not a live up-to-date view model of the current page
        ///       it is simply used to set the view model of the current page 
        ///       at the time it changes
        /// </summary>
        private ViewModelBase _currentPageViewModel;

        public ViewModelBase CurrentPageViewModel
        {
            get => _currentPageViewModel;
            set => SetProperty(ref _currentPageViewModel, value);
        }

        /// <summary>
        /// Navigates to the specified page
        /// </summary>
        /// <param name="page">The page to go to</param>
        /// <param name="viewModel">The view model, if any, to set explicitly to the new page</param>
        public void GoToPage(ApplicationPage page, ViewModelBase viewModel = null)
        {
            // Always hide settings page if we are changing pages
            SettingsMenuVisible = false;

            // Set the view model
            CurrentPageViewModel = viewModel;

            //Set the current page
            CurrentPage = page;

            // Fire off a CurrentPage changed event
            RaisePropertyChanged(nameof(CurrentPage));


            //Show Side menu or not
            SideMenuVisible = page == ApplicationPage.Chat;

        }

        /// <summary>
        /// Handles what happens when we have successfully logged in
        /// </summary>
        /// <param name="loginResult">The results from the successful login</param>
        public async Task HandleSuccessfulLoginAsync(UserProfileDetailsApiModel loginResult)
        {
            // Store this in the client data store
            await ClientDataStore.SaveLoginCredentialsAsync(loginResult);

            // Load new settings
            await SettingsVM.LoadAsync();

            // Go to chat page
            ApplicationVM.GoToPage(ApplicationPage.Chat);
        }
    }
}
