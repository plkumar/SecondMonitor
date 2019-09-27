namespace SecondMonitor.Rating.Application.Championship.Repository
{
    using Common.DataModel.Championship;
    using SecondMonitor.ViewModels.Repository;
    using SecondMonitor.ViewModels.Settings;

    public class ChampionshipFileRepository : AbstractXmlRepository<AllChampionshipsDto>, IChampionshipsRepository
    {
        public ChampionshipFileRepository(ISettingsProvider settingsProvider)
        {
            RepositoryDirectory = settingsProvider.ChampionshipRepositoryPath;
        }

        protected override string RepositoryDirectory { get; }
        protected override string FileName => "Championships.xml";
    }
}