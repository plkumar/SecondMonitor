namespace SecondMonitor.Rating.Application.Championship.ViewModels.Events
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.DataModel.Championship.Events;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class DriversNewStandingsViewModel : AbstractViewModel<SessionResultDto>
    {
        private readonly IViewModelFactory _viewModelFactory;

        public DriversNewStandingsViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            DriversNewStandings = new List<DriverNewStandingViewModel>();
        }

        public string ChampionshipName { get; set; }

        public string EventName { get; set; }

        public string EventIndex { get; set; }

        public string SessionName { get; set; }

        public string SessionIndex { get; set; }

        public List<DriverNewStandingViewModel> DriversNewStandings { get; }

        protected override void ApplyModel(SessionResultDto model)
        {
            foreach (DriverSessionResultDto driverSessionResultDto in model.DriverSessionResult.OrderBy(x => x.AfterEventPosition))
            {
                var newDriverNewStanding = _viewModelFactory.Create<DriverNewStandingViewModel>();
                newDriverNewStanding.FromModel(driverSessionResultDto);
                DriversNewStandings.Add(newDriverNewStanding);
            }
        }

        public override SessionResultDto SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}