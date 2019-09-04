namespace SecondMonitor.Rating.Application.Rating.Controller.RaceObserver.States
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Common.DataModel.Player;
    using Common.Factories;
    using Context;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;
    using DataModel.Summary;
    using NLog;
    using RatingProvider.FieldRatingProvider;
    using RatingProvider.FieldRatingProvider.ReferenceRatingProviders;
    using SecondMonitor.ViewModels.Settings;
    using SimulatorRating.RatingUpdater;

    public class RaceState : AbstractSessionTypeState
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IQualificationResultRatingProvider _qualificationResultRatingProvider;
        private readonly IRatingUpdater _ratingUpdater;
        private readonly ISessionFinishStateFactory _finishStateFactory;
        private readonly int _graceLaps;
        private bool _isFlashing;
        private readonly Stopwatch _flashStopwatch;
        private bool _ratingComputed;
        private bool _wasMoving;

        public RaceState(IQualificationResultRatingProvider qualificationResultRatingProvider, IRatingUpdater ratingUpdater, ISessionFinishStateFactory finishStateFactory, SharedContext sharedContext, IReferenceRatingProviderFactory referenceRatingProviderFactory, ISettingsProvider settingsProvider) : base(sharedContext, referenceRatingProviderFactory, settingsProvider)
        {
            _qualificationResultRatingProvider = qualificationResultRatingProvider;
            _ratingUpdater = ratingUpdater;
            _finishStateFactory = finishStateFactory;
            _graceLaps = settingsProvider.DisplaySettingsViewModel.RatingSettingsViewModel.GraceLapsCount;
            _flashStopwatch = new Stopwatch();
            _ratingComputed = false;
            _wasMoving = false;
        }

        public override SessionKind SessionKind { get; protected set; } = SessionKind.RaceWithoutQualification;
        public override SessionPhaseKind SessionPhaseKind { get; protected set; }
        public override bool CanUserSelectClass => false;
        public override bool ShowRatingChange => _ratingComputed || SharedContext?.RaceContext?.WasQuit == true;

        protected override void Initialize(SimulatorDataSet simulatorDataSet)
        {
            _isFlashing = false;
            _ratingComputed = false;
            _wasMoving = false;
            DriverInfo[] eligibleDrivers = FilterEligibleDrivers(simulatorDataSet);
            if (CanUserPreviousRaceContext(eligibleDrivers))
            {
                Logger.Info("Using Previous Race Context");
                SessionKind = SharedContext.RaceContext.IsRatingBasedOnQualification ? SessionKind.RaceWithQualification : SessionKind.RaceWithoutQualification;
                SessionDescription = SharedContext.RaceContext.UsedDifficulty.ToString();
                return;
            }
            Logger.Info("Cannot use Race Context");
            int difficultyToUse = SharedContext.QualificationContext?.QualificationDifficulty ?? SharedContext.UserSelectedDifficulty;

            SharedContext.RaceContext = new RaceContext()
            {
                UsedDifficulty = difficultyToUse,
            };
            SessionDescription = difficultyToUse.ToString();

            if (CanUseQualification(eligibleDrivers) && SharedContext.QualificationContext != null)
            {
                Logger.Info("Can use qualification result");
                SessionKind = SessionKind.RaceWithQualification;
                SharedContext.RaceContext.IsRatingBasedOnQualification = true;
                SharedContext.RaceContext.FieldRating = _qualificationResultRatingProvider.CreateFieldRatingFromQualificationResult(SharedContext.QualificationContext.LastQualificationResult, difficultyToUse);
                SharedContext.QualificationContext = null;
            }
            else
            {
                Logger.Info("Cannot use qualification result");
                SharedContext.RaceContext.FieldRating = _qualificationResultRatingProvider.CreateFieldRating(eligibleDrivers, difficultyToUse);
            }
        }

       private DriverInfo[] FilterEligibleDrivers(SimulatorDataSet simulatorDataSetDataSet)
        {
             var eligibleDrivers = simulatorDataSetDataSet.SessionInfo.IsMultiClass ? simulatorDataSetDataSet.DriversInfo.Where(x => x.CarClassId == simulatorDataSetDataSet.PlayerInfo.CarClassId) : simulatorDataSetDataSet.DriversInfo;
             return eligibleDrivers.ToArray();
        }

        public override bool DoDataLoaded(SimulatorDataSet simulatorDataSet)
        {
            if (!IsStateInitialized && simulatorDataSet.DriversInfo.Any(x => string.IsNullOrWhiteSpace(x.DriverName)))
            {
                return false;
            }


            // Second Race - Session didn't change, but player state has
            if (_ratingComputed && simulatorDataSet.PlayerInfo.FinishStatus == DriverFinishStatus.None)
            {
                Initialize(simulatorDataSet);
            }

            if (_isFlashing)
            {
                Flash();
                return base.DoDataLoaded(simulatorDataSet);
            }

            if (simulatorDataSet.SessionInfo.SessionPhase == SessionPhase.Countdown)
            {
                SessionPhaseKind = SessionPhaseKind.NotStarted;
            }
            else if (!_wasMoving)
            {
                _wasMoving = simulatorDataSet.PlayerInfo.Speed.InMs > 5;
                SessionPhaseKind = SessionPhaseKind.NotStarted;
            }
            else if (simulatorDataSet.PlayerInfo.CompletedLaps < _graceLaps)
            {
                SessionPhaseKind = SessionPhaseKind.FreeRestartPeriod;
            }
            else
            {
                SessionPhaseKind = SessionPhaseKind.InProgress;
            }

            if (!_isFlashing && simulatorDataSet.PlayerInfo.FinishStatus == DriverFinishStatus.Finished && (simulatorDataSet.SessionInfo.SessionPhase == SessionPhase.Green || simulatorDataSet.SessionInfo.SessionPhase == SessionPhase.Checkered))
            {
                StartFlashing();
                ComputeRatingFromResults(simulatorDataSet);
                _ratingComputed = true;
            }

            return base.DoDataLoaded(simulatorDataSet);
        }

        private void Flash()
        {
            if (_flashStopwatch.ElapsedMilliseconds <= 800)
            {
                return;
            }
            _flashStopwatch.Restart();
            SessionKind = SessionKind == SessionKind.RaceWithQualification ? SessionKind.Idle : SessionKind.RaceWithQualification;
            SessionPhaseKind = SessionPhaseKind == SessionPhaseKind.None ? SessionPhaseKind.InProgress : SessionPhaseKind.None;
        }

        private void StartFlashing()
        {
            _isFlashing = true;
            _flashStopwatch.Start();
            SessionKind = SessionKind.RaceWithQualification;
            SessionPhaseKind = SessionPhaseKind.None;

        }

        public override bool DoSessionCompletion(SessionSummary sessionSummary)
        {

            Driver player = sessionSummary.Drivers.FirstOrDefault(x => x.IsPlayer);
            if (_ratingComputed || player == null)
            {
                return false;
            }

            _ratingComputed = true;

            if (player.FinishStatus == DriverFinishStatus.Finished)
            {
                ComputeRatingFromResults(sessionSummary);
                return false;
            }

            if (SessionPhaseKind == SessionPhaseKind.InProgress)
            {
                ComputeRatingAsLast(sessionSummary);
                SharedContext.RaceContext.WasQuit = true;
            }
            return false;
        }

        private void ComputeRatingAsLast(SessionSummary sessionSummary)
        {
            Driver player = sessionSummary.Drivers.FirstOrDefault(x => x.IsPlayer);
            if (player == null)
            {
                return;
            }
            var newRatings = _ratingUpdater.UpdateRatingsAsLoss(SharedContext.RaceContext.FieldRating, SharedContext.DifficultyRating, SharedContext.SimulatorRating, SharedContext.RaceContext.UsedDifficulty, player, sessionSummary.TrackInfo.TrackFullName);

        }

        private void ComputeRatingFromResults(SessionSummary sessionSummary)
        {
            var newRatings = _ratingUpdater.UpdateRatingsByResults(SharedContext.RaceContext.FieldRating, SharedContext.DifficultyRating, SharedContext.SimulatorRating,  _finishStateFactory.Create(sessionSummary), SharedContext.RaceContext.UsedDifficulty);
        }

        private void ComputeRatingFromResults(SimulatorDataSet simulatorDataSet)
        {
            var newRatings = _ratingUpdater.UpdateRatingsByResults(SharedContext.RaceContext.FieldRating, SharedContext.DifficultyRating, SharedContext.SimulatorRating,  _finishStateFactory.Create(simulatorDataSet), SharedContext.RaceContext.UsedDifficulty);
        }

        private bool CanUseQualification(DriverInfo[] eligibleDrivers)
        {
            if (SharedContext.QualificationContext?.LastQualificationResult == null)
            {
                Logger.Info("No previous qualification result");
                return false;
            }


            List<string> missingDrivers = SharedContext.QualificationContext.LastQualificationResult.Select(y => y.DriverName).Except(eligibleDrivers.Select(x => x.DriverName)).ToList();
            missingDrivers.ForEach(x => Logger.Info($"Missing rating qualification result for: {x}"));
            return missingDrivers.Count == 0;
        }

        private bool CanUserPreviousRaceContext(DriverInfo[] eligibleDrivers)
        {
            if(SharedContext.RaceContext?.FieldRating == null)
            {
                Logger.Info("No Previous Race Context");
                return false;
            }

            return eligibleDrivers.All(x => SharedContext.RaceContext.FieldRating.ContainsKey(x.DriverName));
        }

        protected override SessionType SessionType => SessionType.Race;

        public override bool TryGetDriverRating(string driverName, out DriversRating driversRating)
        {
            if (SharedContext?.RaceContext?.FieldRating != null)
            {
                return SharedContext.RaceContext.FieldRating.TryGetValue(driverName, out driversRating);
            }


            driversRating = new DriversRating();
            return false;

        }
    }
}