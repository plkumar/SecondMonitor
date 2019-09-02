namespace SecondMonitor.ManagePlugins
{
    using Ninject.Modules;
    using PluginsConfiguration.Common.Controller;
    using PluginsConfiguration.Common.Repository;

    public class ManagePluginsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPluginSettingsProvider>().To<PluginsSettingsProvider>().InSingletonScope();
            Bind<IPluginConfigurationRepository>().To<AppDataPluginConfigurationRepository>().InSingletonScope();
        }
    }
}