using AutoMapper;
using CommonServiceLocator;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using PropertyChanged;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unity;

namespace Quan.Word.Core
{
    /// <summary>
    /// A base view model that fires Property Changed events as needed
    /// Either <see cref="AddINotifyPropertyChangedInterfaceAttribute"/> or <see cref="BindableBase"/> is fine to use.
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public abstract class ViewModelBase : BindableBase, INotifyDataErrorInfo
    {
        public DelegateCommand FinishInteractionCommand { get; set; }

        public IEventAggregator EventAggregator { get; }

        public IUnityContainer Container { get; }

        public IMapper Mapper { get; }

        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();


        #region Action

        public Action FinishInteraction { get; set; }

        #endregion

        protected ViewModelBase()
        {
            // Don't set when we are in design-mode
            if (ServiceLocator.IsLocationProviderSet)
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

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ?
                _errorsByPropertyName[propertyName] : null;
        }

        public bool HasErrors => _errorsByPropertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        public void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
    }
}
