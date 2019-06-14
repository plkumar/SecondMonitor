namespace SecondMonitor.Rating.Application
{
    using Controller;
    using Controller.RaceObserver;
    using Controller.SimulatorRating;
    using Ninject.Modules;
    using ViewModels;
    using ViewModels.Rating;
    using ViewModels.RatingHistory;
    using ViewModels.RatingOverview;

    public class RatingApplicationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRatingApplicationController>().To<RatingApplicationController>();
            Bind<IRaceObserverController>().To<RaceObserverController>();
            Bind<IRatingStorageController>().To<RatingStorageController>();
            Bind<ISimulatorRatingControllerFactory>().To<SimulatorRatingControllerFactory>();
            Bind<ISimulatorRatingController>().To<SimulatorRatingController>();
            Bind<IRaceStateFactory>().To<RaceStateFactory>();

            Bind<IHistoryWindowViewModel>().To<HistoryWindowViewModel>();
            Bind<IRaceResultViewModel>().To<RaceResultViewModel>();
            Bind<IRaceHistoriesViewModel>().To<RaceHistoriesViewModel>();

            Bind<IRatingOverviewWindowViewModel>().To<RatingOverviewWindowViewModel>();
            Bind<ISimulatorRatingsViewModel>().To<SimulatorRatingsViewModel>();
            Bind<IClassRatingViewModel>().To<ClassRatingViewModel>();

            Bind<IRatingApplicationViewModel>().To<RatingApplicationViewModel>();
            Bind<IRatingViewModel>().To<RatingViewModel>();
        }
    }
}