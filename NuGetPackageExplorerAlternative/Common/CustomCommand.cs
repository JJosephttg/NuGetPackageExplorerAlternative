using System;
using System.Windows.Input;

namespace NuGetPackageExplorerAlternative {
    public class CustomCommand : ICommand {
        Func<object, bool> _canExecute;
        Action<object> _action;

        public CustomCommand(Action<object> action) { InitializeCommand((p) => true, action); }
        public CustomCommand(Func<object, bool> canExecute, Action<object> action) { InitializeCommand(canExecute, action); }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) {
            return _canExecute.Invoke(parameter);
        }

        public void Execute(object parameter) {
            _action.Invoke(parameter);
        }

        private void InitializeCommand(Func<object, bool> canExecute, Action<object> action) {
            _canExecute = canExecute;
            _action = action;
        }
    }
}
