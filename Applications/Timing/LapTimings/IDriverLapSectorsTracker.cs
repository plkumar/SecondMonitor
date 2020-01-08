namespace SecondMonitor.Timing.LapTimings
{
    using System;

    public interface IDriverLapSectorsTracker
    {
        void Update();

        TimeSpan GetSectionTime(double lapDistance);

        TimeSpan GetRelativeGapToPlayer();
    }
}