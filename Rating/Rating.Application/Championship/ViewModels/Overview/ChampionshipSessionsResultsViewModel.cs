namespace SecondMonitor.Rating.Application.Championship.ViewModels.Overview
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Common.DataModel.Championship;
    using Common.DataModel.Championship.Events;
    using Events;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class ChampionshipSessionsResultsViewModel : AbstractViewModel<ChampionshipDto>
    {
        private readonly IViewModelFactory _viewModelFactory;
        private ICommand _closeCommand;

        public ChampionshipSessionsResultsViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            ResultsHistory = new List<IViewModel>();
        }

        public List<IViewModel> ResultsHistory { get; }

        public ICommand CloseCommand
        {
            get => _closeCommand;
            set => SetProperty(ref _closeCommand, value);
        }

        protected override void ApplyModel(ChampionshipDto model)
        {
            ResultsHistory.Clear();
            foreach (EventDto eventDto in model.Events)
            {
                foreach (SessionDto sessionDto in eventDto.Sessions.Where(x => x.SessionResult != null))
                {
                    var sessionResultViewModel = _viewModelFactory.Create<SessionResultWithTitleViewModel>();
                    sessionResultViewModel.FromModel((model, eventDto, sessionDto));
                    ResultsHistory.Add(sessionResultViewModel);
                }
            }
        }

        public override ChampionshipDto SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}