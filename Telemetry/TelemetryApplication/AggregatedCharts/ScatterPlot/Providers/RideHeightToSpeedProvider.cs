namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Controllers.Synchronization;
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class RideHeightToSpeedProvider : AbstractWheelChartProvider
    {
        public RideHeightToSpeedProvider(SpeedToRideHeightExtractor dataExtractor, ILoadedLapsCache loadedLaps, IDataPointSelectionSynchronization dataPointSelectionSynchronization) : base(dataExtractor, loadedLaps, dataPointSelectionSynchronization)
        {
        }

        public override string ChartName => "Ride Height / Speed";
    }
}