namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class RpmToHorizontalGChartProvider : AbstractGearsChartProvider
    {
        public override string ChartName => "Longitudinal Acceleration (RPM)";
        public override AggregatedChartKind Kind => AggregatedChartKind.ScatterPlot;

        public RpmToHorizontalGChartProvider(ILoadedLapsCache loadedLapsCache, RpmToHorizontalGExtractor rpmToHorizontalGExtractor) : base(loadedLapsCache, rpmToHorizontalGExtractor)
        {
        }
    }
}