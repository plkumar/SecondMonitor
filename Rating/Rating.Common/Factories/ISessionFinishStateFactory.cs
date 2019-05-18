namespace SecondMonitor.Rating.Common.Factories
{
    using DataModel;
    using SecondMonitor.DataModel.Snapshot;
    using SecondMonitor.DataModel.Summary;

    public interface ISessionFinishStateFactory
    {
        SessionFinishState Create(SimulatorDataSet simulatorDataSet);
        SessionFinishState Create(SessionSummary sessionSummary);
    }
}