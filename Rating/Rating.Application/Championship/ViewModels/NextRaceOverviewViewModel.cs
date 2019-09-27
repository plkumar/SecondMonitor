namespace SecondMonitor.Rating.Application.Championship.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.DataModel.Championship;
    using Filters;
    using SecondMonitor.ViewModels;

    public class NextRaceOverviewViewModel : AbstractViewModel<ChampionshipDto>
    {
        private readonly List<IChampionshipRaceRequirement> _championshipMatches;
        private List<string> _textualRequirements;


        public NextRaceOverviewViewModel(IEnumerable<IChampionshipRaceRequirement> championshipMatches)
        {
            _championshipMatches = championshipMatches.ToList();
        }

        public List<string> TextualRequirements
        {
            get => _textualRequirements;
            set => SetProperty(ref _textualRequirements, value);
        }

        protected override void ApplyModel(ChampionshipDto model)
        {
            if (model == null)
            {
                TextualRequirements = new List<string>();
                return;
            }

            TextualRequirements = _championshipMatches.Select(x => x.GetDescription(model)).ToList();
        }

        public override ChampionshipDto SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}