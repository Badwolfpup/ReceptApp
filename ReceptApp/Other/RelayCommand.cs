using System.Windows.Input;

namespace ReceptApp
{
    public class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private Action<object> _execute { get; set; }
        private Action<object, object> _execute1 { get; set; }
        private Action<object, object, object> _execute2 { get; set; }

        //private Predicate<object> _CanExecute { get; set; }

        public RelayCommand(Action<object> executeMethod)
        {
            _execute = executeMethod ?? throw new ArgumentNullException(nameof(executeMethod));
            _execute1 = null; // Ensure only one is set
            _execute2 = null; // Ensure only one is set
        }

        // Overload 2: Two parameters
        public RelayCommand(Action<object, object> executeMethod)
        {
            _execute1 = executeMethod ?? throw new ArgumentNullException(nameof(executeMethod));
            _execute = null; // Ensure only one is set
            _execute2 = null; // Ensure only one is set
        }

        // Overload 2: Array with parameters
        public RelayCommand(Action<object, object, object> executeMethod)
        {
            _execute2 = executeMethod ?? throw new ArgumentNullException(nameof(executeMethod));
            _execute = null; // Ensure only one is set
            _execute1 = null; // Ensure only one is set
        }

        //public event EventHandler CanExecuteChanged
        //{
        //    add { CommandManager.RequerySuggested += value; }
        //    remove { CommandManager.RequerySuggested -= value; }
        //}

        public bool CanExecute(object parameter)
        {
            return true; // Add _canExecute logic here if needed
        }

        public void Execute(object parameter)
        {
            if (_execute != null)
            {
                // Single-parameter case
                _execute(parameter);
            }
            else if (_execute1 != null)
            {
                // Two-parameter case
                if (parameter is Tuple<object, object> tuple)
                {
                    _execute1(tuple.Item1, tuple.Item2);
                }
                else if (parameter is object[] array)
                {
                    _execute1(array[0], array[1]);
                }
                else
                {
                    throw new ArgumentException("Parameter must be a Tuple<object, object> or object[] with two elements for the two-parameter overload.");
                }
            }
            else if (_execute2 != null)
            {
                // Array case
                if (parameter is Tuple<object, object, object> tuple1)
                {
                    _execute2(tuple1.Item1, tuple1.Item2, tuple1.Item3);
                }
                else
                {
                    throw new ArgumentException("Parameter must be an object[] for the array overload.");
                }
            }
            else
            {
                throw new InvalidOperationException("No execute method defined.");
            }
        }
    }
}
