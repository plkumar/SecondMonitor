namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.Synchronization
{
    using System;

    public interface ILapStintSynchronization
    {
        event EventHandler<LapStintArg> LapStintChanged;

        void SetStintNumberForLap(string lapId, int stint);
    }
}