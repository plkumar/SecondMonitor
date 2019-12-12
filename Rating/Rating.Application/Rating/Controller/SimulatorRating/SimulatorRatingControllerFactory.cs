namespace SecondMonitor.Rating.Application.Rating.Controller.SimulatorRating
{
    using System;
    using System.Linq;
    using Ninject;
    using Ninject.Parameters;
    using Ninject.Syntax;

    public class SimulatorRatingControllerFactory : ISimulatorRatingControllerFactory
    {
        private readonly IResolutionRoot _resolutionRoot;
        public static readonly string[] SupportedSimulators = new[] {"R3E", "Assetto Corsa", "AMS", "RFactor 2", "PCars 2"};
        //private static readonly string[] SupportedSimulators = new[] { "R3E", "Assetto Corsa", "RFactor 2", "AMS" };

        public SimulatorRatingControllerFactory(IResolutionRoot resolutionRoot)
        {
            _resolutionRoot = resolutionRoot;
        }

        public bool IsSimulatorSupported(string simulatorName)
        {
            return SupportedSimulators.Contains(simulatorName);
        }

        public ISimulatorRatingController CreateController(string simulatorName)
        {
            if (!IsSimulatorSupported(simulatorName))
            {
                throw new ArgumentException($"Simulator : {simulatorName}, is not supported");
            }

            return _resolutionRoot.Get<ISimulatorRatingController>(new ConstructorArgument("simulatorName", simulatorName, false));
        }
    }
}