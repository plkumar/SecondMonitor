namespace SecondMonitor.Timing.SessionTiming
{
    using System;
    using Drivers.ViewModel;

    public class LapEventArgs : EventArgs
    {
        public LapEventArgs(ILapInfo lapInfo)
        {
            Lap = lapInfo;
        }

        public ILapInfo Lap
        {
            get;
        }
    }
}