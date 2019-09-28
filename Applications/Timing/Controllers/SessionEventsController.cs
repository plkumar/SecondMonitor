namespace SecondMonitor.Timing.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using NLog;
    using ViewModels.SessionEvents;

    public class SessionEventsController : ISessionEventsController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ISessionEventProvider _sessionEventProvider;
        private bool _isStarted;
        private SessionType _lastSessionType;
        private Stopwatch _periodicCheckStopwatch;
        private string _lastTrackName;
        private string _lastPlayersCar;
        private string _lastPlayersClass;

        public SessionEventsController(ISessionEventProvider sessionEventProvider)
        {
            _sessionEventProvider = sessionEventProvider;
        }

        public Task StartControllerAsync()
        {
            ResetProperties();
            _isStarted = true;
            _periodicCheckStopwatch = Stopwatch.StartNew();
            return Task.CompletedTask;
        }

        public Task StopControllerAsync()
        {
            _isStarted = false;
            return Task.CompletedTask;
        }

        public void Visit(SimulatorDataSet simulatorDataSet)
        {
            if (!_isStarted)
            {
                return;
            }

            if (_lastSessionType != simulatorDataSet.SessionInfo.SessionType)
            {
                _sessionEventProvider.NotifySessionTypeChanged(simulatorDataSet);
                _lastSessionType = simulatorDataSet.SessionInfo.SessionType;
                Logger.Info($"Session Type Change Detected : new session Type is {_lastSessionType}");
            }

            CheckPeriodicProperties(simulatorDataSet);
        }

        private void CheckPeriodicProperties(SimulatorDataSet simulatorDataSet)
        {
            if (_periodicCheckStopwatch.ElapsedMilliseconds < 1000)
            {
                return;
            }

            _periodicCheckStopwatch.Restart();

            if (_lastTrackName != simulatorDataSet.SessionInfo.TrackInfo.TrackFullName)
            {
                _sessionEventProvider.NotifyTrackChanged(simulatorDataSet);
                _lastTrackName = simulatorDataSet.SessionInfo.TrackInfo.TrackFullName;
                Logger.Info($"Track Change Detected : new Tracl is {_lastTrackName}");
            }

            if (simulatorDataSet.PlayerInfo != null)
            {
                CheckPlayerProperties(simulatorDataSet);
            }

        }

        private void CheckPlayerProperties(SimulatorDataSet simulatorDataSet)
        {
            if (_lastPlayersCar != simulatorDataSet.PlayerInfo.CarName || _lastPlayersClass != simulatorDataSet.PlayerInfo.CarClassName)
            {
                _sessionEventProvider.NotifyPlayerPropertiesChanged(simulatorDataSet);
                _lastPlayersCar = simulatorDataSet.PlayerInfo.CarName;
                _lastPlayersClass = simulatorDataSet.PlayerInfo.CarClassName;
                Logger.Info($"Players Car Change detected, new car is  {_lastPlayersCar}, of class {_lastPlayersClass}");
            }
        }

        private void ResetProperties()
        {
            _lastTrackName = string.Empty;
            _lastPlayersCar = string.Empty;
            _lastPlayersClass = string.Empty;
        }


        public void Reset()
        {
            if (!_isStarted)
            {
                return;
            }
            ResetProperties();
        }
    }
}