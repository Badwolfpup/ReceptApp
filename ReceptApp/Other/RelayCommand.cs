using System;
using System.Windows.Input;

namespace ReceptApp
{
    public class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private Action<object> _Execute { get; set; }

        //private Predicate<object> _CanExecute { get; set; }

        public RelayCommand(Action<object> executeMethod)
        {
            _Execute = executeMethod;

            //_CanExecute = canExecuteMethod;

        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _Execute(parameter);
        }
    }
}
