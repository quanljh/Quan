using Quan.Word.Core;
using Quan.Word.Relational;
using System.Threading.Tasks;
using System.Windows;
using static Quan.FrameworkDI;
using static Quan.Word.DI;

namespace Quan.Word
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Custom startup so we load our IoC immediately before anything else
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            // Let the base application do what it needs
            base.OnStartup(e);

            // Setup the main application
            await ApplicationSetupAsync();

            // Log it 
            Logger.LogDebugSource("Application starting...");

            // Setup the application view model based on if we are logged in
            ApplicationVM.GoToPage(
                // If we are logged in...
                ClientDataStore.HasCredentials() ?
                    // Go to chat page
                    ApplicationPage.Chat :
                    // Otherwise, go to login page
                    ApplicationPage.Login);

            var window = new MainWindow();

            if (window.DataContext is ViewModelBase vb)
            {
                vb.FinishInteraction = () =>
                {
                    window.Close();
                };
            }
            window.Show();
        }

        private async Task ApplicationSetupAsync()
        {
            // Setup the Quan Framework
            Framework.Construct<DefaultFrameworkConstruction>()
                .AddFileLogger("QuanLog.txt")
                .AddClientDataStore()
                .AddQuanWordViewModels()
                .AddQuanWordClientServices()
                .Build();

            // Ensure the client data store
            await ClientDataStore.EnsureDataStoreAsync();

            // Load new settings
            await SettingsVM.LoadAsync();

        }
    }
}
