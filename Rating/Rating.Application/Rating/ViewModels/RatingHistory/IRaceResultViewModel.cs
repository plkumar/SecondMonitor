namespace SecondMonitor.Rating.Application.Rating.ViewModels.RatingHistory
{
    using System;
    using Common.DataModel;
    using SecondMonitor.ViewModels;

    public interface IRaceResultViewModel : IViewModel<RaceResult>
    {
        string ClassName { get; set; }

        string CarName { get; set; }

        string TrackName { get; set; }

        int FinishingPosition { get; set; }

        int SimRatingChange { get; set; }

        int SimRatingAfterRace { get; set; }

        int ClassRatingChange { get; set; }

        int ClassRatingAfterRace { get; set; }

        DateTime CreationTime { get; set; }
        int Difficulty { get; }
    }
}