namespace SecondMonitor.ViewModels
{
    using System;
    using System.Windows;

    public class WindowService : IWindowService
    {
        public Window OpenWindow(IViewModel viewModel, string title) => OpenWindow(viewModel, title, WindowState.Normal, SizeToContent.WidthAndHeight, WindowStartupLocation.Manual, (Window)null);

        public Window OpenWindow(IViewModel viewModel, string title, WindowState startState, SizeToContent sizeToContent, WindowStartupLocation startupLocation) =>
            OpenWindow(viewModel, title, startState, sizeToContent, startupLocation, Application.Current.MainWindow);




        public Window OpenWindow(IViewModel viewModel, string title, WindowState startState, SizeToContent sizeToContent, WindowStartupLocation startupLocation, Window owner)
        {
             Window window = new Window() {WindowState = startState, Title = title,  Content = viewModel, SizeToContent = sizeToContent };
            window.Closed += WindowOnClosed;
            window.Owner = owner;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Show();
            return window;
        }

        public Window OpenWindow(IViewModel viewModel, string title, WindowState startState, SizeToContent sizeToContent, WindowStartupLocation startupLocation, Action onClose)
        {
            Window window = new Window() { WindowState = startState, Title = title, Content = viewModel, SizeToContent = sizeToContent, WindowStartupLocation = startupLocation};

            if (startupLocation == WindowStartupLocation.CenterOwner && window != Application.Current.MainWindow)
            {
                window.Owner = Application.Current.MainWindow;
            }

            window.Closed += (sender, e) =>
            {
                if (!(sender is Window sWindow))
                {
                    return;
                }
                onClose();
                sWindow.Content = null;

            };
            window.Show();
            return window;
        }

        private void WindowOnClosed(object sender, EventArgs e)
        {
            if (!(sender is Window window))
            {
                return;
            }

            window.Content = null;
            window.Closed -= WindowOnClosed;
        }
    }
}