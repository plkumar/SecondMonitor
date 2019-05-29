namespace SecondMonitor.ViewModels.RaceSuggestion
{
    using Controllers;
    using DataModel.Snapshot;

    public interface IRaceSuggestionController : IController, ISimulatorDataSetVisitor
    {
        IRaceSuggestionViewModel RaceSuggestionViewModel { get;}
    }
}