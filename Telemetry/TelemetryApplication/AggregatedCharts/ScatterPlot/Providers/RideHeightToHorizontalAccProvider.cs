namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Controllers.Synchronization;
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class RideHeightToHorizontalAccProvider : AbstractWheelChartProvider
    {
        public RideHeightToHorizontalAccProvider(HorizontalAccelerationToRideHeightExtractor dataExtractor, ILoadedLapsCache loadedLaps, IDataPointSelectionSynchronization dataPointSelectionSynchronization) : base(dataExtractor, loadedLaps, dataPointSelectionSynchronization)
        {
        }

        public override string ChartName => "Ride Height / Longitudinal Acceleration";
    }
}