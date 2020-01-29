using CommonServiceLocator;
using Prism.Events;
using Prism.Mvvm;
using Quan.ViewModels;
using Quan.Views;
using Reactive.Bindings;
using System.Reactive.Concurrency;
using System.Windows;
using Quan.ViewModels.Base;
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

            var window = Container.Resolve<BrowserView>();
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
    }
}
