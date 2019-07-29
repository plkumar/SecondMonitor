namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.MainWindow.LapPicker
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts.Commands;
    using Contracts.UserInput;
    using DataModel.Extensions;
    using OpenWindow;
    using SecondMonitor.ViewModels.Colors;
    using SecondMonitor.ViewModels.Factory;
    using SettingsWindow;
    using Synchronization;
    using TelemetryLoad;
    using TelemetryManagement.DTO;
    using ViewModels;
    using ViewModels.LapPicker;

    public class LapPickerController : ILapPickerController
    {
        private readonly ITelemetryViewsSynchronization _telemetryViewsSynchronization;
        private readonly ITelemetryLoadController _telemetryLoadController;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IColorPaletteProvider _colorPaletteProvider;
        private readonly IOpenWindowController _openWindowController;
        private readonly ISettingsWindowController _settingsWindowController;
        private readonly IUserInputProvider _userInputProvider;
        private readonly ILapSelectionViewModel _lapSelectionViewModel;
        private readonly List<LapSummaryDto> _allAvailableLaps;
        private readonly List<LapSummaryDto> _loadedLaps;

        public LapPickerController(ITelemetryViewsSynchronization telemetryViewsSynchronization, ITelemetryLoadController telemetryLoadController, IMainWindowViewModel mainWindowViewModel, IViewModelFactory viewModelFactory,
             IColorPaletteProvider colorPaletteProvider, IOpenWindowController openWindowController, ISettingsWindowController settingsWindowController, IUserInputProvider userInputProvider)
        {
            _allAvailableLaps = new List<LapSummaryDto>();
            _loadedLaps = new List<LapSummaryDto>();
            _telemetryViewsSynchronization = telemetryViewsSynchronization;
            _telemetryLoadController = telemetryLoadController;
            _lapSelectionViewModel = mainWindowViewModel.LapSelectionViewModel;
            _viewModelFactory = viewModelFactory;
            _colorPaletteProvider = colorPaletteProvider;
            _openWindowController = openWindowController;
            _settingsWindowController = settingsWindowController;
            _userInputProvider = userInputProvider;
        }

        public async Task StartControllerAsync()
        {
            Subscribe();
            _lapSelectionViewModel.AddCustomLapCommand = new AsyncCommand(AddCustomLap);
            _lapSelectionViewModel.LoadAllLapsCommand = new AsyncCommand(LoadAllLaps);
            _lapSelectionViewModel.UnloadAllLapsCommand = new AsyncCommand(UnLoadAllLaps);
            _lapSelectionViewModel.AvailableStints = new[] {"All"}.Concat(Enumerable.Range(0, 20).Select(x => x.ToString())).ToList();
            _lapSelectionViewModel.SelectedStint = "All";
            await StartChildControllersAsync();
        }

        private Task UnLoadAllLaps()
        {
            var lapsToUnloadLoad = _lapSelectionViewModel.LapSummaries.Where(x => x.Selected).Select(x => x.OriginalModel);
            if (int.TryParse(_lapSelectionViewModel.SelectedStint, out int stintToLoad))
            {
                lapsToUnloadLoad = lapsToUnloadLoad.Where(x => x.Stint == stintToLoad);
            }
            return Task.WhenAll(lapsToUnloadLoad.Select(x => _telemetryLoadController.UnloadLap(x)));
        }

        private Task LoadAllLaps()
        {
            var lapsToLoad = _lapSelectionViewModel.LapSummaries.Where(x => !x.Selected).Select(x => x.OriginalModel);
            if (int.TryParse(_lapSelectionViewModel.SelectedStint, out int stintToLoad))
            {
                lapsToLoad = lapsToLoad.Where(x => x.Stint == stintToLoad);
            }
            return Task.WhenAll(lapsToLoad.Select(x => _telemetryLoadController.LoadLap(x)));

        }

        public async Task StopControllerAsync()
        {
            UnSubscribe();
            await StopChildControllersAsync();
        }

        private async Task StartChildControllersAsync()
        {
            await _openWindowController.StartControllerAsync();
            await _settingsWindowController.StartControllerAsync();
        }

        private async Task StopChildControllersAsync()
        {
            IEnumerable<Task> unloadTask = _allAvailableLaps.Select(x => _telemetryLoadController.UnloadLap(x));
            await Task.WhenAll(unloadTask);
            await _openWindowController.StopControllerAsync();
            await _settingsWindowController.StopControllerAsync();
        }

        private void Subscribe()
        {
            _telemetryViewsSynchronization.NewSessionLoaded += OnSessionStarted;
            _telemetryViewsSynchronization.SessionAdded += OnSessionAdded;
            _telemetryViewsSynchronization.LapAddedToSession += OnLapAddedToSession;
            _lapSelectionViewModel.LapSelected += LapSelectionViewModelOnLapSelected;
            _lapSelectionViewModel.LapUnselected += LapSelectionViewModelOnLapUnselected;
            _telemetryViewsSynchronization.LapLoaded += TelemetryViewsSynchronizationOnLapLoaded;
            _telemetryViewsSynchronization.LapUnloaded += TelemetryViewsSynchronizationOnLapUnloaded;
        }

        private void TelemetryViewsSynchronizationOnLapUnloaded(object sender, LapSummaryArgs e)
        {
            _loadedLaps.RemoveAll(x => x.Id == e.LapSummary.Id);
            var lapViewModel = _lapSelectionViewModel.LapSummaries.FirstOrDefault(x => x.OriginalModel.Id == e.LapSummary.Id);
            if (lapViewModel == null)
            {
                return;
            }
            lapViewModel.Selected = false;
        }

        private void TelemetryViewsSynchronizationOnLapLoaded(object sender, LapTelemetryArgs e)
        {
            _loadedLaps.Add(e.LapTelemetry.LapSummary);
            var lapViewModel = _lapSelectionViewModel.LapSummaries.FirstOrDefault(x => x.OriginalModel.Id == e.LapTelemetry.LapSummary.Id);
            if (lapViewModel == null)
            {
                return;
            }
            lapViewModel.Selected = true;
        }

        private void UnSubscribe()
        {
            _telemetryViewsSynchronization.NewSessionLoaded -= OnSessionStarted;
            _telemetryViewsSynchronization.SessionAdded -= OnSessionAdded;
            _telemetryViewsSynchronization.LapAddedToSession -= OnLapAddedToSession;
            _lapSelectionViewModel.LapSelected -= LapSelectionViewModelOnLapSelected;
            _lapSelectionViewModel.LapUnselected -= LapSelectionViewModelOnLapUnselected;
            _telemetryViewsSynchronization.LapLoaded -= TelemetryViewsSynchronizationOnLapLoaded;
            _telemetryViewsSynchronization.LapUnloaded -= TelemetryViewsSynchronizationOnLapUnloaded;
        }



        private void LapSelectionViewModelOnLapUnselected(object sender, LapSummaryArgs e)
        {
            if (!_loadedLaps.Exists(x => x.Id == e.LapSummary.Id))
            {
                return;
            }

            _telemetryLoadController.UnloadLap(e.LapSummary);
        }

        private void LapSelectionViewModelOnLapSelected(object sender, LapSummaryArgs e)
        {
            if (_loadedLaps.Exists(x => x.Id == e.LapSummary.Id))
            {
                return;
            }
            _telemetryLoadController.LoadLap(e.LapSummary);
        }

        private void OnLapAddedToSession(object sender, LapSummaryArgs e)
        {
            AddLaps(new List<LapSummaryDto>(){ e.LapSummary});
        }

        private void AddLaps(IReadOnlyCollection<LapSummaryDto> lapsSummary)
        {
            foreach (LapSummaryDto lapSummaryDto in lapsSummary)
            {
                ILapSummaryViewModel newViewModel = _viewModelFactory.Create<ILapSummaryViewModel>();
                newViewModel.FromModel(lapSummaryDto);
                newViewModel.LapColor = _colorPaletteProvider.GetNext();
                _lapSelectionViewModel.AddLapSummaryViewModel(newViewModel);
                _allAvailableLaps.Add(lapSummaryDto);
            }

            LapSummaryDto bestLap = _allAvailableLaps.Where(x => x.LapTime != TimeSpan.Zero). OrderBy(x => x.LapTime).FirstOrDefault();
            if (bestLap != null)
            {
                _lapSelectionViewModel.BestLap = $"{bestLap?.CustomDisplayName} - {bestLap.LapTime.FormatToDefault()}";
            }

            LapSummaryDto bestSector1Lap = _allAvailableLaps.Where(x => x.Sector1Time > TimeSpan.Zero).OrderBy(x => x.Sector1Time).FirstOrDefault();
            _lapSelectionViewModel.BestSector1 = bestSector1Lap?.Sector1Time > TimeSpan.Zero ? $"{bestSector1Lap.CustomDisplayName} - {bestSector1Lap.Sector1Time.FormatToDefault()}" : string.Empty;

            LapSummaryDto bestSector2Lap = _allAvailableLaps.Where(x => x.Sector2Time > TimeSpan.Zero).OrderBy(x => x.Sector1Time).FirstOrDefault();
            _lapSelectionViewModel.BestSector2 = bestSector2Lap?.Sector2Time > TimeSpan.Zero ? $"{bestSector2Lap.CustomDisplayName} - {bestSector2Lap.Sector2Time.FormatToDefault()}" : string.Empty;

            LapSummaryDto bestSector3Lap = _allAvailableLaps.Where(x => x.Sector3Time > TimeSpan.Zero).OrderBy(x => x.Sector1Time).FirstOrDefault();
            _lapSelectionViewModel.BestSector3 = bestSector3Lap?.Sector3Time > TimeSpan.Zero ? $"{bestSector3Lap.CustomDisplayName} - {bestSector3Lap.Sector3Time.FormatToDefault()}" : string.Empty;
        }

        private void AddLapsFromSession(SessionInfoDto sessionInfoDto)
        {
            AddLaps(sessionInfoDto.LapsSummary);
        }

        private void ReinitializeViewMode(SessionInfoDto sessionInfoDto)
        {
            _lapSelectionViewModel.Clear();
            _allAvailableLaps.Clear();
            _loadedLaps.Clear();
            _lapSelectionViewModel.TrackName = string.IsNullOrEmpty(sessionInfoDto.LayoutName) ? sessionInfoDto.TrackName : $"{sessionInfoDto.TrackName} - {sessionInfoDto.LayoutName}";
            _lapSelectionViewModel.CarName = sessionInfoDto.CarName;
            _lapSelectionViewModel.SessionTime = sessionInfoDto.SessionRunDateTime;
            _lapSelectionViewModel.SimulatorName = sessionInfoDto.Simulator;
            AddLapsFromSession(sessionInfoDto);
        }

        private void OnSessionAdded(object sender, TelemetrySessionArgs e)
        {
            AddLapsFromSession(e.SessionInfoDto);
        }

        private async Task AddCustomLap()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog {DefaultExt = ".lap; .plap", Filter = "Lap Files (*.lap, *.plap)|*.lap; *.plap" };
            bool? result = dlg.ShowDialog();
            if (result == false)
            {
                return;
            }

            string filename = dlg.FileName;
            string fileCustomName = await _userInputProvider.GetUserInput("Enter Lap Name:", $"Ex-{Path.GetFileNameWithoutExtension(filename)}");
            await _telemetryLoadController.LoadLap(new FileInfo(filename), fileCustomName);
        }


        private void OnSessionStarted(object sender, TelemetrySessionArgs e)
        {
            ReinitializeViewMode(e.SessionInfoDto);
        }
    }
}