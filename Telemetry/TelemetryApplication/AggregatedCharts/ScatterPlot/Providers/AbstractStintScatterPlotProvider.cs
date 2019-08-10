namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Controllers.Synchronization;
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

    public abstract class AbstractStintScatterPlotProvider : AbstractAggregatedChartProvider
    {
        private readonly AbstractScatterPlotExtractor _dataExtractor;
        private readonly IDataPointSelectionSynchronization _dataPointSelectionSynchronization;

        protected AbstractStintScatterPlotProvider(ILoadedLapsCache loadedLapsCache, AbstractScatterPlotExtractor dataExtractor, IDataPointSelectionSynchronization dataPointSelectionSynchronization ) : base(loadedLapsCache)
        {
            _dataExtractor = dataExtractor;
            _dataPointSelectionSynchronization = dataPointSelectionSynchronization;
        }

        public bool IsLegendVisible { get; set; }

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
            AxisDefinition xAxis = new AxisDefinition(_dataExtractor.XMajorTickSize, _dataExtractor.XMajorTickSize / 4, _dataExtractor.XUnit);
            AxisDefinition yAxis = new AxisDefinition(_dataExtractor.YMajorTickSize, _dataExtractor.YMajorTickSize / 4, _dataExtractor.YUnit);
            ScatterPlot scatterPlot = new ScatterPlot(title, xAxis, yAxis);

            IColorPaletteProvider colorPaletteProvider = new BasicColorPaletteProvider();
            foreach (IGrouping<int, LapTelemetryDto> lapsInStintGroup in lapsStintGrouping)
            {
                string seriesTitle = $"Laps: {string.Join(", ", lapsInStintGroup.Select(x => x.LapSummary.CustomDisplayName))} - Stint: {lapsInStintGroup.Key}";
                scatterPlot.AddScatterPlotSeries(_dataExtractor.ExtractSeries(lapsInStintGroup, Enumerable.Empty<ITelemetryFilter>().ToList(), seriesTitle, colorPaletteProvider.GetNext().ToOxyColor()));
            }

            ScatterPlotChartViewModel viewModel = new ScatterPlotChartViewModel(_dataPointSelectionSynchronization) { Title = ChartName };
            viewModel.FromModel(scatterPlot);
            return new[] {viewModel};
        }

        protected IReadOnlyCollection<IAggregatedChartViewModel> CreateChartForEachStint(IEnumerable<IGrouping<int, LapTelemetryDto>> lapsStintGrouping, AggregatedChartSettingsDto aggregatedChartSettings)
        {
            List<IAggregatedChartViewModel> charts = new List<IAggregatedChartViewModel>();
            foreach (IGrouping<int, LapTelemetryDto> lapsInStintGroup in lapsStintGrouping)
            {
                string title = BuildChartTitle(lapsInStintGroup, aggregatedChartSettings);

                AxisDefinition xAxis = new AxisDefinition(_dataExtractor.XMajorTickSize, _dataExtractor.XMajorTickSize / 4, _dataExtractor.XUnit);
                AxisDefinition yAxis = new AxisDefinition(_dataExtractor.YMajorTickSize, _dataExtractor.YMajorTickSize / 4, _dataExtractor.YUnit);
                ScatterPlot scatterPlot = new ScatterPlot(title, xAxis, yAxis)
                {
                    IsLegendVisible = IsLegendVisible
                };

                scatterPlot.AddScatterPlotSeries(_dataExtractor.ExtractSeries(lapsInStintGroup, Enumerable.Empty<ITelemetryFilter>().ToList(), title, OxyColors.Green));

                ScatterPlotChartViewModel viewModel = new ScatterPlotChartViewModel(_dataPointSelectionSynchronization) { Title = ChartName };
                viewModel.FromModel(scatterPlot);
                charts.Add(viewModel);
            }

            return charts;

        }

    }
}