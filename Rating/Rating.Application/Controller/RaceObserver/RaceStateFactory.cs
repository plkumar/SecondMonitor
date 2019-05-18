namespace SecondMonitor.Rating.Application.Controller.RaceObserver
{
    using System;
    using DataModel;
    using SimulatorRating;
    using States;
    using States.Context;

    public class RaceStateFactory : IRaceStateFactory
    {
        public IRaceState CreateInitialState(ISimulatorRatingController simulatorRatingController)
        {
            return new IdleState(new SharedContext() {SimulatorRatingController = simulatorRatingController});
        }
    }
}