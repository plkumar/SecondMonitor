namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.SettingsWindow
{
    using System.Threading.Tasks;
    using Contracts.Commands;
    using SecondMonitor.ViewModels.Factory;
    using Settings;
    using TelemetryApplication.Settings.DTO;
    using ViewModels;
    using ViewModels.SettingsWindow;

    public class SettingsWindowController : ISettingsWindowController
    {
        private readonly ISettingsController _settingsController;
        private readonly ISettingsWindowViewModel _settingsWindowViewModel;

        public SettingsWindowController(IMainWindowViewModel mainWindowViewModel, IViewModelFactory viewModelFactory, ISettingsController settingsController)
        {
            _settingsController = settingsController;
            _settingsWindowViewModel = viewModelFactory.Create<ISettingsWindowViewModel>();
            mainWindowViewModel.LapSelectionViewModel.SettingsWindowViewModel = _settingsWindowViewModel;
            BindCommands();
        }

        public Task StartControllerAsync()
        {
            return Task.CompletedTask;
        }

        public Task StopControllerAsync()
        {
            return Task.CompletedTask;
        }

        private void OpenWindow()
        {
            TelemetrySettingsDto telemetrySettingsDto = _settingsController.TelemetrySettings;
            _settingsWindowViewModel.FromModel(telemetrySettingsDto);
            _settingsWindowViewModel.IsWindowOpened = true;
        }

        private void SaveAndClose()
        {
            TelemetrySettingsDto newTelemetrySettingsDto = _settingsWindowViewModel.SaveToNewModel();
            _settingsController.SetTelemetrySettings(newTelemetrySettingsDto, RequestedAction.RefreshCharts);
            CloseWindow();
        }

        private void CloseWindow()
        {
            _settingsWindowViewModel.IsWindowOpened = false;
        }

        private void BindCommands()
        {
            _settingsWindowViewModel.OpenWindowCommand = new RelayCommand(OpenWindow);
            _settingsWindowViewModel.CancelCommand = new RelayCommand(CloseWindow);
            _settingsWindowViewModel.OkCommand = new RelayCommand(SaveAndClose);
        }
    }
}