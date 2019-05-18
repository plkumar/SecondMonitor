namespace SecondMonitor.Rating.Application.Controller.SimulatorRating.RatingUpdater
{
    using System.Collections.Generic;
    using Common.DataModel;
    using Common.DataModel.Player;
    using DataModel.Summary;

    public interface IRatingUpdater
    {
        void UpdateRatingsByResults(Dictionary<string, DriversRating> ratings, DriversRating simulatorRating, SessionFinishState sessionFinishState);
        void UpdateRatingsAsLoss(Dictionary<string, DriversRating> ratingMap, DriversRating simulatorRating, Driver player, string trackName);
    }
}