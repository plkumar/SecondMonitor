namespace SecondMonitor.DataModel.Telemetry
{
    public interface ITimedTelemetrySnapshotVisitor
    {
        void Visit(TimedTelemetrySnapshot timedTelemetrySnapshot);
    }
}