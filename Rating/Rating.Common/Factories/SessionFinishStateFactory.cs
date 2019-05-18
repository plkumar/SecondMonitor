namespace SecondMonitor.Rating.Common.Factories
{
    using System.Linq;
    using DataModel;
    using SecondMonitor.DataModel.Snapshot;
    using SecondMonitor.DataModel.Summary;

    public class SessionFinishStateFactory : ISessionFinishStateFactory
    {
        public SessionFinishState Create(SimulatorDataSet simulatorDataSet)
        {
            return new SessionFinishState(simulatorDataSet.SessionInfo.TrackInfo.TrackFullName, simulatorDataSet.DriversInfo.Select(x => new DriverFinishState(x.IsPlayer, x.DriverName, x.CarClassName, x.Position)));
        }

        public SessionFinishState Create(SessionSummary sessionSummary)
        {
            return new SessionFinishState(sessionSummary.TrackInfo.TrackFullName, sessionSummary.Drivers.Select(x => new DriverFinishState(x.IsPlayer, x.DriverName, x.ClassName, x.FinishingPosition)));
        }
    }
}