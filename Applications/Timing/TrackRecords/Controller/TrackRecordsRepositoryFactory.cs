namespace SecondMonitor.Timing.TrackRecords.Controller
{
    using ViewModels.Settings;

    public class TrackRecordsRepositoryFactory : ITrackRecordsRepositoryFactory
    {
        private readonly ISettingsProvider _settingsProvider;

        public TrackRecordsRepositoryFactory(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public TrackRecordsRepository Create()
        {
            return new TrackRecordsRepository(_settingsProvider.TrackRecordsPath);
        }
    }
}