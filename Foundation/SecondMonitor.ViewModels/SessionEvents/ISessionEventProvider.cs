namespace SecondMonitor.ViewModels.SessionEvents
{
    using System;
    using System.Collections.Generic;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;

    public interface ISessionEventProvider
    {
        event EventHandler<DataSetArgs> SessionTypeChange;
        event EventHandler<DataSetArgs> PlayerFinished;
        event EventHandler<DriversArgs> DriversAdded;
        event EventHandler<DriversArgs> DriversRemoved;
        event EventHandler<DataSetArgs> TrackChanged;
        event EventHandler<DataSetArgs> PlayerPropertiesChanged;

        void NotifySessionTypeChanged(SimulatorDataSet dataSet);
        void NotifyPlayerFinished(SimulatorDataSet dataSet);
        void NotifyDriversAdded(SimulatorDataSet dataSet, IEnumerable<DriverInfo> drivers);
        void NotifyDriversRemoved(SimulatorDataSet dataSet, IEnumerable<DriverInfo> drivers);
        void NotifyTrackChanged(SimulatorDataSet dataSet);
        void NotifyPlayerPropertiesChanged(SimulatorDataSet dataSet);

    }
}