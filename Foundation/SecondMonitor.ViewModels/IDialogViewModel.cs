namespace SecondMonitor.ViewModels
{
    using System.Windows;
    using System.Windows.Input;

    public interface IDialogViewModel : IViewModel
    {
        ICommand OkCommand { get; }

        ICommand CancelCommand { get; }

        bool? DialogResult { get;}

        void RegisterWindow(Window window);

    }
}