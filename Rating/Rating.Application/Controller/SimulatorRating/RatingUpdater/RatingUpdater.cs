namespace SecondMonitor.Rating.Application.Controller.SimulatorRating.RatingUpdater
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.DataModel;
    using Common.DataModel.Player;
    using DataModel.Extensions;
    using DataModel.Summary;
    using Glicko2;
    using NLog;
    using NLog.Fluent;

    public class RatingUpdater : IRatingUpdater
    {
        private readonly ISimulatorRatingController _ratingController;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public RatingUpdater(ISimulatorRatingController ratingController)
        {
            _ratingController = ratingController;
        }

        public (DriversRating newSimulatorRating, DriversRating newClassRating, DriversRating newDifficultyRating) UpdateRatingsByResults(Dictionary<string, DriversRating> ratings, DriversRating difficultyRating, DriversRating simulatorRating, SessionFinishState sessionFinishState, int difficulty)
        {
            DriverFinishState player = sessionFinishState.DriverFinishStates.First(x => x.IsPlayer);
            Logger.Info("---- CALCULATING NEW CLASS RATINGS ---");
            GlickoPlayer newClassRating = CalculateNewRatingByResult(ratings, ratings[player.Name], sessionFinishState, player);
            Logger.Info("---- CALCULATING NEW SIM RATINGS ---");
            GlickoPlayer newSimRating = CalculateNewRatingByResult(ratings, simulatorRating, sessionFinishState, player);
            Logger.Info("---- CALCULATING NEW DIFFICULTY RATINGS ---");
            GlickoPlayer newDifficultyRating = CalculateNewRatingByResult(ratings, difficultyRating, sessionFinishState, player);

            ComputeNewRatingsAndNotify(newClassRating.FromGlicko(), newDifficultyRating.FromGlicko(), newSimRating.FromGlicko(), difficulty, player, sessionFinishState.TrackName);
            return (newSimRating.FromGlicko(), newClassRating.FromGlicko(), newDifficultyRating.FromGlicko());
        }

        public (DriversRating newSimulatorRating, DriversRating newClassRating, DriversRating newDifficultyRating) UpdateRatingsAsLoss(Dictionary<string, DriversRating> ratings, DriversRating difficultyRating, DriversRating simulatorRating,int difficulty, Driver player, string trackName)
        {
            DriverFinishState playerFinishState = new DriverFinishState(true, player.DriverName, player.CarName, player.ClassName, ratings.Count);
            GlickoPlayer newClassRating = CalculateNewAsLoss(ratings, ratings[player.DriverName], player);
            GlickoPlayer newSimRating = CalculateNewAsLoss(ratings, simulatorRating, player);

            ComputeNewRatingsAndNotify(newClassRating.FromGlicko(), difficultyRating, newSimRating.FromGlicko(), difficulty, playerFinishState, trackName);
            return (newSimRating.FromGlicko(), newClassRating.FromGlicko(), difficultyRating);
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
            Logger.Info("---- CALCULATING NEW RATINGS ---");
            Logger.Info($"Players Rating: {rating.Rating}-{rating.Deviation}-{rating.Volatility}");
            LogRatings(ratings);
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

        private static void LogRatings(Dictionary<string, DriversRating> ratings)
        {
            ratings.ForEach(x => Logger.Info($"Rating for {x.Key} : {x.Value.Rating} - {x.Value.Deviation} -  {x.Value.Volatility}"));
        }
    }
}