namespace SecondMonitor.Rating.Application.ViewModels.RatingOverview
{
    using System.Collections.ObjectModel;
    using Common.DataModel;
    using Contracts.Commands;
    using SecondMonitor.ViewModels;

    public interface IRatingOverviewWindowViewModel : IViewModel<Ratings>
    {
        ObservableCollection<ISimulatorRatingsViewModel> SimulatorRatings { get; set; }

        IRelayCommandWithParameter<(ISimulatorRatingsViewModel simulatorRating, IClassRatingViewModel classRating)> OpenClassHistoryCommand { get; set; }
    }
}