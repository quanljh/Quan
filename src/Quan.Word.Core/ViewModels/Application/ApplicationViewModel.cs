﻿using System.Threading.Tasks;

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
        public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.Chat;

        /// <summary>
        /// True if the side menu should be shown
        /// </summary>
        public bool SideMenuVisible { get; set; } = true;

        /// <summary>
        /// True if the settings menu should be shown
        /// </summary>
        public bool SettingsMenuVisible { get; set; }


        /// <summary>
        /// The view model to use for the current page when the CurrentPage changes
        /// NOTE: This is not a live up-to-date view model of the current page
        ///       it is simply used to set the view model of the current page 
        ///       at the time it changes
        /// </summary>
        public ViewModelBase CurrentPageViewModel { get; set; }

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
        public async Task HandleSuccessfulLoginAsync(LoginResultApiModel loginResult)
        {
            // Store this in the client data store
            await IoC.ClientDataStore.SaveLoginCredentialsAsync(Mapper.Map<LoginCredentialsDataModel>(loginResult));

            // Load new settings
            await IoC.Settings.LoadAsync();

            // Go to chat page
            IoC.Application.GoToPage(ApplicationPage.Chat);
        }
    }
}
