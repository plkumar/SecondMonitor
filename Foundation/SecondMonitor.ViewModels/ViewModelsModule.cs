namespace SecondMonitor.ViewModels
{
    using Controllers;
    using Dialogs;
    using Factory;
    using Ninject.Modules;
    using PluginsSettings;
    using RaceSuggestion;
    using SessionEvents;
    using Settings;
    using SimulatorContent;
    using SplashScreen;
    using Track;
    using TrackRecords;
    using WheelDiameterWizard;

    public class ViewModelsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IViewModelFactory>().To<ViewModelFactory>();
            Bind<IChildControllerFactory>().To<ChildControllerFactory>();
            Bind<IWindowService>().To<WindowService>();
            Bind<IDialogService>().To<DialogService>();
            Bind<IBroadcastLimitSettingsViewModel>().To<BroadcastLimitSettingsViewModel>();
            Bind<IPluginConfigurationViewModel>().To<PluginConfigurationViewModel>();
            Bind<IPluginsConfigurationViewModel>().To<PluginsConfigurationViewModel>();
            Bind<IRemoteConfigurationViewModel>().To<RemoteConfigurationViewModel>();
            Bind<F12019ConfigurationViewModel>().ToSelf();
            Bind<ISettingsProvider>().To<AppDataSettingsProvider>().InSingletonScope();
            Bind<ISimulatorContentRepository>().To<StoredSimulatorContentRepository>().InSingletonScope();
            Bind<ISimulatorContentController>().To<SimulatorContentController>().InSingletonScope();

            Bind<ITrackRecordsViewModel>().To<TrackRecordsViewModel>();
            Bind<IRecordViewModel>().To<RecordViewModel>();

            Bind<IRaceSuggestionViewModel>().To<RaceSuggestionViewModel>();
            Bind<TrackRecordViewModel>().ToSelf();
            Bind<CarRecordViewModel>().ToSelf();
            Bind<SimulatorRecordsViewModel>().ToSelf();
            Bind<CarRecordsCollectionViewModel>().ToSelf();
            Bind<RecordEntryViewModel>().ToSelf();
            Bind<WelcomeStageViewModel>().ToSelf();
            Bind<AccelerationStageViewModel>().ToSelf();
            Bind<PreparationStageViewModel>().ToSelf();
            Bind<MeasurementPhaseViewModel>().ToSelf();
            Bind<ResultsStageViewModel>().ToSelf();
            Bind<SplashScreenViewModel>().ToSelf();
            Bind<TrackGeometryViewModel>().ToSelf();

            Bind<ISessionEventProvider>().To<SessionEventProvider>().InSingletonScope();

            Bind<YesNoDialogViewModel>().ToSelf();
        }
    }
}
