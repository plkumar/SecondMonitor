namespace SecondMonitor.Rating.Application.Controller.SimulatorRating.RatingUpdater
{
    using System.Collections.Generic;
    using Common.DataModel;
    using Common.DataModel.Player;
    using DataModel.Summary;

    public interface IRatingUpdater
    {
        void UpdateRatingsByResults(Dictionary<string, DriversRating> ratings, DriversRating difficultyRating, DriversRating simulatorRating, SessionFinishState sessionFinishState, int difficulty);
        void UpdateRatingsAsLoss(Dictionary<string, DriversRating> ratingMap, DriversRating difficultyRating, DriversRating simulatorRating, int difficulty, Driver player, string trackName);
    }
}