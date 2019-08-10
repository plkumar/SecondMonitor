namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Controllers.Synchronization;
    using DataModel.BasicProperties;
    using DataModel.Extensions;
    using Extractors;
    using OxyPlot;
    using SecondMonitor.ViewModels.Colors;
    using SecondMonitor.ViewModels.Colors.Extensions;
    using Settings.DTO;
    using TelemetryManagement.DTO;
    using ViewModels.AggregatedCharts;
    using ViewModels.AggregatedCharts.ScatterPlot;
    using ViewModels.LoadedLapCache;

    public class WheelSlipBrakeProvider : AbstractAggregatedChartProvider
    {
        private readonly WheelSlipExtractor _dataExtractor;
        private readonly IDataPointSelectionSynchronization _dataPointSelectionSynchronization;
        private static readonly ColorDto FromColor = ColorDto.FromHex("#FF000FFF");
        private static readonly ColorDto ToColor = ColorDto.FromHex("#ffff0000");
        public static readonly int ColorSteps = 5;

        public WheelSlipBrakeProvider(WheelSlipExtractor dataExtractor, ILoadedLapsCache loadedLapsCache, IDataPointSelectionSynchronization dataPointSelectionSynchronization) : base(loadedLapsCache)
        {
            _dataExtractor = dataExtractor;
            _dataPointSelectionSynchronization = dataPointSelectionSynchronization;
        }

        public override string ChartName => "Wheel Slip (Braking)";
        public override AggregatedChartKind Kind => AggregatedChartKind.ScatterPlot;

        public override IReadOnlyCollection<IAggregatedChartViewModel> CreateAggregatedChartViewModels(AggregatedChartSettingsDto aggregatedChartSettings)
        {
            _dataExtractor.ThrottlePositionFilter.Minimum = 0;
            _dataExtractor.ThrottlePositionFilter.Maximum = 0.02;
            IEnumerable<IGrouping<int, LapTelemetryDto>> lapsStintGrouping = GetLapsGrouped(aggregatedChartSettings);
            if (aggregatedChartSettings.StintRenderingKind == StintRenderingKind.SingleChart)
            {
                return CreateChartForAllStints(lapsStintGrouping.ToList());
            }
            else
            {
                return CreateChartForEachStint(lapsStintGrouping, aggregatedChartSettings);
            }
        }

        private IReadOnlyCollection<IAggregatedChartViewModel> CreateChartForAllStints(List<IGrouping<int, LapTelemetryDto>> lapsStintGrouping)
        {
            _dataExtractor.BrakePositionFilter.Minimum = double.MinValue;
            _dataExtractor.BrakePositionFilter.Maximum = double.MaxValue;
            IColorPaletteProvider colorPaletteProvider = new BasicColorPaletteProvider();
            string title = BuildTitleForAllStints(lapsStintGrouping);

            List<ScatterPlotSeries> leftFrontWheelSeries = new List<ScatterPlotSeries>();
            List<ScatterPlotSeries> rightFrontWheelSeries = new List<ScatterPlotSeries>();
            List<ScatterPlotSeries> leftRearWheelSeries = new List<ScatterPlotSeries>();
            List<ScatterPlotSeries> rightRearWheelSeries = new List<ScatterPlotSeries>();

            foreach (IGrouping<int, LapTelemetryDto> lapsGrouped in lapsStintGrouping)
            {
                OxyColor color = colorPaletteProvider.GetNext().ToOxyColor();
                string seriesTitle = $"Laps: {string.Join(", ", lapsGrouped.Select(x => x.LapSummary.CustomDisplayName))} - Stint: {lapsGrouped.Key}";
                leftFrontWheelSeries.Add(_dataExtractor.ExtractFrontLeft(lapsGrouped, "FL :" + seriesTitle, color));
                rightFrontWheelSeries.Add(_dataExtractor.ExtractFrontRight(lapsGrouped, "FR :" + seriesTitle, color));
                leftRearWheelSeries.Add(_dataExtractor.ExtractRearLeft(lapsGrouped, "RL :" + seriesTitle, color));
                rightRearWheelSeries.Add(_dataExtractor.ExtractRearRight(lapsGrouped, "RR :" + seriesTitle, color));
            }

            WheelsChartViewModel mainViewModel = new WheelsChartViewModel()
            {
                Title = title,
                FrontLeftChartViewModel = CreateScatterPlotChartViewModel("Front Left", leftFrontWheelSeries.ToArray()),
                FrontRightChartViewModel = CreateScatterPlotChartViewModel("Front Right", rightFrontWheelSeries.ToArray()),
                RearLeftChartViewModel = CreateScatterPlotChartViewModel("Rear Left", leftRearWheelSeries.ToArray()),
                RearRightChartViewModel = CreateScatterPlotChartViewModel("Rear Right", rightRearWheelSeries.ToArray())
            };
            return new[] { mainViewModel };
        }

        public IReadOnlyCollection<IAggregatedChartViewModel> CreateChartForEachStint(IEnumerable<IGrouping<int, LapTelemetryDto>> lapsStintGrouping, AggregatedChartSettingsDto aggregatedChartSettings)
        {
            IColorPaletteProvider colorPaletteProvider = new ColorRangePaletteProvider(FromColor, ToColor, ColorSteps);
            List<IAggregatedChartViewModel> charts = new List<IAggregatedChartViewModel>();
            double brakeStep = 1.0 / (colorPaletteProvider.PaletteSize - 1);
            foreach (IGrouping<int, LapTelemetryDto> lapsGrouped in lapsStintGrouping)
            {
                string title = BuildChartTitle(lapsGrouped, aggregatedChartSettings);
                List<ScatterPlotSeries> leftFrontWheelSeries = new List<ScatterPlotSeries>();
                List<ScatterPlotSeries> rightFrontWheelSeries = new List<ScatterPlotSeries>();
                List<ScatterPlotSeries> leftRearWheelSeries = new List<ScatterPlotSeries>();
                List<ScatterPlotSeries> rightRearWheelSeries = new List<ScatterPlotSeries>();

                colorPaletteProvider.Reset();

                OxyColor color;
                string brakeTitle;
                string seriesTitle;

                for (int i = 0; i < colorPaletteProvider.PaletteSize - 1; i++)
                {
                    color = colorPaletteProvider.GetNext().ToOxyColor();
                    _dataExtractor.BrakePositionFilter.Minimum = brakeStep * i;
                    _dataExtractor.BrakePositionFilter.Maximum = brakeStep * (i + 1);
                    brakeTitle = $"Brake: {_dataExtractor.BrakePositionFilter.Minimum * 100:F0} - {_dataExtractor.BrakePositionFilter.Maximum * 100:F0}";
                    seriesTitle = $"Stint: {lapsGrouped.Key} " + brakeTitle;

                    leftFrontWheelSeries.Add(_dataExtractor.ExtractFrontLeft(lapsGrouped, "FL :" + seriesTitle, color));
                    rightFrontWheelSeries.Add(_dataExtractor.ExtractFrontRight(lapsGrouped, "FR :" + seriesTitle, color));
                    leftRearWheelSeries.Add(_dataExtractor.ExtractRearLeft(lapsGrouped, "RL :" + seriesTitle, color));
                    rightRearWheelSeries.Add(_dataExtractor.ExtractRearRight(lapsGrouped, "RR :" + seriesTitle, color));
                }

                color = colorPaletteProvider.GetNext().ToOxyColor();
                _dataExtractor.BrakePositionFilter.Minimum = 1;
                _dataExtractor.BrakePositionFilter.Maximum = double.MaxValue;
                brakeTitle = $"Brake: 100";
                seriesTitle = $"Stint: {lapsGrouped.Key} " + brakeTitle;

                leftFrontWheelSeries.Add(_dataExtractor.ExtractFrontLeft(lapsGrouped, "FL :" + seriesTitle, color));
                rightFrontWheelSeries.Add(_dataExtractor.ExtractFrontRight(lapsGrouped, "FR :" + seriesTitle, color));
                leftRearWheelSeries.Add(_dataExtractor.ExtractRearLeft(lapsGrouped, "RL :" + seriesTitle, color));
                rightRearWheelSeries.Add(_dataExtractor.ExtractRearRight(lapsGrouped, "RR :" + seriesTitle, color));

                WheelsChartViewModel mainViewModel = new WheelsChartViewModel()
                {
                    Title = title,
                    FrontLeftChartViewModel = CreateScatterPlotChartViewModel("Front Left", leftFrontWheelSeries.ToArray()),
                    FrontRightChartViewModel = CreateScatterPlotChartViewModel("Front Right", rightFrontWheelSeries.ToArray()),
                    RearLeftChartViewModel = CreateScatterPlotChartViewModel("Rear Left", leftRearWheelSeries.ToArray()),
                    RearRightChartViewModel = CreateScatterPlotChartViewModel("Rear Right", rightRearWheelSeries.ToArray())
                };

                charts.Add(mainViewModel);
            }

            return charts;
        }

        private ScatterPlotChartViewModel CreateScatterPlotChartViewModel(string title, params ScatterPlotSeries[] series)
        {
            ScatterPlot scatterPlot = new ScatterPlot(title, CreateXAxisDefinition(), CreateYAxisDefinition());
            scatterPlot.YAxis.SetCustomRange(-1, 1);
            series.ForEach(scatterPlot.AddScatterPlotSeries);

            ScatterPlotChartViewModel viewModel = new ScatterPlotChartViewModel(_dataPointSelectionSynchronization) { Title = title };
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
