namespace SecondMonitor.ViewModels.SimulatorContent
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using DataModel;
    using DataModel.SimulatorContent;
    using DataModel.Snapshot;

    public class SimulatorContentController : ISimulatorContentController
    {
        private readonly ISimulatorContentRepository _simulatorContentRepository;
        private SimulatorsContent _simulatorsContent;
        private Stopwatch _stopwatch;

        private SimulatorContent _currentSimulatorContent;
        private string _lastCar;
        private string _lastTrack;

        public SimulatorContentController(ISimulatorContentRepository simulatorContentRepository)
        {
            _simulatorContentRepository = simulatorContentRepository;
            _lastCar = string.Empty;
            _lastTrack = string.Empty;
        }
        public Task StartControllerAsync()
        {
            _simulatorsContent = _simulatorContentRepository.LoadOrCreateSimulatorsContent();
            _stopwatch = Stopwatch.StartNew();
            return Task.CompletedTask;
        }

        public Task StopControllerAsync()
        {
            _simulatorContentRepository.SaveSimulatorContent(_simulatorsContent);
            return Task.CompletedTask;
        }

        public void Visit(SimulatorDataSet simulatorDataSet)
        {
            if (_stopwatch.ElapsedMilliseconds < 10000 || _simulatorsContent == null )
            {
                return;
            }

            _stopwatch.Restart();

            if (simulatorDataSet.PlayerInfo?.CarInfo == null || string.IsNullOrEmpty(simulatorDataSet.Source) || SimulatorsNameMap.IsNotConnected(simulatorDataSet.Source))
            {
                return;
            }

            if (_currentSimulatorContent == null || _currentSimulatorContent.SimulatorName != simulatorDataSet.Source)
            {
                SwitchSimulatorContent(simulatorDataSet.Source);
            }

            if (_lastCar != simulatorDataSet.PlayerInfo.CarName && !string.IsNullOrEmpty(simulatorDataSet.PlayerInfo.CarName))
            {
                _currentSimulatorContent.AddCar(simulatorDataSet.PlayerInfo.CarName, simulatorDataSet.PlayerInfo.CarClassName);
                _lastCar = simulatorDataSet.PlayerInfo.CarName;
            }

            if (_lastTrack != simulatorDataSet.SessionInfo.TrackInfo.TrackFullName && !string.IsNullOrEmpty(simulatorDataSet.SessionInfo.TrackInfo.TrackFullName) && simulatorDataSet.SessionInfo.TrackInfo.LayoutLength.InMeters > 0)
            {
                _currentSimulatorContent.AddTrack(simulatorDataSet.SessionInfo.TrackInfo.TrackFullName, simulatorDataSet.SessionInfo.TrackInfo.LayoutLength.InMeters);
                _lastTrack = simulatorDataSet.SessionInfo.TrackInfo.TrackFullName;
            }

        }

        private void SwitchSimulatorContent(string simulatorName)
        {
            _currentSimulatorContent = _simulatorsContent.GetOrCreateSimulatorContent(simulatorName);
            _currentSimulatorContent.Tracks.RemoveAll(x => x.LapDistance == 0);
            _lastCar = string.Empty;
            _lastTrack = string.Empty;
        }

        public void Reset()
        {

        }

        public IReadOnlyCollection<Track> GetAllTracksForSimulator(string simulatorName)
        {
            return _simulatorsContent == null ? Enumerable.Empty<Track>().ToList() : _simulatorsContent.GetOrCreateSimulatorContent(simulatorName).Tracks;
        }

        public IReadOnlyCollection<CarClass> GetAllCarClassesForSimulator(string simulatorName)
        {
            return _simulatorsContent == null ? Enumerable.Empty<CarClass>().ToList() : _simulatorsContent.GetOrCreateSimulatorContent(simulatorName).Classes;
        }
    }
}