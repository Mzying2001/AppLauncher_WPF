using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace AppLauncher.Commands
{
    public class DelegateCommand : DelegateCommand<object> { }
    public class DelegateCommand<ParamType> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute == null || CanExecute((ParamType)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute?.Invoke((ParamType)parameter);
        }

        public Func<ParamType, bool> CanExecute { get; set; }

        public Action<ParamType> Execute { get; set; }
    }
}
