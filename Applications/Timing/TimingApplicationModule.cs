namespace SecondMonitor.Timing
{
    using Controllers;
    using Ninject.Modules;
    using TrackRecords;
    using TrackRecords.Controller;

    public class TimingApplicationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITrackRecordsRepositoryFactory>().To<TrackRecordsRepositoryFactory>();
            Bind<ITrackRecordsController>().To<TrackRecordsController>().InSingletonScope();
            Bind<MapManagementController>().ToSelf().InSingletonScope();
        }
    }
}