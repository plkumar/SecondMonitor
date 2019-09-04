namespace SecondMonitor.Rating.Application.Rating.ViewModels.RatingHistory
{
    using System.Collections.Generic;
    using Common.DataModel;
    using SecondMonitor.ViewModels;

    public interface IHistoryWindowViewModel : IViewModel<Ratings>
    {
        ICollection<IRaceHistoriesViewModel> SimulatorHistories { get; set; }
    }
}