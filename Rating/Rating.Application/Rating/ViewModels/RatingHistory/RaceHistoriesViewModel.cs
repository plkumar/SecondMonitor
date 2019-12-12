namespace SecondMonitor.Rating.Application.Rating.ViewModels.RatingHistory
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.DataModel;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class RaceHistoriesViewModel : AbstractViewModel<IEnumerable<RaceResult>>, IRaceHistoriesViewModel
    {
        private readonly IViewModelFactory _viewModelFactory;

        public RaceHistoriesViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public string Title { get; set; }

        public List<IRaceResultViewModel> RaceResults { get; set; }

        protected override void ApplyModel(IEnumerable<RaceResult> model)
        {
            RaceResults = model.OrderByDescending(x => x.CreationTime).Select(x =>
            {
                var newRaceResultViewModel = _viewModelFactory.Create<IRaceResultViewModel>();
                newRaceResultViewModel.FromModel(x);
                return newRaceResultViewModel;
            }).ToList();
        }

        public override IEnumerable<RaceResult> SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }

    }
}