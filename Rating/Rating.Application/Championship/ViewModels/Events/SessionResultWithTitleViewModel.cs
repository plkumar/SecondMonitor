namespace SecondMonitor.Rating.Application.Championship.ViewModels.Events
{
    using Common.DataModel.Championship;
    using Common.DataModel.Championship.Events;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class SessionResultWithTitleViewModel : AbstractViewModel<(ChampionshipDto championship, EventDto eventDto, SessionDto sessionDto)>
    {
        public SessionResultWithTitleViewModel(IViewModelFactory viewModelFactory)
        {
            EventTitleViewModel = viewModelFactory.Create<EventTitleViewModel>();
            SessionResultViewModel = viewModelFactory.Create<SessionResultViewModel>();
        }

        public EventTitleViewModel EventTitleViewModel { get; }
        public SessionResultViewModel SessionResultViewModel { get; }
        protected override void ApplyModel((ChampionshipDto championship,  EventDto eventDto, SessionDto sessionDto) model)
        {
            var (championship, eventDto, sessionDto) = model;
            EventTitleViewModel.FromModel(model);
            SessionResultViewModel.FromModel(model.sessionDto.SessionResult);
        }

        public override (ChampionshipDto championship, EventDto eventDto, SessionDto sessionDto) SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}