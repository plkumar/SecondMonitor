namespace SecondMonitor.Rating.Application.ViewModels.RatingHistory
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Common.DataModel;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class HistoryWindowViewModel : AbstractViewModel<Ratings>, IHistoryWindowViewModel
    {
        private readonly IViewModelFactory _viewModelFactory;

        public HistoryWindowViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            SimulatorHistories = new ObservableCollection<IRaceHistoriesViewModel>();
        }
        public ObservableCollection<IRaceHistoriesViewModel> SimulatorHistories { get; set; }

        protected override void ApplyModel(Ratings model)
        {
            SimulatorHistories.Clear();
            foreach (SimulatorRating modelSimulatorsRating in model.SimulatorsRatings.OrderBy(x => x.SimulatorName))
            {
                IRaceHistoriesViewModel newHistoryViewModel = _viewModelFactory.Create<IRaceHistoriesViewModel>();
                newHistoryViewModel.FromModel(modelSimulatorsRating.Results);
                newHistoryViewModel.Title = modelSimulatorsRating.SimulatorName;
                SimulatorHistories.Add(newHistoryViewModel);
            }
        }

        public override Ratings SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }


    }
}