namespace SecondMonitor.Rating.Application.Championship.ViewModels.Events
{
    using Common.DataModel.Championship;
    using Common.DataModel.Championship.Events;
    using SecondMonitor.ViewModels;

    public class EventTitleViewModel : AbstractViewModel<(ChampionshipDto championship , EventDto eventDto, SessionDto sessionDto)>
    {
        public string ChampionshipName { get; set; }

        public string EventName { get; set; }

        public string EventIndex { get; set; }

        public string SessionName { get; set; }

        public string SessionIndex { get; set; }

        protected override void ApplyModel((ChampionshipDto championship, EventDto eventDto, SessionDto sessionDto) model)
        {
            (ChampionshipDto championship, EventDto eventDto, SessionDto sessionDto) = model;
            int eventIndex = championship.Events.IndexOf(eventDto);

            ChampionshipName = championship.ChampionshipName;
            EventName = eventDto.EventName;
            EventIndex = $"({eventIndex + 1} / {championship.Events.Count})";

            int sessionIndex = eventDto.Sessions.IndexOf(sessionDto);
            SessionName = sessionDto.Name;
            SessionIndex = $"({sessionIndex + 1} / {eventDto.Sessions.Count})";
        }

        public override (ChampionshipDto, EventDto, SessionDto) SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}