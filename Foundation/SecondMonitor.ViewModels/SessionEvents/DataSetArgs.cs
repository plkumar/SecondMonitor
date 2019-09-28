namespace SecondMonitor.ViewModels.SessionEvents
{
    using System;
    using DataModel.Snapshot;

    public class DataSetArgs : EventArgs
    {
        public DataSetArgs(SimulatorDataSet dataSet)
        {
            DataSet = dataSet;
        }

        public SimulatorDataSet DataSet { get; }
    }
}