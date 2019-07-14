namespace SecondMonitor.Rating.Application.Controller.RaceObserver.States
{
    using Context;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using DataModel.Summary;
    using RatingProvider.FieldRatingProvider.ReferenceRatingProviders;
    using SecondMonitor.ViewModels.Settings;

    public class WarmupState : AbstractSessionTypeState
    {
        public override SessionKind SessionKind { get; protected set; } = SessionKind.OtherSession;

        public override SessionPhaseKind SessionPhaseKind { get; protected set; } = SessionPhaseKind.None;

        public override bool ShowRatingChange => true;

        public override bool CanUserSelectClass => false;

        public override bool DoSessionCompletion(SessionSummary sessionSummary)
        {
            return false;
        }

        protected override void Initialize(SimulatorDataSet simulatorDataSet)
        {

        }

        protected override SessionType SessionType => SessionType.WarmUp;

        public WarmupState(SharedContext sharedContext, IReferenceRatingProviderFactory referenceRatingProviderFactory, ISettingsProvider settingsProvider) : base(sharedContext, referenceRatingProviderFactory, settingsProvider)
        {
        }
    }
}