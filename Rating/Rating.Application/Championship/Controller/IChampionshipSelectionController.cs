namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System.Collections.Generic;
    using Common.DataModel.Championship;
    using SecondMonitor.ViewModels.Controllers;

    public interface IChampionshipSelectionController : IChildController<IChampionshipController>
    {
        void ShowOrFocusSelectionDialog(IEnumerable<ChampionshipDto> championships);
    }
}