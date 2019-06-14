namespace SecondMonitor.Contracts.Commands
{
    using System.Windows.Input;

    public interface IRelayCommandWithParameter<in T> : ICommand
    {
        void Execute(T parameter);
    }
}