namespace SecondMonitor.Timing.Controllers
{
    using DataModel.Snapshot;
    using ViewModels.Controllers;

    public interface ISessionEventsController : IController, ISimulatorDataSetVisitor
    {

    }
}