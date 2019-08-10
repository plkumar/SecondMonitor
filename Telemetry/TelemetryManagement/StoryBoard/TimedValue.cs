namespace SecondMonitor.Telemetry.TelemetryManagement.StoryBoard
{
    using System;
    using System.Collections.Generic;
    using DataModel.Telemetry;

    public struct TimedValue
    {
        public TimedValue(double value, TimedTelemetrySnapshot startSnapshot, TimedTelemetrySnapshot endSnapshot)
        {
            Value = value;
            StartSnapshot = startSnapshot;
            EndSnapshot = endSnapshot;
            ValueTime = endSnapshot.LapTime - startSnapshot.LapTime;
        }

        public double Value { get; }
        public TimedTelemetrySnapshot StartSnapshot { get; }
        public TimedTelemetrySnapshot EndSnapshot { get; }
        public IEnumerable<TimedTelemetrySnapshot> BothPoints => new[] {StartSnapshot, EndSnapshot};
        public TimeSpan ValueTime { get; }
    }
}