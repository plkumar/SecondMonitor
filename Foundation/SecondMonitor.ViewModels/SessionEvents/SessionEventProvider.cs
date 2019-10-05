namespace SecondMonitor.ViewModels.SessionEvents
{
    using System;
    using System.Collections.Generic;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;

    public class SessionEventProvider : ISessionEventProvider
    {
        public event EventHandler<DataSetArgs> SessionTypeChange;
        public event EventHandler<DataSetArgs> PlayerFinishStateChanged;
        public event EventHandler<DriversArgs> DriversAdded;
        public event EventHandler<DriversArgs> DriversRemoved;
        public event EventHandler<DataSetArgs> TrackChanged;
        public event EventHandler<DataSetArgs> PlayerPropertiesChanged;

        public SimulatorDataSet LastDataSet { get; private set; }
        public SimulatorDataSet BeforeLastDataSet { get; private set; }

        public void NotifySessionTypeChanged(SimulatorDataSet dataSet)
        {
            SessionTypeChange?.Invoke(this, new DataSetArgs(dataSet));
        }

        public void NotifyPlayerFinishStateChanged(SimulatorDataSet dataSet)
        {
            PlayerFinishStateChanged?.Invoke(this, new DataSetArgs(dataSet));
        }

        public void NotifyDriversAdded(SimulatorDataSet dataSet, IEnumerable<DriverInfo> drivers)
        {
            DriversAdded?.Invoke(this, new DriversArgs(dataSet, drivers));
        }

        public void NotifyDriversRemoved(SimulatorDataSet dataSet, IEnumerable<DriverInfo> drivers)
        {
            DriversRemoved?.Invoke(this, new DriversArgs(dataSet, drivers));
        }

        public void NotifyTrackChanged(SimulatorDataSet dataSet)
        {
            TrackChanged?.Invoke(this, new DataSetArgs(dataSet));
        }

        public void NotifyPlayerPropertiesChanged(SimulatorDataSet dataSet)
        {
            PlayerPropertiesChanged?.Invoke(this, new DataSetArgs(dataSet));
        }

        public void SetLastDataSet(SimulatorDataSet dataSet)
        {
            BeforeLastDataSet = LastDataSet;
            LastDataSet = dataSet;
        }
    }
}