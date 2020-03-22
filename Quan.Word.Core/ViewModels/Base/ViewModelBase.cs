using AutoMapper;
using CommonServiceLocator;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using PropertyChanged;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace Quan.Word.Core
{
    /// <summary>
    /// A base view model that fires Property Changed events as needed
    /// Either <see cref="AddINotifyPropertyChangedInterfaceAttribute"/> or <see cref="BindableBase"/> is fine to use.
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public abstract class ViewModelBase : BindableBase
    {
        public DelegateCommand FinishInteractionCommand { get; set; }


        public IEventAggregator EventAggregator { get; }

        public IUnityContainer Container { get; }

        public IMapper Mapper { get; }


        #region Action

        public Action FinishInteraction { get; set; }

        #endregion

        protected ViewModelBase()
        {
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                Container = ServiceLocator.Current.GetInstance<IUnityContainer>();
                EventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
                Mapper = ServiceLocator.Current.GetInstance<IMapper>();
            }
            FinishInteractionCommand = new DelegateCommand(() => { FinishInteraction?.Invoke(); });
        }


        #region Command Helpers

        /// <summary>
        /// Runs a command if the updating flag is not set
        /// If the flag is true (indicating the function is already running)then the action is not run.
        /// If the flag is false (indication no running function)then the action is run.
        /// Once the action is finished if it was run, then the flag is reset to false.
        /// </summary>
        /// <param name="updatingFlag">The boolean property flag defining if the command is already running</param>
        /// <param name="action">The action to run if the command is not already running</param>
        /// <returns></returns>
        protected async Task RunCommand(Expression<Func<bool>> updatingFlag, Func<Task> action)
        {
            //Check if the flag property is true (meaning the function is already running)
            if (updatingFlag.GetPropertyValue())
                return;

            //Set the property flag to true to indicate we are running
            updatingFlag.SetPropertyValue(true);

            try
            {
                //Run the passed in action
                await action();
            }
            finally
            {
                // Set the property flag back to false now it's finished
                updatingFlag.SetPropertyValue(false);
            }
        }

        #endregion
    }
}
