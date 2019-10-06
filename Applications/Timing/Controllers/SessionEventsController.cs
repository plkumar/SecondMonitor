namespace SecondMonitor.Timing.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;
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
        private DriverFinishStatus _lastPlayerFinishStatus;

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

            _sessionEventProvider.SetLastDataSet(simulatorDataSet);

            if (_lastSessionType != simulatorDataSet.SessionInfo.SessionType)
            {
                _sessionEventProvider.NotifySessionTypeChanged(simulatorDataSet);
                _lastSessionType = simulatorDataSet.SessionInfo.SessionType;
                Logger.Info($"Session Type Change Detected : new session Type is {_lastSessionType}");
            }

            CheckPeriodicProperties(simulatorDataSet);
        }


        public void Reset()
        {
            if (!_isStarted)
            {
                return;
            }
            ResetProperties();
        }


        private void CheckPeriodicProperties(SimulatorDataSet simulatorDataSet)
        {
            if (simulatorDataSet.PlayerInfo.FinishStatus != _lastPlayerFinishStatus)
            {
                _sessionEventProvider.NotifyPlayerFinishStateChanged(simulatorDataSet);
                _lastPlayerFinishStatus = simulatorDataSet.PlayerInfo.FinishStatus;
            }

            if (_periodicCheckStopwatch.ElapsedMilliseconds < 1000)
            {
                return;
            }

            _periodicCheckStopwatch.Restart();

            if (_lastTrackName != simulatorDataSet.SessionInfo.TrackInfo.TrackFullName)
            {
                _sessionEventProvider.NotifyTrackChanged(simulatorDataSet);
                _lastTrackName = simulatorDataSet.SessionInfo.TrackInfo.TrackFullName;
                Logger.Info($"Track Change Detected : new Track is {_lastTrackName}");
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
            _lastPlayerFinishStatus = DriverFinishStatus.Na;
        }
    }
}