using System;
using System.Windows.Input;

namespace Quan.Word.Core
{
    public class RelayCommand : ICommand
    {
        #region Private Members

        /// <summary>
        /// The action to run
        /// </summary>
        private Action mAction;

        /// <summary>
        /// The action with parameter to run
        /// </summary>
        private Action<object> mActionWithParameter;

        #endregion

        #region Public Events

        /// <summary>
        /// The event thats fired when the <see cref="CanExecute(object)"/> value has changed
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) =>
        {

        };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public RelayCommand(Action action)
        {
            mAction = action;
        }

        public RelayCommand(Action<object> action)
        {
            mActionWithParameter = action;
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// A relay command can always execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Executes the commands Action
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            if (parameter != null)
                mActionWithParameter(parameter);
            else
                mAction();
        }

        #endregion
    }
}
