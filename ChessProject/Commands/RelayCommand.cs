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

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
