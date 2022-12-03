using System;
using System.Windows.Input;

namespace UnitePlugin.Utility
{
    [Serializable]
    public class RelayCommand<T> : ICommand
    {
        [field: NonSerialized]
        private readonly Action<T> _execute;
        [field: NonSerialized]
        private readonly Predicate<T> _canExecute;
        [field: NonSerialized]
        private EventHandler _canExecuteChanged;

        public event EventHandler CanExecuteChanged
        {
            add => _canExecuteChanged += value;
            // ReSharper disable once DelegateSubtraction
            remove => _canExecuteChanged -= value;
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        
        public void RaiseCanExecuteChanged()
        {
            _canExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
