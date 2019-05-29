namespace SecondMonitor.ViewModels
{
    using DataModel.Snapshot;
    using DataModel.Snapshot.Systems;

    public interface IWheelStatusViewModel
    {
        void ApplyWheelCondition(SimulatorDataSet simulatorDataSet, WheelInfo wheelInfo);
        void Reset();
    }
}