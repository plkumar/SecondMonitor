namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Filter
{
    using DataModel.Telemetry;

    public interface ITelemetryFilter
    {
        bool Accepts(TimedTelemetrySnapshot dataSet);
    }
}