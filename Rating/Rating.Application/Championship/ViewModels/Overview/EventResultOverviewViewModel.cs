namespace SecondMonitor.Rating.Application.Championship.ViewModels.Overview
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.DataModel.Championship.Events;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;
    using SecondMonitor.ViewModels.Track;

    public class EventResultOverviewViewModel : AbstractViewModel<EventDto>
    {
        public EventResultOverviewViewModel(IViewModelFactory viewModelFactory)
        {
            TrackGeometryViewModel = viewModelFactory.Create<TrackGeometryViewModel>();
        }

        public TrackGeometryViewModel TrackGeometryViewModel { get; }

        public List<KeyValuePair<string, string>> SessionsResults { get; private set; }
        public string EventName { get; private set; }
        public EventStatus EventStatus { get; private set; }

        public string TrackName { get; private set; }


        protected override void ApplyModel(EventDto model)
        {
            EventStatus = model.EventStatus;
            EventName = model.EventName;
            TrackName = model.TrackName;
            SessionsResults = model.Sessions.Select(x =>
            {
                string sessionName = x.Name;
                if (x.SessionResult == null)
                {
                    return new KeyValuePair<string, string>(sessionName, "-");
                }

                DriverSessionResultDto playerResult = x.SessionResult.DriverSessionResult.FirstOrDefault( y => y.IsPlayer);
                return playerResult == null ? new KeyValuePair<string, string>(sessionName, "-") : new KeyValuePair<string, string>(sessionName, playerResult.FinishPosition.ToString());

            }).ToList();

        }

        public override EventDto SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}