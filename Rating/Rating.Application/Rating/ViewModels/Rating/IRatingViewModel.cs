﻿namespace SecondMonitor.Rating.Application.Rating.ViewModels.Rating
{
    using Common.DataModel.Player;
    using SecondMonitor.ViewModels;

    public interface IRatingViewModel : IViewModel<DriversRating>
    {
        string SecondaryRating { get; }
        int RatingChange { get; set; }
        bool RatingChangeVisible { get; set; }
    }
}