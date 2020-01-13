namespace SecondMonitor.ViewModels.CarStatus
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;
    using DataModel.Summary;

    public class SessionRemainingCalculator
    {
        private readonly IPaceProvider _paceProvider;
        private int _leaderTimeoutLap;
        private readonly Stopwatch _lastCalculationStopWatch;
        private readonly Stopwatch _lastDriverWithMostDistanceCalculation;
        private double? _lapsRemaining;
        private TimeSpan? _timeRemaining;
        private (DriverInfo driver, TimeSpan pace) _driverWithMostDistanceAtRaceEnd;

        public SessionRemainingCalculator(IPaceProvider paceProvider)
        {
            _paceProvider = paceProvider;
            _leaderTimeoutLap = -1;
            _lastCalculationStopWatch = Stopwatch.StartNew();
            _lastDriverWithMostDistanceCalculation = Stopwatch.StartNew();
        }


        private void Recalculate(SimulatorDataSet dataSet)
        {
            RecalculateTimeRemaining(dataSet);
            RecalculateLapsRemaining(dataSet);
            _lastCalculationStopWatch.Restart();
        }

        public TimeSpan GetTimeRemaining(SimulatorDataSet dataSet)
        {
            if (_lastCalculationStopWatch.ElapsedMilliseconds > 500 || !_timeRemaining.HasValue)
            {
                Recalculate(dataSet);

            }

            return _timeRemaining.GetValueOrDefault(TimeSpan.Zero);
        }

        public double GetLapsRemaining(SimulatorDataSet dataSet)
        {
            if (_lastCalculationStopWatch.ElapsedMilliseconds > 500 || !_lapsRemaining.HasValue)
            {
                Recalculate(dataSet);

            }

            return _lapsRemaining.GetValueOrDefault(0);
        }

        private void RecalculateTimeRemaining(SimulatorDataSet dataSet)
        {
            if (dataSet.SessionInfo.SessionLengthType == SessionLengthType.Time || dataSet.SessionInfo.SessionLengthType == SessionLengthType.TimeWithExtraLap)
            {
                _timeRemaining = TimeSpan.FromSeconds(dataSet.SessionInfo.SessionTimeRemaining);
            }
            else
            {
                _timeRemaining = _paceProvider.LeadersPace != null
                    ? TimeSpan.FromSeconds(GetLeaderLapsToGo(dataSet) * _paceProvider.LeadersPace.Value.TotalSeconds)
                    : TimeSpan.Zero;
            }
        }

        public void Reset()
        {
            _leaderTimeoutLap = -1;
            _timeRemaining = null;
            _lapsRemaining = null;
            _driverWithMostDistanceAtRaceEnd = (null, TimeSpan.Zero);
        }



        public void RecalculateLapsRemaining(SimulatorDataSet dataSet)
        {
            TimeSpan? playerPace = _paceProvider.PlayersPace;
            /*if (_driverWithMostDistanceAtRaceEnd.driver == null || _lastDriverWithMostDistanceCalculation.ElapsedMilliseconds > 20000)
            {*/
                _driverWithMostDistanceAtRaceEnd = GetDriverWithHigherDistanceAtRaceEnd(dataSet);
                //_lastDriverWithMostDistanceCalculation.Restart();
            //}
            TimeSpan driverWithMostDistancePace = _driverWithMostDistanceAtRaceEnd.pace;

            if (!playerPace.HasValue || playerPace.Value == TimeSpan.Zero || _driverWithMostDistanceAtRaceEnd.driver == null || driverWithMostDistancePace == TimeSpan.Zero)
            {
                _lapsRemaining = double.NaN;
                return;
            }

            if (dataSet.SessionInfo.SessionLengthType == SessionLengthType.Laps)
            {
                _lapsRemaining = GetLeaderLapsToGo(dataSet);
                return;
            }
            else
            {
                if (_leaderTimeoutLap == -1 && dataSet.SessionInfo.SessionTimeRemaining <= 0)
                {
                    _leaderTimeoutLap = dataSet.LeaderInfo.CompletedLaps;
                }
                double secondsTillSessionEnds = _driverWithMostDistanceAtRaceEnd.driver.IsPlayer ? dataSet.SessionInfo.SessionTimeRemaining : GetSecondsTillDriverFinished(dataSet, _driverWithMostDistanceAtRaceEnd.driver, driverWithMostDistancePace);
                double distanceToGo = (secondsTillSessionEnds / playerPace.Value.TotalSeconds) * dataSet.SessionInfo.TrackInfo.LayoutLength.InMeters;
                double distanceWithLapDistance = distanceToGo + dataSet.PlayerInfo.LapDistance;
                double distanceToFinishLap = dataSet.SessionInfo.TrackInfo.LayoutLength.InMeters - ((int)distanceWithLapDistance % (int)dataSet.SessionInfo.TrackInfo.LayoutLength.InMeters);
                double totalDistanceToGo = distanceToGo + distanceToFinishLap;

                if (dataSet.LeaderInfo.FinishStatus == DriverFinishStatus.Finished)
                {
                    totalDistanceToGo -= dataSet.SessionInfo.TrackInfo.LayoutLength.InMeters;
                }

                if (dataSet.SessionInfo.SessionLengthType == SessionLengthType.TimeWithExtraLap && (_leaderTimeoutLap == -1 || _leaderTimeoutLap == dataSet.LeaderInfo.CompletedLaps))
                {
                    totalDistanceToGo += dataSet.SessionInfo.TrackInfo.LayoutLength.InMeters;
                }

                _lapsRemaining = totalDistanceToGo / dataSet.SessionInfo.TrackInfo.LayoutLength.InMeters;
            }
        }

        private double CalculateTotalDistanceByRaceEnd(DriverInfo driver, TimeSpan? driversPace, SimulatorDataSet dataSet)
        {
            if (!driversPace.HasValue)
            {
                return 0;
            }
            return driver.TotalDistance + (dataSet.SessionInfo.SessionTimeRemaining / driversPace.Value.TotalSeconds) * dataSet.SessionInfo.TrackInfo.LayoutLength.InMeters;
        }

        private (DriverInfo driver, TimeSpan pace) GetDriverWithHigherDistanceAtRaceEnd(SimulatorDataSet dataSet)
        {
            var driverPaceMap = _paceProvider.GetPaceForDriversMap();
            var driversPaceWithDistanceTraveled = dataSet.DriversInfo.OrderBy(x => x.Position).Where(x => driverPaceMap.ContainsKey(x.DriverName)).Select(x => (driver:x, totalDistance:CalculateTotalDistanceByRaceEnd(x, driverPaceMap[x.DriverName], dataSet))).Where(x => !double.IsInfinity(x.totalDistance)).ToList();
            if (driversPaceWithDistanceTraveled.Count == 0)
            {
                return (null, TimeSpan.Zero);
            }

            var driverWithMostDistance = driversPaceWithDistanceTraveled.OrderBy(x => x.totalDistance).Last();
            return (driverWithMostDistance.driver, driverPaceMap[driverWithMostDistance.driver.DriverName]);
        }

        private double GetSecondsTillDriverFinished(SimulatorDataSet dataSet, DriverInfo driver, TimeSpan driverPace)
        {
            double distanceToGo = (dataSet.SessionInfo.SessionTimeRemaining /
                                   driverPace.TotalSeconds) * dataSet.SessionInfo.TrackInfo.LayoutLength.InMeters;
            double distanceWithLapDistance = distanceToGo + driver.LapDistance;
            double distanceToFinishLap = dataSet.SessionInfo.TrackInfo.LayoutLength.InMeters - ((int)distanceWithLapDistance % (int)dataSet.SessionInfo.TrackInfo.LayoutLength.InMeters);
            double totalDistanceToGo = distanceToGo + distanceToFinishLap;
            double totalLapsToGo = totalDistanceToGo / dataSet.SessionInfo.TrackInfo.LayoutLength.InMeters;
            return totalLapsToGo * driverPace.TotalSeconds;
        }

        private double GetLeaderLapsToGo(SimulatorDataSet dataSet)
        {
            double fullLapsToGo = dataSet.SessionInfo.TotalNumberOfLaps - dataSet.SessionInfo.LeaderCurrentLap + 1;
            return fullLapsToGo - (dataSet.LeaderInfo.LapDistance / dataSet.SessionInfo.TrackInfo.LayoutLength.InMeters);
        }
    }
}