namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System;
    using Common.DataModel.Championship;
    using SecondMonitor.ViewModels.Controllers;

    public interface IChampionshipCreationController : IChildController<IChampionshipOverviewController>
    {
        void TryFocusCreationWindow();
        void OpenChampionshipCreationDialog(Action<ChampionshipDto> newChampionshipCallback, Action cancellationCallback);
    }
}