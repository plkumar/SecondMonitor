namespace SecondMonitor.ViewModels
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using Contracts.Commands;

    public abstract class AbstractDialogViewModel<T> : AbstractViewModel<T>, IDialogViewModel
    {
        private Window _window;

        protected AbstractDialogViewModel()
        {
            DialogResult = null;
            OkCommand = new RelayCommand(OkPressed);
            CancelCommand = new RelayCommand(CancelPressed);
        }

        private void CancelPressed()
        {
            DialogResult = false;
            CloseWindow();

        }

        private void OkPressed()
        {
            DialogResult = true;
            CloseWindow();
        }

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get; }

        public bool? DialogResult { get; private set; }

        public void RegisterWindow(Window window)
        {
            _window = window;
            _window.Closed+= WindowOnClosed;
        }

        public void UnRegisterWindow()
        {
            _window.Closed -= WindowOnClosed;
        }

        private void WindowOnClosed(object sender, EventArgs e)
        {
            UnRegisterWindow();
        }

        private void CloseWindow()
        {
            _window?.Close();
        }
    }
}