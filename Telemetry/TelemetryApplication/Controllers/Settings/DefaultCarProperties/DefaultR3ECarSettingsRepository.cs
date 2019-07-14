namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.Settings.DefaultCarProperties
{
    using System;
    using System.IO;
    using System.Reflection;
    using SecondMonitor.ViewModels.Repository;
    using TelemetryApplication.Settings.DTO.DefaultCarProperties;

    public class DefaultR3ECarSettingsRepository : AbstractXmlRepository<DefaultR3ECarsProperties>
    {

        public DefaultR3ECarSettingsRepository()
        {
            RepositoryDirectory = AssemblyDirectory;
        }

        protected override string RepositoryDirectory { get; }

        protected override string FileName => "R3EDefaultProperties.xml";

        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}