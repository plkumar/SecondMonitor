namespace SecondMonitor.Rating.Application.Rating.Controller.RaceObserver.States
{
    using System.Diagnostics;
    using Context;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using DataModel.Summary;
    using NLog;
    using RatingProvider.FieldRatingProvider.ReferenceRatingProviders;
    using SecondMonitor.ViewModels.Settings;

    public class IdleState : AbstractSessionTypeState
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Stopwatch _stateDuration;

        public IdleState(SharedContext sharedContext, IReferenceRatingProviderFactory referenceRatingProviderFactory, ISettingsProvider settingsProvider) : base(sharedContext, referenceRatingProviderFactory, settingsProvider)
        {
            _stateDuration = Stopwatch.StartNew();
        }

        public override SessionKind SessionKind { get; protected set; } = SessionKind.Idle;
        public override SessionPhaseKind SessionPhaseKind { get; protected set; } = SessionPhaseKind.None;

        public override bool ShowRatingChange => true;

        public override bool CanUserSelectClass => true;

        protected override void Initialize(SimulatorDataSet simulatorDataSet)
        {
        }

        public override bool DoDataLoaded(SimulatorDataSet simulatorDataSet)
        {
            if (_stateDuration.ElapsedMilliseconds > 7000 && !IsStateInitialized)
            {
                _stateDuration.Stop();
                if (!simulatorDataSet.SimulatorSourceInfo.NAStateBetweenSessions)
                {
                    Logger.Info("Idle state for 7seconds - clearing race and qualification context");
                    SharedContext.QualificationContext = null;
                    SharedContext.RaceContext = null;

                }
                IsStateInitialized = true;
            }
            return simulatorDataSet.SessionInfo.SessionType != SessionType && base.DoDataLoaded(simulatorDataSet);
        }

        public override bool DoSessionCompletion(SessionSummary sessionSummary)
        {
            return false;
        }

        protected override SessionType SessionType => SessionType.Na;

    }
}