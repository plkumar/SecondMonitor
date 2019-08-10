namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Controllers.Synchronization;
    using DataModel.Extensions;
    using Extractors;
    using Filter;
    using OxyPlot;
    using SecondMonitor.ViewModels.Colors;
    using SecondMonitor.ViewModels.Colors.Extensions;
    using Settings.DTO;
    using TelemetryManagement.DTO;
    using ViewModels.AggregatedCharts;
    using ViewModels.AggregatedCharts.ScatterPlot;
    using ViewModels.LoadedLapCache;

    public abstract class AbstractWheelChartProvider : AbstractAggregatedChartProvider
    {
        private readonly AbstractWheelScatterPlotDataExtractor _dataExtractor;
        private readonly IDataPointSelectionSynchronization _dataPointSelectionSynchronization;

        public override AggregatedChartKind Kind => AggregatedChartKind.ScatterPlot;

        protected AbstractWheelChartProvider(AbstractWheelScatterPlotDataExtractor dataExtractor, ILoadedLapsCache loadedLaps, IDataPointSelectionSynchronization dataPointSelectionSynchronization) : base(loadedLaps)
        {
            _dataExtractor = dataExtractor;
            _dataPointSelectionSynchronization = dataPointSelectionSynchronization;
        }

        public override IReadOnlyCollection<IAggregatedChartViewModel> CreateAggregatedChartViewModels(AggregatedChartSettingsDto aggregatedChartSettings)
        {
            IEnumerable<IGrouping<int, LapTelemetryDto>> lapsStintGrouping = GetLapsGrouped(aggregatedChartSettings);
            if (aggregatedChartSettings.StintRenderingKind == StintRenderingKind.SingleChart)
            {
                return CreateChartForAllStints(lapsStintGrouping);
            }
            else
            {
                return CreateChartForEachStint(lapsStintGrouping, aggregatedChartSettings);
            }
        }

        private IReadOnlyCollection<IAggregatedChartViewModel> CreateChartForAllStints(IEnumerable<IGrouping<int, LapTelemetryDto>> lapsStintGrouping)
        {
            IColorPaletteProvider colorPaletteProvider = new BasicColorPaletteProvider();
            var lapsInStints = lapsStintGrouping as IGrouping<int, LapTelemetryDto>[] ?? lapsStintGrouping.ToArray();
            string title = BuildTitleForAllStints(lapsInStints);

            List<ScatterPlotSeries> allWheelsSeries = new List<ScatterPlotSeries>();
            List<ScatterPlotSeries> leftFrontWheelSeries = new List<ScatterPlotSeries>();
            List<ScatterPlotSeries> rightFrontWheelSeries = new List<ScatterPlotSeries>();
            List<ScatterPlotSeries> leftRearWheelSeries = new List<ScatterPlotSeries>();
            List<ScatterPlotSeries> rightRearWheelSeries = new List<ScatterPlotSeries>();

            foreach (IGrouping<int, LapTelemetryDto> lapsGrouped in lapsInStints)
            {
                OxyColor color = colorPaletteProvider.GetNext().ToOxyColor();
                string seriesTitle = $"Laps: {string.Join(", ", lapsGrouped.Select(x => x.LapSummary.CustomDisplayName))} - Stint: {lapsGrouped.Key}";
                allWheelsSeries.Add(_dataExtractor.ExtractMultiPointSeries(lapsGrouped, new ITelemetryFilter[0],"All" + seriesTitle, color));
                leftFrontWheelSeries.Add(_dataExtractor.ExtractFrontLeft(lapsGrouped, "FL :"+ seriesTitle, color));
                rightFrontWheelSeries.Add(_dataExtractor.ExtractFrontRight(lapsGrouped, "FR :" + seriesTitle, color));
                leftRearWheelSeries.Add(_dataExtractor.ExtractRearLeft(lapsGrouped, "RL :" + seriesTitle, color));
                rightRearWheelSeries.Add(_dataExtractor.ExtractRearRight(lapsGrouped, "RR :" + seriesTitle, color));
            }

            SplitAggregatedChartViewModel mainViewModel = new SplitAggregatedChartViewModel()
            {
                Title = title,
                TopViewModel = CreateScatterPlotChartViewModel("All Wheels", allWheelsSeries.ToArray()),
                BottomViewModel = new WheelsChartViewModel()
                {
                    FrontLeftChartViewModel = CreateScatterPlotChartViewModel("Front Left", leftFrontWheelSeries.ToArray()),
                    FrontRightChartViewModel = CreateScatterPlotChartViewModel("Front Right", rightFrontWheelSeries.ToArray()),
                    RearLeftChartViewModel = CreateScatterPlotChartViewModel("Rear Left", leftRearWheelSeries.ToArray()),
                    RearRightChartViewModel = CreateScatterPlotChartViewModel("Rear Right", rightRearWheelSeries.ToArray())
                }
            };
            return new[] {mainViewModel};
        }

        public IReadOnlyCollection<IAggregatedChartViewModel> CreateChartForEachStint(IEnumerable<IGrouping<int, LapTelemetryDto>> lapsStintGrouping, AggregatedChartSettingsDto aggregatedChartSettings)
        {
            List<IAggregatedChartViewModel> charts = new List<IAggregatedChartViewModel>();

            foreach (IGrouping<int, LapTelemetryDto> lapsGrouped in lapsStintGrouping)
            {
                string title = BuildChartTitle(lapsGrouped, aggregatedChartSettings);
                ScatterPlotSeries frontLeftSeries = _dataExtractor.ExtractFrontLeft(lapsGrouped);
                ScatterPlotSeries frontRightSeries = _dataExtractor.ExtractFrontRight(lapsGrouped);
                ScatterPlotSeries rearLeftSeries = _dataExtractor.ExtractRearLeft(lapsGrouped);
                ScatterPlotSeries rearRightSeries = _dataExtractor.ExtractRearRight(lapsGrouped);

                SplitAggregatedChartViewModel mainViewModel = new SplitAggregatedChartViewModel()
                {
                    Title = title,
                    TopViewModel = CreateScatterPlotChartViewModel("All Wheels", frontLeftSeries, frontRightSeries, rearLeftSeries, rearRightSeries),
                    BottomViewModel = CreateWheelsChartViewModel(frontLeftSeries, frontRightSeries, rearLeftSeries, rearRightSeries)
                };
                charts.Add(mainViewModel);
            }

            return charts;
        }

        private WheelsChartViewModel CreateWheelsChartViewModel(ScatterPlotSeries fl, ScatterPlotSeries fr, ScatterPlotSeries rl, ScatterPlotSeries rr)
        {
            WheelsChartViewModel wheelsChartViewModel = new WheelsChartViewModel
            {
                FrontLeftChartViewModel = CreateScatterPlotChartViewModel("Front Left", fl),
                FrontRightChartViewModel = CreateScatterPlotChartViewModel("Front Right", fr),
                RearLeftChartViewModel = CreateScatterPlotChartViewModel("Rear Left", rl),
                RearRightChartViewModel = CreateScatterPlotChartViewModel("Rear Right", rr)
            };
            return wheelsChartViewModel;
        }

        private ScatterPlotChartViewModel CreateScatterPlotChartViewModel(string title, params ScatterPlotSeries[] series)
        {
            ScatterPlot scatterPlot = new ScatterPlot(title, CreateXAxisDefinition(), CreateYAxisDefinition());
            series.ForEach(scatterPlot.AddScatterPlotSeries);

            ScatterPlotChartViewModel viewModel = new ScatterPlotChartViewModel(_dataPointSelectionSynchronization) {Title = title};
            viewModel.FromModel(scatterPlot);
            return viewModel;

        }

        protected virtual AxisDefinition CreateXAxisDefinition()
        {
            return new AxisDefinition(_dataExtractor.XMajorTickSize, _dataExtractor.XMajorTickSize / 4, _dataExtractor.XUnit);
        }

        protected virtual AxisDefinition CreateYAxisDefinition()
        {
            return new AxisDefinition(_dataExtractor.YMajorTickSize, _dataExtractor.YMajorTickSize / 4, _dataExtractor.YUnit);
        }
    }
}