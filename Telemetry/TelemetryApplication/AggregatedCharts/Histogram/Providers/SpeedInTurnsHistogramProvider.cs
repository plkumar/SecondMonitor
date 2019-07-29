namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Histogram.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts.Commands;
    using Extractors;
    using Filter;
    using SecondMonitor.ViewModels.Factory;
    using Settings.DTO;
    using TelemetryManagement.DTO;
    using ViewModels.AggregatedCharts;
    using ViewModels.AggregatedCharts.Histogram;
    using ViewModels.LoadedLapCache;

    public class SpeedInTurnsHistogramProvider : AbstractAggregatedChartProvider
    {
        private readonly SpeedHistogramExtractor _speedHistogramExtractor;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly List<ITelemetryFilter> _filter;

        public SpeedInTurnsHistogramProvider(SpeedHistogramExtractor speedHistogramExtractor, LateralAccFilter lateralAccFilter, ILoadedLapsCache loadedLapsCache, IViewModelFactory viewModelFactory) : base(loadedLapsCache)
        {
            _speedHistogramExtractor = speedHistogramExtractor;
            _viewModelFactory = viewModelFactory;
            lateralAccFilter.MinimumG = 0.4;
            lateralAccFilter.MaximumG = double.MaxValue;
            _filter = new List<ITelemetryFilter>() { lateralAccFilter};
        }

        public override string ChartName => "Speed in Turns";
        public override AggregatedChartKind Kind => AggregatedChartKind.Histogram;
        public override IReadOnlyCollection<IAggregatedChartViewModel> CreateAggregatedChartViewModels(AggregatedChartSettingsDto aggregatedChartSettings)
        {
            List<IAggregatedChartViewModel> charts = new List<IAggregatedChartViewModel>();
            var groupedByStint = GetLapsGrouped(aggregatedChartSettings);
            foreach (IGrouping<int, LapTelemetryDto> lapsInStint in groupedByStint)
            {
                string title = BuildChartTitle(lapsInStint, aggregatedChartSettings);

                HistogramChartViewModel viewModel = _viewModelFactory.Create<HistogramChartViewModel>();
                viewModel.IsBandVisible = true;
                viewModel.Title = title;
                viewModel.Unit = _speedHistogramExtractor.YUnit;
                viewModel.BandSize = _speedHistogramExtractor.DefaultBandSize;
                viewModel.RefreshCommand = new RelayCommand(() => FillHistogramViewmodel(lapsInStint.ToList(), viewModel));
                FillHistogramViewmodel(lapsInStint.ToList(), viewModel);
                charts.Add(viewModel);
            }

            return charts;
        }

        private void RefreshHistogram(IReadOnlyCollection<LapTelemetryDto> loadedLaps, HistogramChartViewModel viewModel)
        {
            FillHistogramViewmodel(loadedLaps, viewModel);
        }

        private void FillHistogramViewmodel(IReadOnlyCollection<LapTelemetryDto> loadedLaps, HistogramChartViewModel viewModel)
        {
            viewModel.FromModel(_speedHistogramExtractor.ExtractHistogram(loadedLaps, _filter, viewModel.BandSize, viewModel.Title));
        }
    }
}