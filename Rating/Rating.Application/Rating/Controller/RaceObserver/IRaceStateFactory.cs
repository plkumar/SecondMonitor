namespace SecondMonitor.Rating.Application.Rating.Controller.RaceObserver
{
    using SimulatorRating;
    using States;

    public interface IRaceStateFactory
    {
        IRaceState CreateInitialState(ISimulatorRatingController simulatorRatingController);
    }
}