namespace SecondMonitor.SimdataManagement
{
    using ViewModels.Settings;

    public class MapsLoaderFactory : IMapsLoaderFactory
    {
        private readonly ISettingsProvider _settingsProvider;

        public MapsLoaderFactory(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public MapsLoader Create()
        {
            return new MapsLoader(_settingsProvider.MapRepositoryPath);
        }
    }
}