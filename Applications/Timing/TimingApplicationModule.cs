namespace SecondMonitor.Timing
{
    using Contracts.TrackRecords;
    using Controllers;
    using LapTimings;
    using Ninject.Modules;
    using TrackRecords.Controller;

    public class TimingApplicationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITrackRecordsRepositoryFactory>().To<TrackRecordsRepositoryFactory>();
            Bind<ITrackRecordsController, ITrackRecordsProvider>().To<TrackRecordsController>().InSingletonScope();
            Bind<MapManagementController>().ToSelf().InSingletonScope();
            Bind<ISessionEventsController>().To<SessionEventsController>();
            Bind<DriverLapSectorsTrackerFactory>().ToSelf();
        }
    }
}