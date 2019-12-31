using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EPJ
{
    
    public class RelayCommand : ICommand
    {
        #region Private variables
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        #endregion

        #region Public Constructors

        public RelayCommand(Action<object> execute) : this(execute, null) => _execute = execute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand members
        public bool CanExecute(object parameter)
        {
            return (_canExecute == null || _canExecute(parameter));
        }

        public void Execute(object parameter) => _execute(parameter);


        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion
    }
}
