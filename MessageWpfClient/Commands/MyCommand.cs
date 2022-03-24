using System;
using System.Windows.Input;

namespace MessageWpfClient.Commands
{
    internal class MyCommand : ICommand
    {
        Action<object> _executeAction;
        Func<object, bool> _canExecute;

        public MyCommand(Action<object> executeAction, Func<object, bool> canExecute)
        {
            _canExecute = canExecute;
            _executeAction = executeAction;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            else
            {
                return _canExecute(parameter);
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add
            { CommandManager.RequerySuggested += value; }

            remove
            { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _executeAction(parameter);
        }
    }
}
