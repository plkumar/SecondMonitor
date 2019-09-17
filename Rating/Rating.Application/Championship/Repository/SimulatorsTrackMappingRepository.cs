namespace SecondMonitor.Rating.Application.Championship.Repository
{
    using Common.DataModel.Championship.TrackMapping;
    using SecondMonitor.ViewModels.Repository;
    using SecondMonitor.ViewModels.Settings;

    public class SimulatorsTrackMappingRepository : AbstractXmlRepository<SimulatorsTrackMapping>, ISimulatorsTrackMappingRepository
    {
        private readonly ISettingsProvider _settingsProvider;

        public SimulatorsTrackMappingRepository(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        protected override string RepositoryDirectory => _settingsProvider.ChampionshipRepositoryPath;
        protected override string FileName => "TrackMappings.xml";
    }
}