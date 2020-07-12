using AutoMapper;
using CommonServiceLocator;
using Prism.Events;
using Prism.Mvvm;
using Quan.Word.Core;
using Quan.Word.Relational;
using Reactive.Bindings;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.ServiceLocation;

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
            IoC.Logger.Log("This is Debug", LogLevel.Debug);

            // Setup the application view model based on if we are logged in
            IoC.Application.GoToPage(
                // If we are logged in...
                IoC.ClientDataStore.HasCredentials() ?
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
            //Because of The ReactiveProperty base on one thread
            ReactivePropertyScheduler.SetDefault(ImmediateScheduler.Instance);
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
            // Setup the Dna Framework
            new DefaultFrameworkConstruction()
                .UseFileLogger("QuanLog.txt")
                .UseClientDataStore()
                .Build();

            //Setup IoC
            IoC.SetUp();

            // Bind a logger
            IoC.Kernel.Bind<ILogFactory>().ToConstant(new BaseLogFactory(new ILogger[]
            {
                // TODO: Add ApplicationSettings so we can set/edit a log location
                //       For now just log to the path where this application is running
                new Core.FileLogger("Oldlog.txt"),
            }));

            // Add our task manager
            IoC.Kernel.Bind<ITaskManager>().ToConstant(new TaskManager());

            // Bind a file manager
            IoC.Kernel.Bind<IFileManager>().ToConstant(new FileManager());

            // Bind a UI Manager
            IoC.Kernel.Bind<IUImanager>().ToConstant(new UIManager());

            // Ensure the client data store
            await IoC.ClientDataStore.EnsureDataStoreAsync();

            // Load new settings
            await IoC.Settings.LoadAsync();

        }

        public class SettingsDataModel
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string Value { get; set; }
        }
    }
}
