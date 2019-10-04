namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using Common.DataModel.Championship;
    using DataModel.Snapshot;
    using SecondMonitor.ViewModels.Controllers;

    public interface IChampionshipEvenController : IChildController<IChampionshipController>
    {
        bool IsChampionshipActive { get; }
        void StartNextEvent(ChampionshipDto championship);
        bool TryResumePreviousChampionship(SimulatorDataSet dataSet);
    }
}