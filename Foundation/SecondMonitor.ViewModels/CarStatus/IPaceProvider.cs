namespace SecondMonitor.ViewModels.CarStatus
{
    using System;
    using System.Collections.Generic;

    public interface IPaceProvider
    {
        TimeSpan? PlayersPace { get; }
        TimeSpan? LeadersPace { get; }

        Dictionary<string, TimeSpan> GetPaceForDriversMap();
    }
}