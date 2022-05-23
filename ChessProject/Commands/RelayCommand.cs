using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ChessProject.Commands
{
    public class RelayCommand<T> : ICommand
    {
        public Action<T> _execute;
        public Predicate<T> _canExecute;

        public RelayCommand(Action<T> execute, bool v) : this(execute, null) { }
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null) throw new ArgumentNullException();
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }

        public void Execute(object parameter) => _execute((T)parameter);



    }
}
