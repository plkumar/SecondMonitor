namespace SecondMonitor.ViewModels.SimulatorContent
{
    using DataModel.SimulatorContent;
    using Repository;
    using Settings;

    public class StoredSimulatorContentRepository : AbstractXmlRepository<SimulatorsContent>, ISimulatorContentRepository
    {
        public StoredSimulatorContentRepository(ISettingsProvider settingsProvider)
        {
            RepositoryDirectory = settingsProvider.SimulatorContentRepository;
        }

        protected override string RepositoryDirectory { get; }

        protected override string FileName => "SimulatorsContent.xml";

        public void SaveSimulatorContent(SimulatorsContent simulatorsContent)
        {
            Save(simulatorsContent);
        }

        public SimulatorsContent LoadOrCreateSimulatorsContent()
        {
            return LoadRatingsOrCreateNew();
        }

    }
}