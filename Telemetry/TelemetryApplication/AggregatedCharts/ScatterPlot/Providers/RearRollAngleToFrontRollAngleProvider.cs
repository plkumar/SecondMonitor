namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Extractors;
    using Filter;
    using OxyPlot;
    using OxyPlot.Annotations;
    using SecondMonitor.ViewModels.Colors;
    using SecondMonitor.ViewModels.Colors.Extensions;
    using Settings.DTO;
    using TelemetryManagement.DTO;
    using ViewModels.AggregatedCharts;
    using ViewModels.AggregatedCharts.ScatterPlot;
    using ViewModels.LoadedLapCache;

    public class RearRollAngleToFrontRollAngleProvider : AbstractAggregatedChartProvider
    {
        protected static readonly List<OxyColor> ColorMap = new RedGreenGradientPalette().GetAllColors().Select(x => x.ToColor().ToOxyColor()).ToList();

        private readonly RearRollAngleToFrontRollAngleExtractor _dataExtractor;
        private readonly LateralAccFilter _lateralAccFilter;
        private readonly List<ITelemetryFilter> _filters;

        public override string ChartName => "Rear Roll Angle / Front Roll Angle ";

        public override AggregatedChartKind Kind => AggregatedChartKind.ScatterPlot;

        public RearRollAngleToFrontRollAngleProvider(ILoadedLapsCache loadedLapsCache, RearRollAngleToFrontRollAngleExtractor dataExtractor, LateralAccFilter lateralAccFilter) : base(loadedLapsCache)
        {
            _dataExtractor = dataExtractor;
            _lateralAccFilter = lateralAccFilter;
            _filters = new List<ITelemetryFilter>() {_lateralAccFilter};
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
            var lapsInStints = lapsStintGrouping as IGrouping<int, LapTelemetryDto>[] ?? lapsStintGrouping.ToArray();
            string title = BuildTitleForAllStints(lapsInStints);
            IColorPaletteProvider colorPaletteProvider = new BasicColorPaletteProvider();
            AxisDefinition xAxis = new AxisDefinition(_dataExtractor.XMajorTickSize, _dataExtractor.XMajorTickSize / 4, _dataExtractor.XUnit, "Rear Roll Angle");
            AxisDefinition yAxis = new AxisDefinition(_dataExtractor.YMajorTickSize, _dataExtractor.YMajorTickSize / 4, _dataExtractor.YUnit, "Front Roll Angle");
            ScatterPlot scatterPlot = new ScatterPlot(title, xAxis, yAxis);
            _lateralAccFilter.MinimumG = 0;
            _lateralAccFilter.MaximumG = double.MaxValue;
            foreach (IGrouping<int, LapTelemetryDto> lapsInStint in lapsInStints)
            {
                string seriesTitle = BuildSeriesTitle(lapsInStint);
                ScatterPlotSeries newSeries = _dataExtractor.ExtractSeries(lapsInStint, _filters, seriesTitle, colorPaletteProvider.GetNext().ToOxyColor());
                scatterPlot.AddScatterPlotSeries(newSeries);
            }

            scatterPlot.AddAnnotation(new LineAnnotation() { Slope = 1, Intercept = 0, Color = OxyColors.Red, StrokeThickness = 1, LineStyle = LineStyle.Solid });
            ScatterPlotChartViewModel viewModel = new ScatterPlotChartViewModel() { Title = title };
            viewModel.FromModel(scatterPlot);
            return new[] {viewModel};
        }

        public IReadOnlyCollection<IAggregatedChartViewModel> CreateChartForEachStint(IEnumerable<IGrouping<int, LapTelemetryDto>> lapsStintGrouping, AggregatedChartSettingsDto aggregatedChartSettings)
        {
            List<IAggregatedChartViewModel> charts = new List<IAggregatedChartViewModel>();

            foreach (IGrouping<int, LapTelemetryDto> lapsInStint in lapsStintGrouping)
            {
                string title = BuildChartTitle(lapsInStint, aggregatedChartSettings);
                AxisDefinition xAxis = new AxisDefinition(_dataExtractor.XMajorTickSize, _dataExtractor.XMajorTickSize / 4, _dataExtractor.XUnit, "Rear Roll Angle");
                AxisDefinition yAxis = new AxisDefinition(_dataExtractor.YMajorTickSize, _dataExtractor.YMajorTickSize / 4, _dataExtractor.YUnit, "Front Roll Angle");
                ScatterPlot scatterPlot = new ScatterPlot(title, xAxis, yAxis);

                for (int i = 0; i < ColorMap.Count; i++)
                {
                    double minG = i * 0.25;
                    double maxG = i + 1 == ColorMap.Count ? double.MaxValue : (i + 1) * 0.25;
                    _lateralAccFilter.MinimumG = minG;
                    _lateralAccFilter.MaximumG = maxG;
                    string seriesTitle = maxG < double.MaxValue ? $"{minG:F2}G - {maxG:F2}G" : $"{minG:F2}G+";
                    ScatterPlotSeries newSeries = _dataExtractor.ExtractSeries(lapsInStint, _filters, seriesTitle, ColorMap[i]);
                    if (newSeries == null)
                    {
                        continue;
                    }


                }

                scatterPlot.AddAnnotation(new LineAnnotation() {Slope = 1, Intercept = 0, Color = OxyColors.Red, StrokeThickness = 1, LineStyle = LineStyle.Solid});
                ScatterPlotChartViewModel viewModel = new ScatterPlotChartViewModel() {Title = title};
                viewModel.FromModel(scatterPlot);
                charts.Add(viewModel);
            }

            return charts;
        }
    }
}