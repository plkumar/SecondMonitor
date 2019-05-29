namespace SecondMonitor.DataModel.Snapshot
{
    public interface ISimulatorDataSetVisitor
    {
        void Visit(SimulatorDataSet simulatorDataSet);
        void Reset();
    }
}