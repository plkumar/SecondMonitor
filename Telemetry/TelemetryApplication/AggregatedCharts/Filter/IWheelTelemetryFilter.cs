namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Filter
{
    public interface IWheelTelemetryFilter  : ITelemetryFilter
    {
        void FilterFrontLeft();
        void FilterFrontRight();
        void FilterRearLeft();
        void FilterRearRight();
    }
}