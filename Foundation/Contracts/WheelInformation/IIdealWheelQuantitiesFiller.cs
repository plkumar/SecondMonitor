namespace SecondMonitor.Contracts.WheelInformation
{
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;

    public interface IIdealWheelQuantitiesFiller
    {
        void FillWheelIdealQuantities(SimulatorDataSet dataSet);
    }
}