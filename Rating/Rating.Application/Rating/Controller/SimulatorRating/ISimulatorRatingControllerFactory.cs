namespace SecondMonitor.Rating.Application.Rating.Controller.SimulatorRating
{
    public interface ISimulatorRatingControllerFactory
    {
        bool IsSimulatorSupported(string simulatorName);
        ISimulatorRatingController CreateController(string simulatorName);
    }
}