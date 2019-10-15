namespace SecondMonitor.Rating.Application.Championship.Operations
{
    using Common.DataModel.Championship;
    using DataModel.Snapshot;

    public interface IChampionshipManipulator
    {
        void StartChampionship(ChampionshipDto championship, SimulatorDataSet dataSet);
        void StartNextEvent(ChampionshipDto championship, SimulatorDataSet dataSet);
        void AddResultsForCurrentSession(ChampionshipDto championship, SimulatorDataSet dataSet);
        void UpdateAiDriversNames(ChampionshipDto championship, SimulatorDataSet dataSet);
    }
}