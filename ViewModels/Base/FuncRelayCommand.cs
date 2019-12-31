using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EPJ.ViewModels.Base
{
    public class FuncRelayCommand<TParameter> : ICommand
    {

        #region Private Properties

        private Predicate<TParameter> canExecute;
        private Action<TParameter> execute;

        #endregion // Private Properties

        #region Public Constructors

        public FuncRelayCommand(Action<TParameter> execute) : this(execute, null)
        {
            this.execute = execute;
        }

        public FuncRelayCommand(Action<TParameter> execute, Predicate<TParameter> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        #endregion // Public Constructors

        #region Implementation

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (this.canExecute == null) return true;
            return this.canExecute((TParameter)parameter);
        }

        public void Execute(object parameter)
        {
            this.execute((TParameter)parameter);
        }

        #endregion // Implementation
    }

    public class RelayCommand : ICommand
    {
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
