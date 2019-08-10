namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Controllers.Synchronization;
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class SpeedHorizontalAccelerationChartProvider : AbstractGearsChartProvider
    {
        public override string ChartName => "Longitudinal Acceleration (Speed)";
        public override AggregatedChartKind Kind => AggregatedChartKind.ScatterPlot;

        public SpeedHorizontalAccelerationChartProvider(ILoadedLapsCache loadedLapsCache, SpeedToHorizontalGExtractor speedToHorizontalGExtractor, IDataPointSelectionSynchronization dataPointSelectionSynchronization) : base(loadedLapsCache, speedToHorizontalGExtractor, dataPointSelectionSynchronization)
        {
        }
    }
}