using CommonServiceLocator;
using Prism.Events;
using Prism.Mvvm;
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
            InitializeContainer();

            InitializeIoCContainer();

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
            //IoC 解耦
            var provider = new UnityServiceLocator(Container);
            ServiceLocator.SetLocatorProvider(() => provider);
            ViewModelLocationProvider.SetDefaultViewModelFactory(x => Container.Resolve(x));
        }


        private void InitializeIoCContainer()
        {
            //Setup IoC
            IoC.SetUp();

            //Current.MainWindow = new MainWindow();
            //Current.MainWindow.Show();
        }

    }
}
