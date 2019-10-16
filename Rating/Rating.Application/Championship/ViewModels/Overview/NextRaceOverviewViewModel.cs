namespace SecondMonitor.Rating.Application.Championship.ViewModels.Overview
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Common.DataModel.Championship;
    using Filters;
    using SecondMonitor.ViewModels;

    public class NextRaceOverviewViewModel : AbstractViewModel<ChampionshipDto>
    {
        private readonly List<IChampionshipCondition> _championshipMatches;
        private bool _isDnfButtonVisible;
        private List<string> _textualRequirements;
        private ICommand _dnfSessionCommand;


        public NextRaceOverviewViewModel(IEnumerable<IChampionshipCondition> championshipMatches)
        {
            _championshipMatches = championshipMatches.ToList();
        }

        public List<string> TextualRequirements
        {
            get => _textualRequirements;
            set => SetProperty(ref _textualRequirements, value);
        }

        public bool IsDnfButtonVisible
        {
            get => _isDnfButtonVisible;
            set => SetProperty(ref _isDnfButtonVisible, value);
        }

        public ICommand DnfSessionCommand
        {
            get => _dnfSessionCommand;
            set => SetProperty(ref _dnfSessionCommand, value);
        }

        protected override void ApplyModel(ChampionshipDto model)
        {
            if (model == null)
            {
                TextualRequirements = new List<string>();
                IsDnfButtonVisible = false;
                return;
            }

            IsDnfButtonVisible = model.ChampionshipState == ChampionshipState.LastSessionCanceled;
            TextualRequirements = _championshipMatches.Select(x => x.GetDescription(model)).ToList();
        }

        public override ChampionshipDto SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}