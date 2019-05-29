namespace SecondMonitor.ViewModels.SimulatorContent
{
    using DataModel.SimulatorContent;

    public interface ISimulatorContentRepository
    {
        SimulatorsContent LoadOrCreateSimulatorsContent();

        void SaveSimulatorContent(SimulatorsContent simulatorsContent);
    }
}