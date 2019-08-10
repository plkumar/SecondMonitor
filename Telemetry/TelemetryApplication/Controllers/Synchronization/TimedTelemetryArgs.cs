namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.Synchronization
{
    using System;
    using System.Collections.Generic;
    using DataModel.Telemetry;
    using TelemetryManagement.StoryBoard;

    public class TimedTelemetryArgs : EventArgs
    {
        public TimedTelemetryArgs(IReadOnlyCollection<TimedTelemetrySnapshot> telemetrySnapshots)
        {
            TelemetrySnapshots = telemetrySnapshots;
        }

        public IReadOnlyCollection<TimedTelemetrySnapshot> TelemetrySnapshots { get; }
    }
}