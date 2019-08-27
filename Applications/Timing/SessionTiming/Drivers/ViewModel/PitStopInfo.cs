namespace SecondMonitor.Timing.SessionTiming.Drivers.ViewModel
{
    using System;

    using DataModel.Snapshot;
    using Timing.Presentation.ViewModel;

    public class PitStopInfo
    {
        public enum PitPhase { Entry, InPits, Exit, Completed }

        public PitStopInfo(SimulatorDataSet set, DriverTiming driver, ILapInfo entryLap)
        {
            Driver = driver;
            EntryLap = entryLap;
            Phase = PitPhase.Entry;
            PitEntry = set.SessionInfo.SessionTime;
            PitStopDuration = TimeSpan.Zero;
            PitExit = PitEntry;
            PitStopStart = PitEntry;
            PitStopEnd = PitEntry;
        }

        public PitPhase Phase { get; private set; }

        public bool Completed => Phase == PitPhase.Completed;

        public bool WasDriveThrough { get; private set; }

        public DriverTiming Driver { get; }

        public ILapInfo EntryLap { get; }

        public TimeSpan PitEntry { get; }

        public TimeSpan PitExit { get; private set; }

        public TimeSpan PitStopStart { get; private set; }

        public TimeSpan PitStopEnd { get; private set; }

        public TimeSpan PitStopDuration { get; private set; }

        public void Tick(SimulatorDataSet set)
        {
            if (Phase == PitPhase.Completed)
            {
                return;
            }

            if (Phase == PitPhase.Entry && Driver.DriverInfo.Speed.InKph < 1)
            {
                Phase = PitPhase.InPits;
                PitStopStart = set.SessionInfo.SessionTime;
            }

            if (Phase == PitPhase.InPits && Driver.DriverInfo.Speed.InKph > 1)
            {
                Phase = PitPhase.Exit;
                PitStopEnd = set.SessionInfo.SessionTime;
            }

            if (!Driver.DriverInfo.InPits)
            {
                WasDriveThrough = Phase == PitPhase.Entry;
                Phase = PitPhase.Completed;
                PitExit = set.SessionInfo.SessionTime;
                PitStopDuration = PitExit.Subtract(PitEntry);
            }

            if(Phase == PitPhase.Entry)
            {
                PitStopStart = set.SessionInfo.SessionTime;
                PitStopEnd = set.SessionInfo.SessionTime;
            }

            if(Phase == PitPhase.InPits)
            {
                PitStopEnd = set.SessionInfo.SessionTime;
            }

            if (Phase != PitPhase.Completed)
            {
                PitExit = set.SessionInfo.SessionTime;
                PitStopDuration = PitExit.Subtract(PitEntry);
            }


        }

        public string PitInfoFormatted
        {
            get
            {
                switch(Phase)
                {
                    case PitPhase.Entry:
                        return "-->" + PitStopDuration.FormatTimeSpanOnlySecondNoMiliseconds(false);
                    case PitPhase.InPits:
                        return "---" + PitStopDuration.FormatTimeSpanOnlySecondNoMiliseconds(false) + "---";
                    case PitPhase.Exit:
                        return PitStopDuration.FormatTimeSpanOnlySecondNoMiliseconds(false) + " -->";
                    case PitPhase.Completed:
                        return PitStopDuration.FormatTimeSpanOnlySecondNoMiliseconds(false);
                    default:
                        return string.Empty;
                }
            }
        }

    }
}
