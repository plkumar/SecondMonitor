namespace SecondMonitor.Rating.Common.Configuration
{
    using System;
    using System.Linq;

    public class SimulatorRatingConfigurationProvider : ISimulatorRatingConfigurationProvider
    {
        private readonly SimulatorsRatingConfiguration _simulatorsRatingConfiguration;

        public SimulatorRatingConfigurationProvider()
        {
            _simulatorsRatingConfiguration = new SimulatorsRatingConfiguration()
            {
                SimulatorRatingConfigurations = new[]
                {
                    new SimulatorRatingConfiguration()
                    {
                        SimulatorName = "R3E",
                        MinimumAiLevel = 80,
                        MaximumAiLevel = 120,
                        RatingPerLevel = 100,
                        DefaultPlayerRating = 1500,
                        DefaultPlayerDeviation = 350,
                        DefaultPlayerVolatility = 0.06,
                        MinimumRating = 100,
                        AiTimeDifferencePerLevel = 1,
                        AiRatingNoise = 25,
                        QuickRaceAiRatingForPlace = 10,
                    },
                    new SimulatorRatingConfiguration()
                    {
                        SimulatorName = "Assetto Corsa",
                        MinimumAiLevel = 70,
                        MaximumAiLevel = 100,
                        RatingPerLevel = 100,
                        DefaultPlayerRating = 1500,
                        DefaultPlayerDeviation = 350,
                        DefaultPlayerVolatility = 0.06,
                        MinimumRating = 100,
                        AiTimeDifferencePerLevel = 1,
                        AiRatingNoise = 25,
                        QuickRaceAiRatingForPlace = 20,
                    },
                    new SimulatorRatingConfiguration()
                    {
                        SimulatorName = "AMS",
                        MinimumAiLevel = 70,
                        MaximumAiLevel = 120,
                        RatingPerLevel = 100,
                        DefaultPlayerRating = 1500,
                        DefaultPlayerDeviation = 350,
                        DefaultPlayerVolatility = 0.06,
                        MinimumRating = 100,
                        AiTimeDifferencePerLevel = 1,
                        AiRatingNoise = 25,
                        QuickRaceAiRatingForPlace = 10,
                    },
                    new SimulatorRatingConfiguration()
                    {
                        SimulatorName = "RFactor 2",
                        MinimumAiLevel = 70,
                        MaximumAiLevel = 120,
                        RatingPerLevel = 100,
                        DefaultPlayerRating = 1500,
                        DefaultPlayerDeviation = 350,
                        DefaultPlayerVolatility = 0.06,
                        MinimumRating = 100,
                        AiTimeDifferencePerLevel = 1,
                        AiRatingNoise = 25,
                        QuickRaceAiRatingForPlace = 10,
                    },
                    new SimulatorRatingConfiguration()
                    {
                        SimulatorName = "PCars 2",
                        MinimumAiLevel = 60,
                        MaximumAiLevel = 120,
                        RatingPerLevel = 100,
                        DefaultPlayerRating = 1500,
                        DefaultPlayerDeviation = 350,
                        DefaultPlayerVolatility = 0.06,
                        MinimumRating = 100,
                        AiTimeDifferencePerLevel = 0.75,
                        AiRatingNoise = 25,
                        QuickRaceAiRatingForPlace = 20,
                    }
                }
            };
        }

        public SimulatorRatingConfiguration GetRatingConfiguration(string simulatorName)
        {
            return _simulatorsRatingConfiguration.SimulatorRatingConfigurations.First(x => x.SimulatorName == simulatorName);
        }
    }
}