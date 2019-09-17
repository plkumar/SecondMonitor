namespace SecondMonitor.Rating.Application.Championship.Controller
{
    public interface ITrackTemplateToSimTrackMapper
    {
        bool TryGetSimulatorTrackName(string simulatorName, string templateTrackName, out string simulatorTrackName);
        void RegisterSimulatorTrackName(string simulatorName, string templateTrackName, string simulatorTrackName);
        void SaveTrackMappings();
    }
}