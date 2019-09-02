namespace SecondMonitor.PluginsConfiguration.Common.DataModel
{
    using System.Collections.Generic;

    public class PluginsConfiguration
    {
        public PluginsConfiguration()
        {
            PluginsConfigurations = new List<PluginConfiguration>();
            F12019Configuration = new F12019Configuration();
        }

        public RemoteConfiguration RemoteConfiguration { get; set; }

        public F12019Configuration F12019Configuration { get; set; }
        public List<PluginConfiguration> PluginsConfigurations { get; set; }
    }
}