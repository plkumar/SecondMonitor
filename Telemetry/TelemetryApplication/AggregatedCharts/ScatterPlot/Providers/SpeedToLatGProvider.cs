namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class SpeedToLatGProvider : AbstractStintScatterPlotProvider
    {
        public SpeedToLatGProvider(ILoadedLapsCache loadedLapsCache, SpeedToLatGAllPointsExtractor dataExtractor) : base(loadedLapsCache, dataExtractor)
        {
        }

        public override string ChartName => "Lat vs Speed";
        public override AggregatedChartKind Kind => AggregatedChartKind.ScatterPlot;
    }
}