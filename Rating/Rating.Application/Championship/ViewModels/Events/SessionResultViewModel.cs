namespace SecondMonitor.Rating.Application.Championship.ViewModels.Events
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.DataModel.Championship.Events;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class SessionResultViewModel : AbstractViewModel<SessionResultDto>
    {
        private readonly IViewModelFactory _viewModelFactory;

        public SessionResultViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            DriversFinish = new List<DriverFinishViewModel>();
        }

        public string Header { get; set; }
        public List<DriverFinishViewModel> DriversFinish { get; private set; }

        protected override void ApplyModel(SessionResultDto model)
        {
            DriversFinish.Clear();
            foreach (DriverSessionResultDto driverSessionResultDto in model.DriverSessionResult.OrderBy(x => x.FinishPosition))
            {
                var newViewModel = _viewModelFactory.Create<DriverFinishViewModel>();
                newViewModel.FromModel(driverSessionResultDto);
                DriversFinish.Add(newViewModel);
            }
        }

        public override SessionResultDto SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}