namespace SecondMonitor.ViewModels
{
    using System;
    using System.Windows;

    public interface IWindowService
    {
        Window OpenWindow(IViewModel viewModel, string title);

        Window OpenWindow(IViewModel viewModel, string title, WindowState startState, SizeToContent sizeToContent, WindowStartupLocation startupLocation);

        bool? OpenDialog(IDialogViewModel viewModel, string title, WindowState startState, SizeToContent sizeToContent, WindowStartupLocation startupLocation);

        Window OpenWindow(IViewModel viewModel, string title, WindowState startState, SizeToContent sizeToContent, WindowStartupLocation startupLocation, Window owner);

        Window OpenWindow(IViewModel viewModel, string title, WindowState startState, SizeToContent sizeToContent, WindowStartupLocation startupLocation, Action onClose);
    }
}