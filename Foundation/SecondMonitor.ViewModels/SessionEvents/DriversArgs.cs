namespace SecondMonitor.ViewModels.SessionEvents
{
    using System.Collections.Generic;
    using System.Linq;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;

    public class DriversArgs : DataSetArgs
    {
        public DriversArgs(SimulatorDataSet dataSet, IEnumerable<DriverInfo> driverInfo) : base(dataSet)
        {
            DriverInfo = driverInfo.ToList();
        }

        public IReadOnlyCollection<DriverInfo> DriverInfo { get; }
    }
}