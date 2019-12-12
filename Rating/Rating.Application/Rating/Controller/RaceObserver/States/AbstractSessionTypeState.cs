namespace SecondMonitor.Rating.Application.Rating.Controller.RaceObserver.States
{
    using System;
    using System.Diagnostics;
    using Common.DataModel.Player;
    using Common.Factories;
    using Context;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using DataModel.Summary;
    using RatingProvider.FieldRatingProvider;
    using RatingProvider.FieldRatingProvider.ReferenceRatingProviders;
    using SecondMonitor.ViewModels.Settings;
    using SimulatorRating.RatingUpdater;

    public abstract class AbstractSessionTypeState : IRaceState
    {
        private readonly IReferenceRatingProviderFactory _referenceRatingProviderFactory;
        private readonly ISettingsProvider _settingsProvider;
        private readonly Stopwatch _stateStopwatch;
        protected AbstractSessionTypeState(SharedContext sharedContext, IReferenceRatingProviderFactory referenceRatingProviderFactory, ISettingsProvider settingsProvider)
        {
            _referenceRatingProviderFactory = referenceRatingProviderFactory;
            _settingsProvider = settingsProvider;
            SharedContext = sharedContext;
            SessionDescription = string.Empty;
            _stateStopwatch = Stopwatch.StartNew();
        }

        public TimeSpan ElapsedStateTime => _stateStopwatch.Elapsed;

        public abstract SessionKind SessionKind { get; protected set; }
        public abstract SessionPhaseKind SessionPhaseKind { get; protected set; }
        public IRaceState NextState { get; protected set; }
        public string SessionDescription { get; protected set; }

        public abstract bool ShowRatingChange { get; }

        public abstract bool CanUserSelectClass { get; }
        protected abstract SessionType SessionType { get; }
        public abstract bool DoSessionCompletion(SessionSummary sessionSummary);

        public SharedContext SharedContext { get; }

        protected abstract void Initialize(SimulatorDataSet simulatorDataSet);
        protected bool IsStateInitialized { get; set; }

        public virtual bool DoDataLoaded(SimulatorDataSet simulatorDataSet)
        {
            if (!IsStateInitialized && simulatorDataSet.SessionInfo.SessionPhase != SessionPhase.Unavailable)
            {
                Initialize(simulatorDataSet);
                IsStateInitialized = true;
            }

            if (simulatorDataSet.SessionInfo.SessionType != SessionType.Race)
            {
                SessionPhaseKind = simulatorDataSet.SessionInfo.SessionPhase == SessionPhase.Countdown ? SessionPhaseKind.NotStarted : SessionPhaseKind.InProgress;
            }

            if (simulatorDataSet.SessionInfo.SessionType == SessionType)
            {
                return false;
            }

            switch (simulatorDataSet.SessionInfo.SessionType)
            {
                case SessionType.Na:
                    NextState = new IdleState(SharedContext, _referenceRatingProviderFactory, _settingsProvider);
                    break;
                case SessionType.WarmUp:
                    NextState = new WarmupState(SharedContext, _referenceRatingProviderFactory, _settingsProvider);
                    break;
                case SessionType.Practice:
                    NextState = new PracticeState(SharedContext, _referenceRatingProviderFactory, _settingsProvider);
                    break;
                case SessionType.Qualification:
                    NextState = new QualificationState(SharedContext, _referenceRatingProviderFactory, _settingsProvider);
                    break;
                case SessionType.Race:
                    NextState = new RaceState(new QualificationResultRatingProvider(SharedContext.SimulatorRatingController, _referenceRatingProviderFactory), new RatingUpdater(SharedContext.SimulatorRatingController), new SessionFinishStateFactory(), SharedContext, _referenceRatingProviderFactory, _settingsProvider);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return true;
        }

        public virtual bool TryGetDriverRating(string driverName, out DriversRating driversRating)
        {
            driversRating = new DriversRating();
            return false;
        }
    }
}