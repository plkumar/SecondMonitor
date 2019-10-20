namespace SecondMonitor.Rating.Application
{
    using Championship.Controller;
    using Championship.Filters;
    using Championship.Operations;
    using Championship.Pool;
    using Championship.Repository;
    using Championship.ViewModels;
    using Championship.ViewModels.Creation;
    using Championship.ViewModels.Creation.Calendar;
    using Championship.ViewModels.Creation.Calendar.Predefined;
    using Championship.ViewModels.Creation.Session;
    using Championship.ViewModels.Creation.Session.SessionLength;
    using Championship.ViewModels.Events;
    using Championship.ViewModels.IconState;
    using Championship.ViewModels.Overview;
    using Championship.ViewModels.Selection;
    using Ninject.Modules;
    using Rating.Controller;
    using Rating.Controller.RaceObserver;
    using Rating.Controller.SimulatorRating;
    using Rating.RatingProvider.FieldRatingProvider.ReferenceRatingProviders;
    using Rating.ViewModels;
    using Rating.ViewModels.Rating;
    using Rating.ViewModels.RatingHistory;
    using Rating.ViewModels.RatingOverview;


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

            Bind<ISimulatorsTrackMappingRepository>().To<SimulatorsTrackMappingRepository>().InSingletonScope();
            Bind<ITrackTemplateToSimTrackMapper>().To<TrackTemplateToSimTrackMapper>().InSingletonScope();

            Bind<IChampionshipController>().To<ChampionshipController>();
            Bind<IChampionshipsRepository>().To<ChampionshipFileRepository>().InSingletonScope();
            Bind<IChampionshipOverviewController>().To<ChampionshipOverviewController>();
            Bind<IChampionshipsPool>().To<ChampionshipsPool>().InSingletonScope();
            Bind<IChampionshipCreationController>().To<ChampionshipCreationController>();
            Bind<ChampionshipIconStateViewModel>().ToSelf();

            Bind<ChampionshipsOverviewViewModel>().ToSelf();
            Bind<ChampionshipOverviewViewModel>().ToSelf();
            Bind<ChampionshipCreationViewModel>().ToSelf();
            Bind<CalendarDefinitionViewModel>().ToSelf();
            Bind<CreatedCalendarViewModel>().ToSelf();
            Bind<AvailableTracksViewModel>().ToSelf();
            Bind<EditableCalendarEntryViewModel>().ToSelf();
            Bind<ExistingTrackCalendarEntryViewModel>().ToSelf();
            Bind<GenericTrackTemplateViewModel>().ToSelf();
            Bind<CalendarPlaceholderEntryViewModel>().ToSelf();
            Bind<CalendarPreviewViewModel>().ToSelf();

            Bind<NextRaceOverviewViewModel>().ToSelf();

            Bind<CalendarTemplateGroupViewModel>().ToSelf();
            Bind<CalendarTemplateViewModel>().ToSelf();
            Bind<PredefinedCalendarSelectionViewModel>().ToSelf();

            Bind<SessionsDefinitionViewModel>().ToSelf();
            Bind<ChampionshipsSelectionViewModel>().ToSelf();

            Bind<EventStartingViewModel>().ToSelf();
            Bind<TrackOverviewViewModel>().ToSelf();

            Bind<DriverStandingViewModel>().ToSelf();
            Bind<StandingOverviewViewModel>().ToSelf();
            Bind<ChampionshipDetailViewModel>().ToSelf();

            Bind<SessionCompletedViewModel>().ToSelf();
            Bind<PodiumViewModel>().ToSelf();
            Bind<DriverFinishViewModel>().ToSelf();
            Bind<DriversFinishViewModel>().ToSelf();
            Bind<DriverNewStandingViewModel>().ToSelf();
            Bind<DriversNewStandingsViewModel>().ToSelf();

            Bind<ISessionLengthDefinitionViewModel>().To<TimeLengthDefinitionViewModel>();
            Bind<ISessionLengthDefinitionViewModel>().To<LapsLengthDefinitionViewModel>();
            Bind<ISessionLengthDefinitionViewModel>().To<DistanceLengthDefinitionViewModel>();

            Bind<ICalendarEntryViewModelFactory>().To<CalendarEntryViewModelFactory>();
            Bind<ISessionDefinitionViewModelFactory>().To<SessionDefinitionViewModelFactory>();

            Bind<IChampionshipFactory>().To<ChampionshipFactory>();

            Bind<IChampionshipCondition>().To<SimulatorRequirement>();
            Bind<IChampionshipCondition>().To<CarClassRequirement>();
            Bind<IChampionshipCondition>().To<TrackRequirement>();
            Bind<IChampionshipCondition>().To<DistanceRequirement>();
            Bind<IChampionshipCondition>().To<OpponentsRequirements>();
            Bind<IChampionshipEligibilityEvaluator>().To<ChampionshipEligibilityEvaluator>();

            Bind<IChampionshipEventController>().To<ChampionshipEventController>();
            Bind<IChampionshipSelectionController>().To<ChampionshipSelectionController>();

            Bind<IChampionshipManipulator>().To<ChampionshipManipulator>();

        }
    }
}
