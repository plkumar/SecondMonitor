namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Controllers.Synchronization;
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class RideHeightToLateralAccProvider : AbstractWheelChartProvider
    {
        public RideHeightToLateralAccProvider(LateralAccelerationToRideHeightExtractor dataExtractor, ILoadedLapsCache loadedLaps, IDataPointSelectionSynchronization dataPointSelectionSynchronization) : base(dataExtractor, loadedLaps, dataPointSelectionSynchronization)
        {
        }

        public override string ChartName => "Ride Height / Lateral Acceleration";
    }
}