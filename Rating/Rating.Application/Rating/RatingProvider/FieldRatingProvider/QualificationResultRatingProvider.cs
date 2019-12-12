namespace SecondMonitor.Rating.Application.Rating.RatingProvider.FieldRatingProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.DataModel.Player;
    using Controller.SimulatorRating;
    using DataModel.Snapshot.Drivers;
    using DataModel.Summary;
    using ReferenceRatingProviders;

    public class QualificationResultRatingProvider : IQualificationResultRatingProvider
    {
        private readonly ISimulatorRatingController _simulatorRatingController;
        private readonly IReferenceRatingProviderFactory _referenceRatingProviderFactory;
        private readonly int _maxNoise;
        private readonly Random _random;
        private readonly int _minimumRating;
        private TimeSpan _referenceTime;
        private int _referenceRating;

        public QualificationResultRatingProvider(ISimulatorRatingController simulatorRatingController, IReferenceRatingProviderFactory referenceRatingProviderFactory)
        {
            _random = new Random();
            _simulatorRatingController = simulatorRatingController;
            _referenceRatingProviderFactory = referenceRatingProviderFactory;
            _maxNoise = (int)(_simulatorRatingController.RatingPerLevel * _simulatorRatingController.AiRatingNoise / 100);
            _minimumRating = _simulatorRatingController.MinimumAiDifficulty;
        }

        public Dictionary<string, DriversRating> CreateFieldRatingFromQualificationResult(List<Driver> qualificationResult, int difficulty)
        {
            InitializeReferenceTime(qualificationResult);
            InitializeReferenceRating(difficulty);

            Dictionary<string, DriversRating> ratings = new Dictionary<string, DriversRating>();
            int lastRating = 0;
            foreach (Driver driver in qualificationResult.Where(x => x.IsPlayer || (x.BestPersonalLap != null && x.BestPersonalLap.LapTime != TimeSpan.Zero)))
            {
                if (driver.IsPlayer)
                {
                    ratings[driver.DriverName] = _simulatorRatingController.GetPlayerRating(driver.ClassName).simRating;
                    continue;
                }

                DriverWithoutRating aiDriver = _simulatorRatingController.GetAiRating(driver.DriverName);
                lastRating = ComputeRating(driver.BestPersonalLap.LapTime);
                ratings[driver.DriverName] = new DriversRating()
                {
                    Deviation = aiDriver.Deviation,
                    Volatility = aiDriver.Volatility,
                    Rating = lastRating
                };
            }

            foreach (Driver driver in qualificationResult.Where(x => !x.IsPlayer && (x.BestPersonalLap == null || x.BestPersonalLap.LapTime == TimeSpan.Zero)))
            {
                DriverWithoutRating aiDriver = _simulatorRatingController.GetAiRating(driver.DriverName);
                ratings[driver.DriverName] = new DriversRating()
                {
                    Deviation = aiDriver.Deviation,
                    Volatility = aiDriver.Volatility,
                    Rating = lastRating
                };
            }

            return ratings;
        }

        public Dictionary<string, DriversRating> CreateFieldRating(DriverInfo[] fieldDrivers, int difficulty)
        {
            InitializeReferenceRating(difficulty);
            fieldDrivers = fieldDrivers.OrderBy(x => x.Position).ToArray();
            Dictionary<string, DriversRating> ratings = new Dictionary<string, DriversRating>();
            int middleDriverIndex = _referenceRatingProviderFactory.CreateReferenceRatingProvider().GetReferenceDriverIndex(fieldDrivers.Length);
            int ratingBetweenPlaces = _simulatorRatingController.QuickRaceAiRatingForPlace;
            for(int i = 0; i < fieldDrivers.Length; i++)
            {
                DriverInfo driver = fieldDrivers[i];
                if (driver.IsPlayer)
                {
                    ratings[driver.DriverName] = _simulatorRatingController.GetPlayerRating(driver.CarClassName).simRating;
                    continue;
                }

                DriverWithoutRating aiDriver = _simulatorRatingController.GetAiRating(driver.DriverName);
                int rating = AddNoise(_referenceRating + (middleDriverIndex - i) * ratingBetweenPlaces);
                ratings[driver.DriverName] = new DriversRating()
                {
                    Deviation = aiDriver.Deviation,
                    Volatility = aiDriver.Volatility,
                    Rating = rating
                };
            }

            return ratings;
        }

        private int ComputeRating(TimeSpan driversTime)
        {
            double percentageDifference = 100 - (driversTime.TotalSeconds / _referenceTime.TotalSeconds) * 100;
            double levelsOfDifference = percentageDifference / _simulatorRatingController.AiTimeDifferencePerLevel;
            int rating = AddNoise((int)(_referenceRating + levelsOfDifference * _simulatorRatingController.RatingPerLevel));
            return rating;
        }

        private int AddNoise(int rating)
        {
            return Math.Max(rating  + _random.Next(_maxNoise) - _maxNoise / 2, _minimumRating);
        }

        private void InitializeReferenceRating(int difficulty)
        {
            _referenceRating = AddNoise(_simulatorRatingController.GetRatingForDifficulty(difficulty));
        }

        private void InitializeReferenceTime(List<Driver> drivers)
        {
            _referenceTime = _referenceRatingProviderFactory.CreateReferenceRatingProvider().GetReferenceTime(drivers);
        }
    }
}