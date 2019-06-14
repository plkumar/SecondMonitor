namespace SecondMonitor.Rating.Application.Controller.SimulatorRating.RatingUpdater
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.DataModel;
    using Common.DataModel.Player;
    using DataModel.Summary;
    using Glicko2;

    public class RatingUpdater : IRatingUpdater
    {
        private readonly ISimulatorRatingController _ratingController;

        public RatingUpdater(ISimulatorRatingController ratingController)
        {
            _ratingController = ratingController;
        }

        public void UpdateRatingsByResults(Dictionary<string, DriversRating> ratings, DriversRating difficultyRating, DriversRating simulatorRating, SessionFinishState sessionFinishState, int difficulty)
        {
            DriverFinishState player = sessionFinishState.DriverFinishStates.First(x => x.IsPlayer);
            GlickoPlayer newClassRating = CalculateNewRatingByResult(ratings, ratings.First(x => x.Key == player.Name).Value, sessionFinishState, player);
            GlickoPlayer newSimRating = CalculateNewRatingByResult(ratings, simulatorRating, sessionFinishState, player);
            GlickoPlayer newDifficultyRating = CalculateNewRatingByResult(ratings, difficultyRating, sessionFinishState, player);

            ComputeNewRatingsAndNotify(newClassRating.FromGlicko(), newDifficultyRating.FromGlicko(), newSimRating.FromGlicko(), difficulty, player, sessionFinishState.TrackName);
        }

        public void UpdateRatingsAsLoss(Dictionary<string, DriversRating> ratings, DriversRating difficultyRating, DriversRating simulatorRating,int difficulty, Driver player, string trackName)
        {
            DriverFinishState playerFinishState = new DriverFinishState(true, player.DriverName, player.CarName, player.ClassName, ratings.Count);
            GlickoPlayer newClassRating = CalculateNewAsLoss(ratings, ratings.First(x => x.Key == player.DriverName).Value, player);
            GlickoPlayer newSimRating = CalculateNewAsLoss(ratings, simulatorRating, player);

            ComputeNewRatingsAndNotify(newClassRating.FromGlicko(), difficultyRating, newSimRating.FromGlicko(), difficulty, playerFinishState, trackName);
        }

        private GlickoPlayer CalculateNewAsLoss(Dictionary<string, DriversRating> ratings, DriversRating rating,  Driver player)
        {
            var playerRating = rating.ToGlicko(player.DriverName);
            var glickoRatings = TransformToGlickoPlayers(ratings);
            var opponents = glickoRatings.Where(x => x.Key != player.DriverName).Select(x => new GlickoOpponent(x.Value, 0)).ToList();
            return CalculateNewRating(playerRating, opponents);
        }

        private GlickoPlayer CalculateNewRatingByResult(Dictionary<string, DriversRating> ratings, DriversRating rating, SessionFinishState sessionFinishState, DriverFinishState player)
        {
            var playerRating = rating.ToGlicko(player.Name);
            var glickoRatings = TransformToGlickoPlayers(ratings);
            DriverFinishState[] eligibleDrivers = sessionFinishState.DriverFinishStates.Where(x => !x.IsPlayer && ratings.ContainsKey(x.Name)).ToArray();
            var opponents = eligibleDrivers.Select(x => new GlickoOpponent(glickoRatings[x.Name], x.FinishPosition < player.FinishPosition ? 0 : 1)).ToList();
            return CalculateNewRating(playerRating, opponents);
        }

        private GlickoPlayer CalculateNewRating(GlickoPlayer player, List<GlickoOpponent> opponents)
        {
            return GlickoCalculator.CalculateRanking(player, opponents);
        }

        private void ComputeNewRatingsAndNotify(DriversRating newPlayerClassRating, DriversRating newDifficultyRating, DriversRating newPlayerSimRatingRating, int difficulty, DriverFinishState playerFinishState, string trackName)
        {
            _ratingController.UpdateRating(newPlayerClassRating, newDifficultyRating, newPlayerSimRatingRating, difficulty, trackName, playerFinishState);
        }

        private static Dictionary<string, GlickoPlayer> TransformToGlickoPlayers(Dictionary<string, DriversRating> ratings)
        {
            return ratings.Select(x => new KeyValuePair<string, GlickoPlayer>(x.Key, x.Value.ToGlicko(x.Key))).ToDictionary(x => x.Key, x=> x.Value);
        }
    }
}