namespace SecondMonitor.Contracts.Commands
{
    using System;

    public class RelayCommandWithParameter<T> : IRelayCommandWithParameter<T>
    {
        private readonly Action<T> _relayAction;

        public RelayCommandWithParameter(Action<T> action)
        {
            _relayAction = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(T parameter)
        {
            _relayAction(parameter);
        }

        public void Execute(object parameter)
        {
            _relayAction(default(T));
        }

        // Interface implementation
#pragma warning disable CS0067
        public event EventHandler CanExecuteChanged;
#pragma warning disable CS0067
    }
}