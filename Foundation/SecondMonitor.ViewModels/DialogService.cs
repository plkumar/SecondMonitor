namespace SecondMonitor.ViewModels
{
    using System.Windows;
    using Dialogs;
    using Factory;

    public class DialogService : IDialogService
    {
        private readonly IWindowService _windowService;
        private readonly IViewModelFactory _viewModelFactory;

        public DialogService(IWindowService windowService, IViewModelFactory viewModelFactory)
        {
            _windowService = windowService;
            _viewModelFactory = viewModelFactory;
        }

        public bool ShowYesNoDialog(string title, string text)
        {
            var yesNoViewModel = _viewModelFactory.Create<YesNoDialogViewModel>();
            yesNoViewModel.Text = text;
            yesNoViewModel.Title = title;

            _windowService.OpenDialog(yesNoViewModel, title, WindowState.Normal, SizeToContent.WidthAndHeight, WindowStartupLocation.CenterOwner);
            return yesNoViewModel.DialogResult == true;
        }
    }
}