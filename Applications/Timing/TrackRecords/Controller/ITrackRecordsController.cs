namespace SecondMonitor.Timing.TrackRecords.Controller
{
    using DataModel.Snapshot;
    using SessionTiming.Drivers.ViewModel;
    using ViewModels.Controllers;
    using ViewModels.TrackRecords;

    public interface ITrackRecordsController : IController
    {
        ITrackRecordsViewModel TrackRecordsViewModel { get; }

        void OnSessionStarted(SimulatorDataSet dataSet);

        void OnDataLoaded(SimulatorDataSet dataSet);

        bool EvaluateFastestLapCandidate(LapInfo lapInfo);
    }
}