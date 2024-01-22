using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MachineLearningWpfUI.Base
{
    public class Command : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<bool> CanExecuteFunc;
        public event EventHandler CanExecuteChanged;

        public Command(Action execute)
        {
            this.execute = x => execute();
        }
        public Command(Action execute, Func<bool> canExecute)
        {
            this.CanExecuteFunc = canExecute;
            this.execute = x => execute();
        }
        public Command(Action<object> execute)
        {
            this.execute = execute;
        }

        public void Execute(object parameter)
        {
            if (CanExecuteFunc != null)
            {
                if (CanExecuteFunc.Invoke())
                    this.execute(parameter);
            }
            else
                this.execute(parameter);

        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

    }
}
