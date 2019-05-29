namespace SecondMonitor.ViewModels
{
    using Factory;
    using Ninject.Modules;
    using PluginsSettings;
    using RaceSuggestion;
    using Settings;
    using SimulatorContent;

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
            Bind<ISettingsProvider>().To<AppDataSettingsProvider>().InSingletonScope();
            Bind<ISimulatorContentRepository>().To<StoredSimulatorContentRepository>().InSingletonScope();
            Bind<ISimulatorContentController>().To<SimulatorContentController>();

            Bind<IRaceSuggestionViewModel>().To<RaceSuggestionViewModel>();
        }
    }
}