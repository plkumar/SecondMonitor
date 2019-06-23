namespace SecondMonitor.Rating.Application.Controller.SimulatorRating
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Configuration;
    using Common.DataModel;
    using Common.DataModel.Player;
    using Common.Repository;
    using NLog;
    using RatingProvider;

    public class SimulatorRatingController : ISimulatorRatingController
    {
        private const double GlickoDeviationC = 7.95;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IRatingRepository _ratingRepository;
        private readonly SimulatorRatingConfiguration _simulatorRatingConfiguration;
        private Ratings _ratings;
        private SimulatorRating _simulatorRating;

        public SimulatorRatingController(string simulatorName, IRatingRepository ratingRepository, ISimulatorRatingConfigurationProvider simulatorRatingConfigurationProvider)
        {
            SimulatorName = simulatorName;
            _ratingRepository = ratingRepository;
            _simulatorRatingConfiguration = simulatorRatingConfigurationProvider.GetRatingConfiguration(simulatorName);
        }

        public event EventHandler<RatingChangeArgs> ClassRatingChanged;
        public event EventHandler<RatingChangeArgs> SimulatorRatingChanged;
        public int MinimumAiDifficulty => _simulatorRatingConfiguration.MinimumAiLevel;
        public int MaximumAiDifficulty => _simulatorRatingConfiguration.MaximumAiLevel;

        public double AiTimeDifferencePerLevel => _simulatorRatingConfiguration.AiTimeDifferencePerLevel;

        public int RatingPerLevel => _simulatorRatingConfiguration.RatingPerLevel;

        public int QuickRaceAiRatingForPlace => _simulatorRatingConfiguration.QuickRaceAiRatingForPlace;

        public string SimulatorName { get; }

        public string LastPlayedClass => _simulatorRating.LastPlayerClass;


        public double AiRatingNoise => _simulatorRatingConfiguration.AiRatingNoise;

        public Task StartControllerAsync()
        {
            _ratings = _ratingRepository.LoadRatingsOrCreateNew();
            _simulatorRating = _ratings.SimulatorsRatings.FirstOrDefault(x => x.SimulatorName == SimulatorName);
            if (_simulatorRating != null)
            {
                return Task.CompletedTask;
            }

            _simulatorRating = CreateSimulatorRating();
            _ratings.SimulatorsRatings.Add(_simulatorRating);

            return Task.CompletedTask;
        }

        private SimulatorRating CreateSimulatorRating()
        {
            SimulatorRating simulatorRating = new SimulatorRating()
            {
                SimulatorName = SimulatorName,
                PlayersRating = new DriversRating()
                {
                    Rating = _simulatorRatingConfiguration.DefaultPlayerRating,
                    Deviation = _simulatorRatingConfiguration.DefaultPlayerDeviation,
                    Volatility = _simulatorRatingConfiguration.DefaultPlayerVolatility,
                    CreationTime = DateTime.Now,
                    Difficulty = GetSuggestedDifficulty(_simulatorRatingConfiguration.DefaultPlayerRating),
                }
            };

            Logger.Info($"Created Default Simulator Rating for {SimulatorName}");
            LogRating(simulatorRating.PlayersRating);
            return simulatorRating;
        }

        private ClassRating CreateClassRating(string className)
        {
            ClassRating classRating = new ClassRating()
            {
                ClassName = className,
                PlayersRating = new DriversRating()
                {
                    Rating = _simulatorRating.PlayersRating.Rating,
                    Deviation = _simulatorRating.PlayersRating.Deviation,
                    Volatility = _simulatorRating.PlayersRating.Volatility,
                    CreationTime = DateTime.Now,
                    Difficulty = _simulatorRating.PlayersRating.Difficulty,
                },
                DifficultyRating = new DriversRating()
                {
                    Rating = _simulatorRating.PlayersRating.Rating,
                    Deviation = _simulatorRating.PlayersRating.Deviation,
                    Volatility = _simulatorRating.PlayersRating.Volatility,
                    CreationTime = DateTime.Now,
                    Difficulty = _simulatorRating.PlayersRating.Difficulty,
                },
                DifficultySettings = new DifficultySettings()
                {
                    SelectedDifficulty = GetSuggestedDifficulty(_simulatorRating.PlayersRating.Rating),
                    WasUserSelected =  true,
                }

            };
            _simulatorRating.ClassRatings.Add(classRating);
            Logger.Info($"Created Rating for {className}");
            LogRating(classRating.PlayersRating);
            return  classRating;
        }

        public Task StopControllerAsync()
        {
            _ratingRepository.SaveRatings(_ratings);
            return Task.CompletedTask;
        }

        public DriversRating GetPlayerOverallRating()
        {
            return FillDifficulty(UpdateDeviation(_simulatorRating.PlayersRating));
        }

        public (DriversRating simRating, DriversRating difficultyRating) GetPlayerRating(string className)
        {
            ClassRating classRating = _simulatorRating.ClassRatings.FirstOrDefault(x => x.ClassName == className) ?? CreateClassRating(className);
            Logger.Info($"Retreived Players Rating for Class {className}");
            LogRating(classRating.PlayersRating);
            return (FillDifficulty(UpdateDeviation(classRating.PlayersRating)), FillDifficulty(UpdateDeviation(classRating.DifficultyRating)));
        }

        public void SetSelectedDifficulty(int difficulty, bool wasUserSelected, string className)
        {
            ClassRating classRating = GetOrCreateClassRating(className);
            classRating.DifficultySettings = new DifficultySettings()
            {
                SelectedDifficulty = difficulty,
                WasUserSelected = wasUserSelected
            };
        }

        public void UpdateRating(DriversRating newClassRating, DriversRating newDifficultyRating, DriversRating newSimRating, int difficulty, string trackName, DriverFinishState driverFinishState)
        {
            ClassRating classRating = GetOrCreateClassRating(driverFinishState.CarClass);
            DriversRating oldClassRating = classRating.PlayersRating;
            DriversRating oldDifficultyRating = classRating.DifficultyRating;
            DriversRating oldSimRating = _simulatorRating.PlayersRating;
            newSimRating = NormalizeRatingChange(oldSimRating, newSimRating);
            newClassRating = NormalizeRatingChange(oldClassRating, newClassRating);
            newSimRating = FillDifficulty(newSimRating);
            newClassRating = FillDifficulty(newClassRating);
            newDifficultyRating = NormalizeRatingChange(oldDifficultyRating, newDifficultyRating);
            newDifficultyRating = FillDifficulty(newDifficultyRating);
            classRating.PlayersRating = newClassRating;
            classRating.DifficultyRating = newDifficultyRating;
            _simulatorRating.PlayersRating = newSimRating;

            if (!classRating.DifficultySettings.WasUserSelected)
            {
                classRating.DifficultySettings.SelectedDifficulty = classRating.DifficultyRating.Difficulty;
            }

            _simulatorRating.LastPlayerClass = driverFinishState.CarClass;
            RaceResult result = new RaceResult()
            {
                CarName = driverFinishState.CarName,
                ClassName = driverFinishState.CarClass,
                CreationTime = DateTime.Now,
                FinishingPosition = driverFinishState.FinishPosition,
                TrackName = trackName,
                Difficulty = difficulty,
                SimulatorRatingChange = new RatingChange()
                {
                    RatingBeforeChange = oldSimRating.Rating,
                    RatingAfterChange =  newSimRating.Rating
                },
                ClassRatingChange= new RatingChange()
                {
                    RatingBeforeChange = oldClassRating.Rating,
                    RatingAfterChange = newClassRating.Rating
                },

            };
            _simulatorRating.Results.Add(result);
            _simulatorRating.Results = _simulatorRating.Results.Take(500).ToList();
            _ratingRepository.SaveRatings(_ratings);
            NotifyRatingsChanges(CreateChangeArgs(oldClassRating, classRating.PlayersRating, driverFinishState.CarClass), CreateChangeArgs(oldSimRating, _simulatorRating.PlayersRating, SimulatorName));
        }

        public int GetSuggestedDifficulty(int rating)
        {
            return _simulatorRatingConfiguration.GetDifficultyForRating(rating);
        }

        public DifficultySettings GetDifficultySettings(string className)
        {
            return GetOrCreateClassRating(className).DifficultySettings;
        }

        public int GetRatingForDifficulty(int aiDifficulty)
        {
            //return (int)(_simulatorRatingConfiguration.MinimumRating + (aiDifficulty - _simulatorRatingConfiguration.MinimumAiLevel + 0.5) * _simulatorRatingConfiguration.RatingPerLevel); - version with half level up for AI
            return _simulatorRatingConfiguration.MinimumRating + (aiDifficulty - _simulatorRatingConfiguration.MinimumAiLevel) * _simulatorRatingConfiguration.RatingPerLevel;
        }

        public IReadOnlyCollection<string> GetAllKnowClassNames()
        {
            return _simulatorRating.ClassRatings.Select(x => x.ClassName).ToList();
        }

        public DriverWithoutRating GetAiRating(string aiDriverName)
        {
            return new DriverWithoutRating()
            {
                Deviation = 80, Volatility = 0.06, Name = aiDriverName
            };
        }

        private static void LogRating(DriversRating driversRating)
        {
            Logger.Info($"Rating - {driversRating.Rating}, Deviation - {driversRating.Deviation}, Volatility - {driversRating.Volatility}");
        }

        private static RatingChangeArgs CreateChangeArgs(DriversRating oldRating, DriversRating newRating, string ratingName)
        {
            return new RatingChangeArgs(newRating, newRating.Rating - oldRating.Rating, newRating.Deviation - oldRating.Deviation, newRating.Volatility - oldRating.Volatility, ratingName);
        }

        private ClassRating GetOrCreateClassRating(string className)
        {
            return _simulatorRating.ClassRatings.FirstOrDefault(x => x.ClassName == className) ?? CreateClassRating(className);
        }

        private void NotifyRatingsChanges(RatingChangeArgs classRatingChange, RatingChangeArgs simRatingChange)
        {
            Logger.Info("New Simulator Rating:");
            LogRating(simRatingChange.NewRating);
            Logger.Info("New Class Rating:");
            LogRating(classRatingChange.NewRating);
            ClassRatingChanged?.Invoke(this, classRatingChange);
            SimulatorRatingChanged?.Invoke(this, simRatingChange);
        }

        private static DriversRating UpdateDeviation(DriversRating driversRating)
        {
            int daysOfInactivity = (int)Math.Floor((DateTime.Now - driversRating.CreationTime).TotalDays);
            double newDeviation = driversRating.Deviation;
            for (int i = 0; i < daysOfInactivity; i++)
            {
                newDeviation = Math.Min(350, Math.Sqrt(Math.Pow(newDeviation, 2) + Math.Pow(GlickoDeviationC, 2)));
            }

            driversRating.Deviation = (int) newDeviation;
            driversRating.Volatility = 0.06;
            return driversRating;
        }

        private DriversRating NormalizeRatingChange(DriversRating oldRating, DriversRating newRating)
        {
            newRating.Rating = Math.Max(newRating.Rating, _simulatorRatingConfiguration.MinimumRating);
            int maximumChange = _simulatorRatingConfiguration.RatingPerLevel * 5;
            int ratingDifference = newRating.Rating - oldRating.Rating;
            if (Math.Abs(ratingDifference) > maximumChange)
            {
                newRating.Rating = ratingDifference < 0 ? oldRating.Rating - maximumChange : oldRating.Rating + maximumChange;
            }

            return newRating;
        }

        private DriversRating FillDifficulty(DriversRating rating)
        {
            rating.Difficulty = GetSuggestedDifficulty(rating.Rating);
            return rating;
        }
    }
}