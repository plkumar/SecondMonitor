namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.TelemetryLoad.DataFillers
{
    using DataModel.Telemetry;

    public interface IMissingTelemetryFiller : ITimedTelemetrySnapshotVisitor
    {
        void SetSimulator(string simulatorName);
    }
}