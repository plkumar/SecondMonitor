namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class SpeedToDownforceProvider : AbstractStintScatterPlotProvider
    {
        public SpeedToDownforceProvider(SpeedToDownforceExtractor dataExtractor, ILoadedLapsCache loadedLapsCache) : base(loadedLapsCache, dataExtractor)
        {
        }

        public override string ChartName => "Downforce / Speed";
        public override AggregatedChartKind Kind => AggregatedChartKind.ScatterPlot;

    }
}