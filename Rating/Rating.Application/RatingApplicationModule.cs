﻿namespace SecondMonitor.Rating.Application
{
    using Championship.Controller;
    using Championship.Pool;
    using Championship.Repository;
    using Championship.ViewModels;
    using Championship.ViewModels.Creation;
    using Championship.ViewModels.Creation.Calendar;
    using Championship.ViewModels.Creation.Calendar.Predefined;
    using Championship.ViewModels.Creation.Session;
    using Championship.ViewModels.IconState;
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
            Bind<IChampionshipsRepository>().To<ChampionshipRepositoryTest>().InSingletonScope();
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

            Bind<CalendarTemplateGroupViewModel>().ToSelf();
            Bind<CalendarTemplateViewModel>().ToSelf();
            Bind<PredefinedCalendarSelectionViewModel>().ToSelf();

            Bind<SessionsDefinitionViewModel>().ToSelf();

            Bind<ICalendarEntryViewModelFactory>().To<CalendarEntryViewModelFactory>();
            Bind<ISessionDefinitionViewModelFactory>().To<SessionDefinitionViewModelFactory>();

        }
    }
}
