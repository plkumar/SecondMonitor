﻿namespace SecondMonitor.PluginManager.GameConnector
{
    using System;

    using SecondMonitor.DataModel.Snapshot;

    public class DataEventArgs : EventArgs
    {

        public DataEventArgs(SimulatorDataSet data)
        {
            Data = data;
        }

        public SimulatorDataSet Data
        {
            get;
            set;
        }
    }

    public interface IGameConnector
    {
        event EventHandler<DataEventArgs> DataLoaded;

        event EventHandler<EventArgs> ConnectedEvent;

        event EventHandler<EventArgs> Disconnected;

        event EventHandler<DataEventArgs> SessionStarted;

        bool IsConnected
        {
            get;
        }

        int TickTime
        {
            get;
            set;
        }

        bool TryConnect();

        void ASyncConnect();

    }
}
