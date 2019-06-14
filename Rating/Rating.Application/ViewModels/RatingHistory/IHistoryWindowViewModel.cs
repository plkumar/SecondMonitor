namespace SecondMonitor.Rating.Application.ViewModels.RatingHistory
{
    using System.Collections.ObjectModel;
    using Common.DataModel;
    using SecondMonitor.ViewModels;

    public interface IHistoryWindowViewModel : IViewModel<Ratings>
    {
        ObservableCollection<IRaceHistoriesViewModel> SimulatorHistories { get; set; }
    }
}