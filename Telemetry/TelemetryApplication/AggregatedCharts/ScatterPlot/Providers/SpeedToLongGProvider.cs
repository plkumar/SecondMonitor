namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class SpeedToLongGProvider : AbstractStintScatterPlotProvider
    {
        public SpeedToLongGProvider(ILoadedLapsCache loadedLapsCache, SpeedToLongGAllPointsExtractor dataExtractor) : base(loadedLapsCache, dataExtractor)
        {
        }

        public override string ChartName => "Long vs Speed";
        public override AggregatedChartKind Kind => AggregatedChartKind.ScatterPlot;
    }
}