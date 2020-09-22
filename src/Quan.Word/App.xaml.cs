using AutoMapper;
using CommonServiceLocator;
using Prism.Events;
using Prism.Mvvm;
using Quan.Word.Core;
using Quan.Word.Relational;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Unity;
using Unity.ServiceLocation;
using static Quan.FrameworkDI;
using static Quan.Word.DI;

namespace Quan.Word
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer Container { get; } = new UnityContainer();

        private EventAggregator EventAggregator { get; } = new EventAggregator();

        /// <summary>
        /// Custom startup so we load our IoC immediately before anything else
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            // Let the base application do what it needs
            base.OnStartup(e);

            // Initialize Unity Container
            InitializeContainer();

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

            var window = Container.Resolve<MainWindow>();
            if (window.DataContext is ViewModelBase vb)
            {
                vb.FinishInteraction = () =>
                {
                    window.Close();
                };
            }
            window.Show();
        }

        private void InitializeContainer()
        {
            Container.RegisterInstance(typeof(IUnityContainer), Container);
            Container.RegisterInstance(typeof(IEventAggregator), EventAggregator);

            // Initialize a mapper configuration
            var config = new MapperConfiguration(cfg => cfg.AddProfile<QuanMapperProfile>());
            // Create a mapper
            var mapper = config.CreateMapper();
            // Register the mapper instance
            Container.RegisterInstance(typeof(IMapper), mapper);

            //Create a UnityServiceLocator which inherits from ServiceLocatorImplBase implement the interface IServiceLocator
            var provider = new UnityServiceLocator(Container);
            //IoC 解耦(To lose the coupling between code and the IoC container)
            ServiceLocator.SetLocatorProvider(() => provider);

            //Override Activator.CreateInstance(type) to IUnityContainer and use it to resolve our view models
            ViewModelLocationProvider.SetDefaultViewModelFactory(x => Container.Resolve(x));
        }


        private async Task ApplicationSetupAsync()
        {
            // Setup the Quan Framework
            Framework.Construct<DefaultFrameworkConstruction>()
                .AddFileLogger()
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
