namespace SecondMonitor.Rating.Application.Rating.ViewModels.RatingHistory
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Common.DataModel;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class HistoryWindowViewModel : AbstractViewModel<Ratings>, IHistoryWindowViewModel
    {
        private readonly IViewModelFactory _viewModelFactory;
        private ICollection<IRaceHistoriesViewModel> _simulatorHistories;

        public HistoryWindowViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            SimulatorHistories = new ObservableCollection<IRaceHistoriesViewModel>();
        }
        public ICollection<IRaceHistoriesViewModel> SimulatorHistories
        {
            get => _simulatorHistories;
            set => SetProperty(ref _simulatorHistories, value);
        }

        protected override void ApplyModel(Ratings model)
        {
            List<IRaceHistoriesViewModel> newRaceHistoriesViewModels = new List<IRaceHistoriesViewModel>();
            foreach (SimulatorRating modelSimulatorsRating in model.SimulatorsRatings.OrderBy(x => x.SimulatorName))
            {
                IRaceHistoriesViewModel newHistoryViewModel = _viewModelFactory.Create<IRaceHistoriesViewModel>();
                newHistoryViewModel.FromModel(modelSimulatorsRating.Results);
                newHistoryViewModel.Title = modelSimulatorsRating.SimulatorName;
                newRaceHistoriesViewModels.Add(newHistoryViewModel);
            }

            SimulatorHistories = newRaceHistoriesViewModels;
        }

        public override Ratings SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }


    }
}