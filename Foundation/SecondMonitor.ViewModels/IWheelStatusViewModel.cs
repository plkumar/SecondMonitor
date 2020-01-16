namespace SecondMonitor.ViewModels
{
    using DataModel.Snapshot;
    using DataModel.Snapshot.Systems;

    public interface IWheelStatusViewModel
    {
        void ApplyWheelCondition(SimulatorDataSet dataSet, WheelInfo wheelInfo);
        void Reset();
    }
}