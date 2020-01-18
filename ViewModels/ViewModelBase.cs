using CommonServiceLocator;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using Unity;

namespace Quan.ViewModels
{
    public abstract class ViewModelBase : BindableBase
    {
        public DelegateCommand FinishInteractionCommand { get; set; }


        public IEventAggregator EventAggregator { get; }

        public IUnityContainer Container { get; }


        #region Action

        public Action FinishInteraction { get; set; }

        #endregion

        protected ViewModelBase()
        {
            Container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            EventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            FinishInteractionCommand = new DelegateCommand(() => { FinishInteraction?.Invoke(); });
        }
    }
}
