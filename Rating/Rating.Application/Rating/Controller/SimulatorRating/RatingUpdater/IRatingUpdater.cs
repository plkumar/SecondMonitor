namespace SecondMonitor.Rating.Application.Rating.Controller.SimulatorRating.RatingUpdater
{
    using System.Collections.Generic;
    using Common.DataModel;
    using Common.DataModel.Player;
    using DataModel.Summary;

    public interface IRatingUpdater
    {
        (DriversRating newSimulatorRating, DriversRating newClassRating, DriversRating newDifficultyRating) UpdateRatingsByResults(Dictionary<string, DriversRating> ratings, DriversRating difficultyRating, DriversRating simulatorRating, SessionFinishState sessionFinishState, int difficulty);
        (DriversRating newSimulatorRating, DriversRating newClassRating, DriversRating newDifficultyRating) UpdateRatingsAsLoss(Dictionary<string, DriversRating> ratingMap, DriversRating difficultyRating, DriversRating simulatorRating, int difficulty, Driver player, string trackName);
    }
}