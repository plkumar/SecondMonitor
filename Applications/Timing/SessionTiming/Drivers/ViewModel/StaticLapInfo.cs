namespace SecondMonitor.Timing.SessionTiming.Drivers.ViewModel
{
    using System;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;

    public class StaticLapInfo : ILapInfo
    {
        //Implementing interface events, suppress warning
#pragma warning disable CS0067
        public event EventHandler<LapInfo.SectorCompletedArgs> SectorCompletedEvent;
        public event EventHandler<LapEventArgs> LapInvalidatedEvent;
        public event EventHandler<LapEventArgs> LapCompletedEvent;
#pragma warning restore CS0067


        public bool Completed => true;
        public bool Valid => true;
        public bool IsPending => false;
        public int LapNumber { get; }
        public LapCompletionMethod LapCompletionMethod { get; set; }
        public bool PitLap { get; set; }
        public TimeSpan LapTime { get; set; }
        public DriverTiming Driver { get; }
        public SectorTiming Sector1 { get;  }
        public SectorTiming Sector2 { get;  }
        public SectorTiming Sector3 { get;  }
        public TimeSpan CurrentlyValidProgressTime => LapTime;
        public LapTelemetryInfo LapTelemetryInfo { get; }
        public bool FirstLap { get; set; }
        public bool InvalidBySim { get; set; }

        public ILapInfo PreviousLap { get; set; }
        public double CompletedDistance { get;  }

        public StaticLapInfo(int lapNumber, TimeSpan lapTime, bool firstLap, ILapInfo previousLap, double completedDistance, DriverTiming driver)
        {
            LapNumber = lapNumber;
            LapTime = lapTime;
            FirstLap = firstLap;
            PreviousLap = previousLap;
            CompletedDistance = completedDistance;
            Driver = driver;
            Sector1 = new SectorTiming(1, TimeSpan.Zero, this);
            Sector2 = new SectorTiming(2, TimeSpan.Zero, this);
            Sector3 = new SectorTiming(3, TimeSpan.Zero, this);
            LapTelemetryInfo = null;
        }

        public bool UpdatePendingState(SimulatorDataSet set, DriverInfo driverInfo)
        {
            return false;
        }

        public void FinishLap(SimulatorDataSet dataSet, DriverInfo driverInfo)
        {
        }

        public bool SwitchToPendingIfNecessary(SimulatorDataSet set, DriverInfo driverInfo)
        {
            return false;
        }

        public void InvalidateLap(LapInvalidationReasonKind driverDnf)
        {

        }

        public void Tick(SimulatorDataSet dataSet, DriverInfo driverInfo)
        {

        }

        public bool IsLapDataSane(SimulatorDataSet dataSet)
        {
            return true;
        }

        public void OverrideTime(TimeSpan overrideLapTime)
        {
            LapTime = overrideLapTime;
        }
    }
}