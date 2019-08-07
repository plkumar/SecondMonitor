namespace SecondMonitor.PluginManager.Core
{
    using System.Threading.Tasks;

    public interface ISecondMonitorPlugin
    {
        PluginsManager PluginManager
        {
            get;
            set;
        }

        string PluginName { get; }
        bool IsEnabledByDefault { get; }

        Task RunPlugin();

        bool IsDaemon
        {
            get;
        }
    }
}
