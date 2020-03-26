using AutoMapper;
using CommonServiceLocator;
using Prism.Events;
using Prism.Mvvm;
using Quan.Mapper;
using Quan.Views;
using Quan.Word.Core;
using Reactive.Bindings;
using System.Reactive.Concurrency;
using System.Windows;
using Unity;
using Unity.ServiceLocation;

namespace Quan
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer Container { get; } = new UnityContainer();

        private EventAggregator EventAggregator { get; } = new EventAggregator();
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            // Initialize Unity Container
            InitializeContainer();

            // Setup the main application
            ApplicationSetup();

            // Log it 
            IoC.Logger.Log("This is Debug",LogLevel.Debug);
            IoC.Logger.Log("This is Verbose", LogLevel.Verbose);
            IoC.Logger.Log("This is Informative", LogLevel.Infomative);
            IoC.Logger.Log("This is Warning", LogLevel.Warning);
            IoC.Logger.Log("This is Error", LogLevel.Error);
            IoC.Logger.Log("This is Success", LogLevel.Success);


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


        private void ApplicationSetup()
        {
            //Setup IoC
            IoC.SetUp();

            // Bind a UI Manager
            IoC.Kernel.Bind<IUImanager>().ToConstant(new UIManager());

            // Bind a logger
            IoC.Kernel.Bind<ILogFactory>().ToConstant(new BaseLogFactory());

            //Current.MainWindow = new MainWindow();
            //Current.MainWindow.Show();
        }

    }
}
