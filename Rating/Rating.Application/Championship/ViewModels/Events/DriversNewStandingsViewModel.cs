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
            EventTitleViewModel = _viewModelFactory.Create<EventTitleViewModel>();
        }


        public EventTitleViewModel EventTitleViewModel { get; }

        public List<DriverNewStandingViewModel> DriversNewStandings { get; }

        protected override void ApplyModel(SessionResultDto model)
        {
            int totalGap = 0;
            int previousPoints = 0;
            bool isFirst = true;
            foreach (DriverSessionResultDto driverSessionResultDto in model.DriverSessionResult.OrderBy(x => x.AfterEventPosition))
            {
                var newDriverNewStanding = _viewModelFactory.Create<DriverNewStandingViewModel>();

                if (!isFirst)
                {
                    int gap = driverSessionResultDto.TotalPoints - previousPoints;
                    totalGap = gap + totalGap;
                    newDriverNewStanding.GapToPrevious = gap;
                    newDriverNewStanding.GapToLeader = totalGap;
                }

                previousPoints = driverSessionResultDto.TotalPoints;
                isFirst = false;

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