namespace SecondMonitor.Rating.Application.Championship.ViewModels.Selection
{
    using System.Collections.Generic;
    using Common.DataModel.Championship;
    using SecondMonitor.ViewModels;

    public class ChampionshipsSelectionViewModel : AbstractViewModel<IEnumerable<ChampionshipDto>>
    {
        protected override void ApplyModel(IEnumerable<ChampionshipDto> model)
        {
        }

        public override IEnumerable<ChampionshipDto> SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}