namespace SecondMonitor.Timing.LapTimings
{
    using System;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using SessionTiming.Drivers.ViewModel;
    using ViewModels.SessionEvents;

    public class DriverLapSectorsTracker : IDriverLapSectorsTracker
    {
        private const int SectionLength = 100;
        private readonly DriverTiming _driverTiming;
        private readonly ISessionEventProvider _sessionEventProvider;
        private readonly TimeSpan[] _sections;
        private int _lastSectionIndex;

        public DriverLapSectorsTracker(DriverTiming driverTiming, ISessionEventProvider sessionEventProvider)
        {
            _driverTiming = driverTiming;
            _sessionEventProvider = sessionEventProvider;
            _lastSectionIndex = -1;
            _sections = new TimeSpan[(int)(sessionEventProvider.LastDataSet.SessionInfo.TrackInfo.LayoutLength.InMeters / SectionLength) + 1];
        }

        public void Update()
        {
            SimulatorDataSet lastDataSet = _sessionEventProvider.LastDataSet;
            if (lastDataSet.SessionInfo.SessionType != SessionType.Race && _driverTiming.DriverInfo.InPits)
            {
                return;
            }

            int currentSectionIndex = (int) _driverTiming.DriverInfo.LapDistance / SectionLength;
            if (currentSectionIndex == _lastSectionIndex)
            {
                return;
            }

            if (currentSectionIndex < _lastSectionIndex)
            {
                _lastSectionIndex++;
                for (int i = _lastSectionIndex; i < _sections.Length; i++)
                {
                    _sections[i] = lastDataSet.SessionInfo.SessionTime;
                }

                _lastSectionIndex = -1;
            }

            while (_lastSectionIndex != currentSectionIndex && _lastSectionIndex < _sections.Length)
            {
                _lastSectionIndex++;
                _sections[_lastSectionIndex] = lastDataSet.SessionInfo.SessionTime;
            }
        }

        public TimeSpan GetSectionTime(double lapDistance)
        {
            int currentSectionIndex = (int) lapDistance / SectionLength;
            return _sections[currentSectionIndex];
        }

        public TimeSpan GetRelativeGapToPlayer()
        {
            DriverTiming playerTiming = _driverTiming.Session?.Player;
            if (playerTiming == null)
            {
                return TimeSpan.Zero;
            }

            if (_driverTiming.DistanceToPlayer < 0)
            {
                return GetRelativeGapTo(_driverTiming, playerTiming);
            }
            else
            {
                return -GetRelativeGapTo(playerTiming, _driverTiming);
            }
        }

        public static TimeSpan GetRelativeGapTo(DriverTiming driverInFront, DriverTiming driverInBack)
        {
            double lapDistance = driverInBack.DriverInfo.LapDistance;
            TimeSpan driverInFrontTime = driverInFront.DriverLapSectorsTracker.GetSectionTime(lapDistance);
            TimeSpan driverInBackTime = driverInBack.DriverLapSectorsTracker.GetSectionTime(lapDistance);

            return driverInFrontTime - driverInBackTime;
        }
    }
}
