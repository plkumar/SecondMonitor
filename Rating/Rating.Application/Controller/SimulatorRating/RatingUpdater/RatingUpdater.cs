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

        public void UpdateRatingsByResults(Dictionary<string, DriversRating> ratings, DriversRating simulatorRating, SessionFinishState sessionFinishState)
        {
            DriverFinishState player = sessionFinishState.DriverFinishStates.First(x => x.IsPlayer);
            DriverFinishState[] eligibleDrivers = sessionFinishState.DriverFinishStates.Where(x => !x.IsPlayer && ratings.ContainsKey(x.Name)).ToArray();
            var glickoRatingsClass = TransformToGlickoPlayers(ratings);
            var glickoRatingsSim = TransformToGlickoPlayers(ratings);
            var playerRating = glickoRatingsClass[player.Name];
            var playerSimRating = simulatorRating.ToGlicko(playerRating.Name);
            var opponentsClass = eligibleDrivers.Select(x => new GlickoOpponent(glickoRatingsClass[x.Name], x.FinishPosition < player.FinishPosition ? 0 : 1)).ToList();
            var opponentSim = eligibleDrivers.Select(x => new GlickoOpponent(glickoRatingsSim[x.Name], x.FinishPosition < player.FinishPosition ? 0 : 1)).ToList();
            ComputeNewRatingsAndNotify(playerRating, playerSimRating, opponentsClass,  opponentSim, player.CarClass, sessionFinishState.TrackName);
        }

        public void UpdateRatingsAsLoss(Dictionary<string, DriversRating> ratings, DriversRating simulatorRating, Driver player, string trackName)
        {
            var glickoRatingsClass = TransformToGlickoPlayers(ratings);
            var glickoRatingsSim = TransformToGlickoPlayers(ratings);
            var playerRating = glickoRatingsClass[player.DriverName];
            var playerSimRating = simulatorRating.ToGlicko(playerRating.Name);
            var opponentsClass = glickoRatingsClass.Where(x => x.Key != player.DriverName).Select(x => new GlickoOpponent(x.Value, 0)).ToList();
            var opponentsSim = glickoRatingsSim.Where(x => x.Key != player.DriverName).Select(x => new GlickoOpponent(x.Value, 0)).ToList();
            ComputeNewRatingsAndNotify(playerRating, playerSimRating, opponentsClass, opponentsSim, player.ClassName, trackName);
        }

        private void ComputeNewRatingsAndNotify(GlickoPlayer player, GlickoPlayer playerAsSimulator, List<GlickoOpponent> opponentsClass, List<GlickoOpponent> opponentsSim, string className, string trackName)
        {
            var newPlayerClassRating = GlickoCalculator.CalculateRanking(player, opponentsClass);
            var newPlayerSimRatingRating = GlickoCalculator.CalculateRanking(playerAsSimulator, opponentsSim);
            _ratingController.UpdateRating(newPlayerClassRating.FromGlicko(), newPlayerSimRatingRating.FromGlicko(), className, trackName);
        }

        private static Dictionary<string, GlickoPlayer> TransformToGlickoPlayers(Dictionary<string, DriversRating> ratings)
        {
            return ratings.Select(x => new KeyValuePair<string, GlickoPlayer>(x.Key, x.Value.ToGlicko(x.Key))).ToDictionary(x => x.Key, x=> x.Value);
        }
    }
}