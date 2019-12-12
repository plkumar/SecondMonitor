namespace SecondMonitor.ViewModels.SessionEvents
{
    using System;
    using System.Collections.Generic;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;

    public interface ISessionEventProvider
    {
        event EventHandler<DataSetArgs> SessionTypeChange;
        event EventHandler<DataSetArgs> PlayerFinishStateChanged;
        event EventHandler<DriversArgs> DriversAdded;
        event EventHandler<DriversArgs> DriversRemoved;
        event EventHandler<DataSetArgs> TrackChanged;
        event EventHandler<DataSetArgs> PlayerPropertiesChanged;

        SimulatorDataSet LastDataSet { get; }
        SimulatorDataSet BeforeLastDataSet { get; }

        void NotifySessionTypeChanged(SimulatorDataSet dataSet);
        void NotifyPlayerFinishStateChanged(SimulatorDataSet dataSet);
        void NotifyDriversAdded(SimulatorDataSet dataSet, IEnumerable<DriverInfo> drivers);
        void NotifyDriversRemoved(SimulatorDataSet dataSet, IEnumerable<DriverInfo> drivers);
        void NotifyTrackChanged(SimulatorDataSet dataSet);
        void NotifyPlayerPropertiesChanged(SimulatorDataSet dataSet);
        void SetLastDataSet(SimulatorDataSet dataSet);
    }
}