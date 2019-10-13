namespace SecondMonitor.Rating.Application.Championship.ViewModels.Events
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.DataModel.Championship;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class StandingOverviewViewModel : AbstractViewModel<IEnumerable<DriverDto>>
    {
        private readonly IViewModelFactory _viewModelFactory;

        public StandingOverviewViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            Header = "Current Standings";
        }

        public string Header { get; set; }
        public IReadOnlyCollection<DriverStandingViewModel> DriversStanding { get; private set; }

        protected override void ApplyModel(IEnumerable<DriverDto> model)
        {
            DriversStanding = model.OrderBy(x => x.Position).Select(x =>
            {
                var newViewModel = _viewModelFactory.Create<DriverStandingViewModel>();
                newViewModel.FromModel(x);
                return newViewModel;
            }).ToList();
        }

        public override IEnumerable<DriverDto> SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}