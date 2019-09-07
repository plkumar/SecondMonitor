namespace SecondMonitor.ViewModels.SimulatorContent
{
    using System.Collections.Generic;
    using Controllers;
    using DataModel.SimulatorContent;
    using DataModel.Snapshot;

    public interface ISimulatorContentController : IController, ISimulatorDataSetVisitor
    {
        IReadOnlyCollection<Track> GetAllTracksForSimulator(string simulatorName);
    }
}