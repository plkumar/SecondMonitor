namespace SecondMonitor.ViewModels
{
    using System;
    using System.Windows;
    using NLog;

    public class WindowService : IWindowService
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Window OpenWindow(IViewModel viewModel, string title) => OpenWindow(viewModel, title, WindowState.Normal, SizeToContent.WidthAndHeight, WindowStartupLocation.Manual, (Window)null);

        public Window OpenWindow(IViewModel viewModel, string title, WindowState startState, SizeToContent sizeToContent, WindowStartupLocation startupLocation)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                return Application.Current.Dispatcher.Invoke(() => OpenWindow(viewModel, title, startState, sizeToContent, startupLocation));
            }
            return OpenWindow(viewModel, title, startState, sizeToContent, startupLocation, Application.Current.MainWindow);
        }

        public bool? OpenDialog(IDialogViewModel viewModel, string title, WindowState startState, SizeToContent sizeToContent, WindowStartupLocation startupLocation) =>
            OpenDialog(viewModel, title, startState, sizeToContent, startupLocation, Application.Current.MainWindow);

        private bool? OpenDialog(IDialogViewModel viewModel, string title, WindowState startState, SizeToContent sizeToContent, WindowStartupLocation startupLocation, Window owner)
        {
            LogWindow(owner);
            Window window = new Window() { WindowState = startState, Title = title, Content = viewModel, SizeToContent = sizeToContent };
            window.Closed += WindowOnClosed;
            window.Owner =  owner;
            window.WindowStartupLocation = startupLocation;
            viewModel.RegisterWindow(window);
            return window.ShowDialog();

        }

        public Window OpenWindow(IViewModel viewModel, string title, WindowState startState, SizeToContent sizeToContent, WindowStartupLocation startupLocation, Window owner)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                return Application.Current.Dispatcher.Invoke(() => OpenWindow(viewModel, title, startState, sizeToContent, startupLocation, owner));
            }
            LogWindow(owner);
            Window window = new Window() {WindowState = startState, Title = title,  Content = viewModel, SizeToContent = sizeToContent };
            window.Closed += WindowOnClosed;
            window.Owner = owner;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Show();
            return window;
        }

        public Window OpenWindow(IViewModel viewModel, string title, WindowState startState, SizeToContent sizeToContent, WindowStartupLocation startupLocation, Action onClose)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                return Application.Current.Dispatcher.Invoke(() => OpenWindow(viewModel, title, startState, sizeToContent, startupLocation, onClose));
            }

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

        private void LogWindow(Window window)
        {
            if (window == null)
            {
                Logger.Info("Parent Window is null");
                return;
            }

            Logger.Info($"Parent Window is of type {window.GetType()}, location {window.Left}, {window.Top}");

        }

    }
}