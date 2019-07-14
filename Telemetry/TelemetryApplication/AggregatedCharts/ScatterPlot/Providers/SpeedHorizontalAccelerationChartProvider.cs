namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class SpeedHorizontalAccelerationChartProvider : AbstractGearsChartProvider
    {
        public override string ChartName => "Longitudinal Acceleration (Speed)";
        public override AggregatedChartKind Kind => AggregatedChartKind.ScatterPlot;

        public SpeedHorizontalAccelerationChartProvider(ILoadedLapsCache loadedLapsCache, SpeedToHorizontalGExtractor speedToHorizontalGExtractor) : base(loadedLapsCache, speedToHorizontalGExtractor)
        {
        }
    }
}