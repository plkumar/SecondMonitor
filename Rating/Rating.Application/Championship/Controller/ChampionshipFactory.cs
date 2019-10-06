namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.DataModel.Championship;
    using Common.DataModel.Championship.TrackMapping;
    using ViewModels.Creation;
    using ViewModels.Creation.Calendar;
    using ViewModels.Creation.Session;

    public class ChampionshipFactory : IChampionshipFactory
    {
        public ChampionshipDto Create(ChampionshipCreationViewModel championshipCreationViewModel)
        {
            ChampionshipDto championship = new ChampionshipDto()
            {
                ChampionshipState = ChampionshipState.NotStarted,
                SimulatorName = championshipCreationViewModel.SelectedSimulator,
                ChampionshipName = championshipCreationViewModel.ChampionshipTitle,
                AiNamesCanChange = championshipCreationViewModel.AiNamesCanChange
            };

            FillEvents(championship, championshipCreationViewModel);
            FillScoring(championship, championshipCreationViewModel);
            championship.NextTrack = championship.Events[0].TrackName;
            championship.TotalEvents = championship.Events.Count * championship.Events[0].Sessions.Count;
            return championship;
        }

        private void FillEvents(ChampionshipDto championshipDto, ChampionshipCreationViewModel championshipCreationViewModel)
        {
            List<SessionDefinitionViewModel> sessionDefinitionViewModels = championshipCreationViewModel.SessionsDefinitionViewModel.SessionsDefinitions.ToList();
            championshipDto.Events = championshipCreationViewModel.CalendarDefinitionViewModel.CalendarViewModel.CalendarEntries.Select(x => CreateEvent(x, sessionDefinitionViewModels)).ToList();
        }

        public void FillScoring(ChampionshipDto championshipDto, ChampionshipCreationViewModel championshipCreationViewModel)
        {
            championshipDto.Scoring = championshipCreationViewModel.SessionsDefinitionViewModel.SessionsDefinitions.Select(x => new ScoringDto()
            {
                Scoring = x.Scoring.ToList(),
            }).ToList();
        }

        private EventDto CreateEvent(AbstractCalendarEntryViewModel calendarEntry, List<SessionDefinitionViewModel> sessionDefinitions)
        {
            EventDto newEventDto = new EventDto
            {
                EventName = calendarEntry.CustomEventName,
                TrackName = calendarEntry.TrackName,
                IsTrackNameExact = calendarEntry is ExistingTrackCalendarEntryViewModel,
                Sessions = sessionDefinitions.Select(x =>
                {
                    SessionDto sessionDto = new SessionDto() {DistanceDescription = x.SelectedSessionLengthDefinitionViewModel.GetTextualDescription(calendarEntry.LayoutLength), Name = x.CustomSessionName};
                    return sessionDto;
                }).ToList(),
            };

            return newEventDto;
        }
    }
}