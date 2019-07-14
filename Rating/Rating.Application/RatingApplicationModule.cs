namespace SecondMonitor.Rating.Application
{
    using Controller;
    using Controller.RaceObserver;
    using Controller.SimulatorRating;
    using Ninject.Modules;
    using RatingProvider.FieldRatingProvider.ReferenceRatingProviders;
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

            Bind<IReferenceRatingProviderFactory>().To<ReferenceRatingProviderFactory>();
            Bind<IReferenceRatingProvider>().To<LeadPackReferenceRatingProvider>().Named("Leading Group");
            Bind<IReferenceRatingProvider>().To<LeaderReferenceRatingProvider>().Named("Leader");
            Bind<IReferenceRatingProvider>().To<MidfieldReferenceRatingProvider>().Named("Midfield");
            Bind<IReferenceRatingProvider>().To<AverageTimeReferenceRatingProvider>().Named("Average Time");
            Bind<IReferenceRatingProvider>().To<LeaderPlus1ReferenceRatingProvider>().Named("Leader - 1%");
            Bind<IReferenceRatingProvider>().To<LeaderPlus3ReferenceRatingProvider>().Named("Leader - 3%");
        }
    }
}