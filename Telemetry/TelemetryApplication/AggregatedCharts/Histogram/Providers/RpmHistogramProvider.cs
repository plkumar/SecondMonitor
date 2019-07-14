namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Histogram.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Extractors;
    using SecondMonitor.ViewModels.Factory;
    using Settings.DTO;
    using TelemetryManagement.DTO;
    using ViewModels.AggregatedCharts;
    using ViewModels.AggregatedCharts.Histogram;
    using ViewModels.LoadedLapCache;

    public class RpmHistogramProvider : AbstractAggregatedChartProvider
    {
        private readonly RpmHistogramDataExtractor _rpmHistogramDataExtractor;
        private readonly IViewModelFactory _viewModelFactory;
        public override string ChartName => "RPM Histogram";
        public override AggregatedChartKind Kind => AggregatedChartKind.Histogram;

        public RpmHistogramProvider(ILoadedLapsCache loadedLapsCache, RpmHistogramDataExtractor rpmHistogramDataExtractor, IViewModelFactory viewModelFactory) : base(loadedLapsCache)
        {
            _rpmHistogramDataExtractor = rpmHistogramDataExtractor;
            _viewModelFactory = viewModelFactory;
        }

        public override IReadOnlyCollection<IAggregatedChartViewModel> CreateAggregatedChartViewModels(AggregatedChartSettingsDto aggregatedChartSettings)
        {
            List<IAggregatedChartViewModel> charts = new List<IAggregatedChartViewModel>();
            var groupedByStint = GetLapsGrouped(aggregatedChartSettings);
            foreach (IGrouping<int, LapTelemetryDto> lapsInStint in groupedByStint)
            {
                string title = BuildChartTitle(lapsInStint, aggregatedChartSettings);

                int maxGear = lapsInStint.SelectMany(x => x.DataPoints).Where(x => !string.IsNullOrWhiteSpace(x.PlayerData.CarInfo.CurrentGear) && x.PlayerData.CarInfo.CurrentGear != "R" && x.PlayerData.CarInfo.CurrentGear != "N").Max(x => int.Parse(x.PlayerData.CarInfo.CurrentGear));

                CompositeAggregatedChartsViewModel viewModel = new CompositeAggregatedChartsViewModel() {Title = title};

                HistogramChartViewModel mainViewModel = _viewModelFactory.Create<HistogramChartViewModel>();
                mainViewModel.FromModel(CreateHistogramAllGears(lapsInStint, _rpmHistogramDataExtractor.DefaultBandSize));

                viewModel.MainAggregatedChartViewModel = mainViewModel;

                for (int i = 1; i <= maxGear; i++)
                {
                    Histogram histogram = CreateHistogram(lapsInStint, i, _rpmHistogramDataExtractor.DefaultBandSize);
                    if (histogram == null)
                    {
                        continue;
                    }

                    HistogramChartViewModel child = _viewModelFactory.Create<HistogramChartViewModel>();
                    child.FromModel(histogram);
                    viewModel.AddChildAggregatedChildViewModel(child);

                }
                charts.Add(viewModel);
            }

            return charts;
        }

        protected Histogram CreateHistogram(IEnumerable<LapTelemetryDto> loadedLaps, int gear, double bandSize)
        {
            return _rpmHistogramDataExtractor.ExtractSeriesForGear(loadedLaps, bandSize, gear.ToString());
        }

        protected Histogram CreateHistogramAllGears(IEnumerable<LapTelemetryDto> loadedLaps, double bandSize)
        {
            return _rpmHistogramDataExtractor.ExtractSeriesForAllGear(loadedLaps, bandSize);
        }
    }
}