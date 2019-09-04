namespace SecondMonitor.Rating.Application.Rating.ViewModels.RatingOverview
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Common.DataModel;
    using Contracts.Commands;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class RatingOverviewWindowViewModel : AbstractViewModel<Ratings>, IRatingOverviewWindowViewModel
    {
        private readonly IViewModelFactory _viewModelFactory;

        public RatingOverviewWindowViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            SimulatorRatings = new ObservableCollection<ISimulatorRatingsViewModel>();
        }
        public ObservableCollection<ISimulatorRatingsViewModel> SimulatorRatings { get; set; }

        public IRelayCommandWithParameter<(ISimulatorRatingsViewModel simulatorRating, IClassRatingViewModel classRating)> OpenClassHistoryCommand { get; set; }

        protected override void ApplyModel(Ratings model)
        {
            SimulatorRatings.Clear();
            foreach (SimulatorRating modelSimulatorsRating in model.SimulatorsRatings.OrderBy(x => x.SimulatorName))
            {
                ISimulatorRatingsViewModel newHistoryViewModel = _viewModelFactory.Create<ISimulatorRatingsViewModel>();
                newHistoryViewModel.FromModel(modelSimulatorsRating);
                newHistoryViewModel.OpenClassHistoryCommand = new RelayCommandWithParameter<IClassRatingViewModel>(x => OpenClassHistory(newHistoryViewModel, x));
                SimulatorRatings.Add(newHistoryViewModel);
            }
        }

        private void OpenClassHistory(ISimulatorRatingsViewModel simulatorRatingsViewModel, IClassRatingViewModel classRatingViewModel)
        {
            OpenClassHistoryCommand.Execute((simulatorRatingsViewModel, classRatingViewModel));
        }

        public override Ratings SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}