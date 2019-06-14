namespace SecondMonitor.Rating.Application.ViewModels.RatingHistory
{
    using System.Collections.Generic;
    using Common.DataModel;
    using SecondMonitor.ViewModels;

    public interface IRaceHistoriesViewModel : IViewModel<IEnumerable<RaceResult>>
    {
        string Title { get; set; }

        List<IRaceResultViewModel>  RaceResults { get; set; }
    }
}