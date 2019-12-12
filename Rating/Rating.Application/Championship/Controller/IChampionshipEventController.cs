namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using Common.DataModel.Championship;
    using DataModel.Snapshot;
    using SecondMonitor.ViewModels.Controllers;

    public interface IChampionshipEventController : IChildController<IChampionshipController>
    {
        bool IsChampionshipActive { get; }
        ChampionshipDto CurrentChampionship { get; }
        void StartNextEvent(ChampionshipDto championship);
        bool TryResumePreviousChampionship(SimulatorDataSet dataSet);
    }
}