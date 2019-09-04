namespace SecondMonitor.Rating.Application.Rating.Controller.RaceObserver
{
    using RatingProvider.FieldRatingProvider.ReferenceRatingProviders;
    using SecondMonitor.ViewModels.Settings;
    using SimulatorRating;
    using States;
    using States.Context;

    public class RaceStateFactory : IRaceStateFactory
    {
        private readonly IReferenceRatingProviderFactory _referenceRatingProviderFactory;
        private readonly ISettingsProvider _settingsProvider;

        public RaceStateFactory(IReferenceRatingProviderFactory referenceRatingProviderFactory, ISettingsProvider settingsProvider)
        {
            _referenceRatingProviderFactory = referenceRatingProviderFactory;
            _settingsProvider = settingsProvider;
        }

        public IRaceState CreateInitialState(ISimulatorRatingController simulatorRatingController)
        {
            return new IdleState(new SharedContext() {SimulatorRatingController = simulatorRatingController}, _referenceRatingProviderFactory, _settingsProvider);
        }
    }
}