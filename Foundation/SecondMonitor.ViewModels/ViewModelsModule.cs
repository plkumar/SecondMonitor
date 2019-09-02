﻿namespace SecondMonitor.ViewModels
{
    using Factory;
    using Ninject.Modules;
    using PluginsSettings;
    using RaceSuggestion;
    using Settings;
    using SimulatorContent;
    using SplashScreen;
    using TrackRecords;
    using WheelDiameterWizard;

    public class ViewModelsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IViewModelFactory>().To<ViewModelFactory>();
            Bind<IWindowService>().To<WindowService>();
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
            Bind<WelcomeStageViewModel>().ToSelf();
            Bind<AccelerationStageViewModel>().ToSelf();
            Bind<PreparationStageViewModel>().ToSelf();
            Bind<MeasurementPhaseViewModel>().ToSelf();
            Bind<ResultsStageViewModel>().ToSelf();
            Bind<SplashScreenViewModel>().ToSelf();
        }
    }
}
