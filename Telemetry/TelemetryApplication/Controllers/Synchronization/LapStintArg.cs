namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.Synchronization
{
    using System;

    public class LapStintArg : EventArgs
    {
        public LapStintArg(string lapId, int newStintNumber)
        {
            LapId = lapId;
            NewStintNumber = newStintNumber;
        }

        public string LapId { get; set; }
        public int NewStintNumber { get; set; }

    }
}