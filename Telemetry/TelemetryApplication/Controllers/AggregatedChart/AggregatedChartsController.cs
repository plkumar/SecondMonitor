namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.AggregatedChart
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using AggregatedCharts;
    using Contracts.Commands;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;
    using Settings;
    using Synchronization;
    using TelemetryApplication.Settings.DTO;
    using ViewModels;
    using ViewModels.AggregatedCharts;
    using ViewModels.LoadedLapCache;
    using ViewModels.SettingsWindow;

    public class AggregatedChartsController : IAggregatedChartsController
    {

        private readonly IMainWindowViewModel _mainWindowViewModel;
        private readonly ILoadedLapsCache _loadedLapsCache;
        private readonly IWindowService _windowService;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly ITelemetryViewsSynchronization _telemetryViewsSynchronization;
        private readonly ISettingsController _settingsController;
        private readonly List<IAggregatedChartProvider> _aggregatedChartProviders;
        private Window _chartSelectionWindow;

        public AggregatedChartsController(IEnumerable<IAggregatedChartProvider> aggregatedChartProviders, IMainWindowViewModel mainWindowViewModel, ILoadedLapsCache loadedLapsCache, IWindowService windowService, IViewModelFactory viewModelFactory,
            ITelemetryViewsSynchronization telemetryViewsSynchronization, ISettingsController settingsController)
        {
            _loadedLapsCache = loadedLapsCache;
            _windowService = windowService;
            _viewModelFactory = viewModelFactory;
            _telemetryViewsSynchronization = telemetryViewsSynchronization;
            _settingsController = settingsController;
            _mainWindowViewModel = mainWindowViewModel;
            _loadedLapsCache = loadedLapsCache;
            _aggregatedChartProviders = aggregatedChartProviders.ToList();
        }

        public Task StartControllerAsync()
        {
            BindCommands();
            Subscribe();
            return Task.CompletedTask;
        }

        private void Subscribe()
        {
            _telemetryViewsSynchronization.LapLoaded += TelemetryViewsSynchronizationOnLapLoaded;
            _telemetryViewsSynchronization.LapUnloaded += TelemetryViewsSynchronizationOnLapUnloaded;
        }

        private void UnSubscribe()
        {
            _telemetryViewsSynchronization.LapLoaded -= TelemetryViewsSynchronizationOnLapLoaded;
            _telemetryViewsSynchronization.LapUnloaded -= TelemetryViewsSynchronizationOnLapUnloaded;
        }

        private void TelemetryViewsSynchronizationOnLapLoaded(object sender, LapTelemetryArgs e)
        {
            RefreshOpenSelectorButton();
        }

        private void TelemetryViewsSynchronizationOnLapUnloaded(object sender, LapSummaryArgs e)
        {
            RefreshOpenSelectorButton();
        }


        private void RefreshOpenSelectorButton()
        {
            _mainWindowViewModel.LapSelectionViewModel.IsOpenAggregatedChartSelectorEnabled = _loadedLapsCache.LoadedLaps.Count > 0;
        }

        private void BindCommands()
        {
            _mainWindowViewModel.LapSelectionViewModel.OpenAggregatedChartSelectorCommand = new RelayCommand(OpenAggregatedChartSelector);
        }

        private void OpenAggregatedChartSelector()
        {
            if (_loadedLapsCache.LoadedLaps.Count == 0)
            {
                return;
            }

            if (_chartSelectionWindow?.IsLoaded == true)
            {
                _chartSelectionWindow.Focus();
                return;
            }

            IAggregatedChartSelectorViewModel viewModel = CreateAggregatedChartSelectionViewModel();
            _chartSelectionWindow = _windowService.OpenWindow(viewModel, "Select Aggregated Chart");
        }

        private IAggregatedChartSelectorViewModel CreateAggregatedChartSelectionViewModel()
        {
            IAggregatedChartSelectorViewModel viewModel = _viewModelFactory.Create<IAggregatedChartSelectorViewModel>();
            viewModel.HistogramChartNames = _aggregatedChartProviders.Where(x => x.Kind == AggregatedChartKind.Histogram).Select(x => x.ChartName).OrderBy(x => x).ToList();
            viewModel.ScatterPlotChartNames = _aggregatedChartProviders.Where(x => x.Kind == AggregatedChartKind.ScatterPlot).Select(x => x.ChartName).OrderBy(x => x).ToList();
            viewModel.CancelAndCloseWindowCommand = new RelayCommand(CancelAndCloseSelectionWindow);
            viewModel.OpenSelectedChartCommand = new RelayCommand(OpenSelectedChart);

            IAggregatedChartSettingsViewModel aggregatedChartSettingsViewModel = _viewModelFactory.Create<IAggregatedChartSettingsViewModel>();
            aggregatedChartSettingsViewModel.FromModel(_settingsController.TelemetrySettings.AggregatedChartSettings);
            viewModel.AggregatedChartSettingsViewModel = aggregatedChartSettingsViewModel;
            return viewModel;
        }

        private void OpenSelectedChart()
        {
            if (!(_chartSelectionWindow?.Content is IAggregatedChartSelectorViewModel viewModel))
            {
                return;
            }

            CancelAndCloseSelectionWindow();

            string providerName = viewModel.SelectedTabIndex == 0 ? viewModel.SelectedHistogramChartName : viewModel.SelectedScatterPlotChartName;
            if (string.IsNullOrWhiteSpace(providerName))
            {
                return;
            }

            IAggregatedChartProvider selectedProvider = _aggregatedChartProviders.FirstOrDefault(x => x.ChartName == providerName);

            if (selectedProvider == null)
            {
                return;
            }

            AggregatedChartSettingsDto aggregatedChartSettingsDto = viewModel.AggregatedChartSettingsViewModel.SaveToNewModel();
            _settingsController.TelemetrySettings.AggregatedChartSettings = aggregatedChartSettingsDto;
            IReadOnlyCollection<IAggregatedChartViewModel> chartViewModels = selectedProvider.CreateAggregatedChartViewModels(aggregatedChartSettingsDto);
            OpenAggregatedCharts(chartViewModels);

        }

        private void OpenAggregatedCharts(IEnumerable<IAggregatedChartViewModel> aggregatedChartViewModels)
        {
            /*List<IAggregatedChartViewModel> enumeratedCharts = aggregatedChartViewModels.ToList();
            if (enumeratedCharts.Count == 2)
            {
                string tile = enumeratedCharts[0].Title;

                void Dispose()
                {
                    enumeratedCharts[0].Dispose();
                    enumeratedCharts[1].Dispose();
                }

                TwoViewModelsLayout viewModel = new TwoViewModelsLayout()
                {
                    FirstViewModel = enumeratedCharts[0],
                    SecondViewModel = enumeratedCharts[1],
                };
                _windowService.OpenWindow(viewModel, tile, WindowState.Maximized, SizeToContent.Manual, (Action) Dispose);
                return;
            }
*/

            foreach (IAggregatedChartViewModel chartViewModel in aggregatedChartViewModels)
            {
                _windowService.OpenWindow(chartViewModel, chartViewModel.Title, WindowState.Maximized, SizeToContent.Manual, chartViewModel.Dispose);
            }
        }

        private void CancelAndCloseSelectionWindow()
        {
            if (_chartSelectionWindow?.IsLoaded != true)
            {
                return;
            }

            _chartSelectionWindow.Close();
        }

        public Task StopControllerAsync()
        {
            UnSubscribe();
            return Task.CompletedTask;
        }


    }
}