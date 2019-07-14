namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class SpeedToRakeProvider : AbstractStintScatterPlotProvider
    {
        public SpeedToRakeProvider(SpeedToRakeExtractor dataExtractor, ILoadedLapsCache loadedLapsCache) : base(loadedLapsCache, dataExtractor)
        {

        }

        public override string ChartName => "Rake / Speed";
        public override AggregatedChartKind Kind => AggregatedChartKind.ScatterPlot;

    }
}