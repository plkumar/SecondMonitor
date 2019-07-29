namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using System.Collections.Generic;
    using System.Linq;
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

    public abstract class AbstractStintScatterPlot2YAxisProvider : AbstractAggregatedChartProvider
    {
        private readonly AbstractScatterPlotExtractor _y1AxisDataExtractor;
        private readonly AbstractScatterPlotExtractor _y2AxisDataExtractor;
        private readonly List<ITelemetryFilter> _filters;

        protected AbstractStintScatterPlot2YAxisProvider(ILoadedLapsCache loadedLapsCache, AbstractScatterPlotExtractor y1AxisDataExtractor, AbstractScatterPlotExtractor y2AxisDataExtractor, IEnumerable<ITelemetryFilter> telemetryFilters) : base(loadedLapsCache)
        {
            _y2AxisDataExtractor = y2AxisDataExtractor;
            _y1AxisDataExtractor = y1AxisDataExtractor;
            _filters = telemetryFilters.ToList();
        }

        protected AbstractStintScatterPlot2YAxisProvider(ILoadedLapsCache loadedLapsCache, AbstractScatterPlotExtractor y1AxisDataExtractor, AbstractScatterPlotExtractor y2AxisDataExtractor) : this(loadedLapsCache, y1AxisDataExtractor, y2AxisDataExtractor, Enumerable.Empty<ITelemetryFilter>())
        {

        }

        protected abstract string Y1Title { get; }

        protected abstract string Y2Title { get; }

        public override IReadOnlyCollection<IAggregatedChartViewModel> CreateAggregatedChartViewModels(AggregatedChartSettingsDto aggregatedChartSettings)
        {
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

        private IReadOnlyCollection<IAggregatedChartViewModel> CreateChartForAllStints(ICollection<IGrouping<int, LapTelemetryDto>> lapsStintGrouping)
        {
            string title = BuildTitleForAllStints(lapsStintGrouping);
            AxisDefinition xAxis = new AxisDefinition(_y1AxisDataExtractor.XMajorTickSize, _y1AxisDataExtractor.XMajorTickSize / 4, _y1AxisDataExtractor.XUnit);
            AxisDefinition y1Axis = new AxisDefinition(_y1AxisDataExtractor.YMajorTickSize, _y1AxisDataExtractor.YMajorTickSize / 4, _y1AxisDataExtractor.YUnit);
            AxisDefinition y2Axis = new AxisDefinition(_y2AxisDataExtractor.YMajorTickSize, _y2AxisDataExtractor.YMajorTickSize / 4, _y2AxisDataExtractor.YUnit);
            ScatterPlot2YAxis scatterPlot = new ScatterPlot2YAxis(title, xAxis, y1Axis, y2Axis);

            IColorPaletteProvider colorPaletteProvider = new BasicColorPaletteProvider();
            foreach (IGrouping<int, LapTelemetryDto> lapsInStintGroup in lapsStintGrouping)
            {
                string series1Title = $"Laps: {string.Join(", ", lapsInStintGroup.Select(x => x.LapSummary.CustomDisplayName))} - Stint: {lapsInStintGroup.Key}";
                scatterPlot.AddScatterPlotSeries(_y1AxisDataExtractor.ExtractSeries(lapsInStintGroup, _filters, Y1Title + "-" + series1Title, colorPaletteProvider.GetNext().ToOxyColor()));
                scatterPlot.AddScatterPlotY2Series(_y2AxisDataExtractor.ExtractSeries(lapsInStintGroup, _filters, Y2Title + "-" + series1Title, colorPaletteProvider.GetNext().ToOxyColor()));

            }
            OnNewScatterPlot(scatterPlot);
            ScatterPlot2YAxisChartViewModel viewModel = new ScatterPlot2YAxisChartViewModel() {Title = ChartName};
            viewModel.FromModel(scatterPlot);
            return new[] {viewModel};
        }

        protected IReadOnlyCollection<IAggregatedChartViewModel> CreateChartForEachStint(IEnumerable<IGrouping<int, LapTelemetryDto>> lapsStintGrouping, AggregatedChartSettingsDto aggregatedChartSettings)
        {
            List<IAggregatedChartViewModel> charts = new List<IAggregatedChartViewModel>();
            foreach (IGrouping<int, LapTelemetryDto> lapsInStintGroup in lapsStintGrouping)
            {
                string title = BuildSeriesTitle(lapsInStintGroup, aggregatedChartSettings);

                AxisDefinition xAxis = new AxisDefinition(_y1AxisDataExtractor.XMajorTickSize, _y1AxisDataExtractor.XMajorTickSize / 4, _y1AxisDataExtractor.XUnit);
                AxisDefinition y1Axis = new AxisDefinition(_y1AxisDataExtractor.YMajorTickSize, _y1AxisDataExtractor.YMajorTickSize / 4, _y1AxisDataExtractor.YUnit);
                AxisDefinition y2Axis = new AxisDefinition(_y2AxisDataExtractor.YMajorTickSize, _y2AxisDataExtractor.YMajorTickSize / 4, _y2AxisDataExtractor.YUnit);
                ScatterPlot2YAxis scatterPlot = new ScatterPlot2YAxis(title, xAxis, y1Axis, y2Axis);

                scatterPlot.AddScatterPlotSeries(_y1AxisDataExtractor.ExtractSeries(lapsInStintGroup, _filters, Y1Title + "-" + title, OxyColors.Green));
                scatterPlot.AddScatterPlotY2Series(_y2AxisDataExtractor.ExtractSeries(lapsInStintGroup, _filters, Y2Title + "-" + title, OxyColors.Red));
                OnNewScatterPlot(scatterPlot);
                ScatterPlot2YAxisChartViewModel viewModel = new ScatterPlot2YAxisChartViewModel() {Title = ChartName};
                viewModel.FromModel(scatterPlot);
                charts.Add(viewModel);
            }

            return charts;
        }

        protected virtual void OnNewScatterPlot(ScatterPlot2YAxis scatterPlot)
        {

        }
    }
}