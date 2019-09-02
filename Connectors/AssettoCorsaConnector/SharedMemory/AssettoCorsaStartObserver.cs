namespace SecondMonitor.AssettoCorsaConnector.SharedMemory
{
    using System;

    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using NLog;
    using NLog.Fluent;

    public class AssettoCorsaStartObserver
    {
        private enum StartState
        {
            Countdown, StartSequence, Started, WaitForLineCross, StartCompleted, StartRestartTimeout
        }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private StartState _startState;
        private SimulatorDataSet _lastDataSet;

        private TimeSpan _restartTimeoutEnd;

        public AssettoCorsaStartObserver()
        {
            ResetStartState();
        }

        public void Observe(SimulatorDataSet dataSet)
        {
            /*if (!ShouldObserve(dataSet))
            {
                _lastDataSet = dataSet;
                return;
            }

            CheckAndAdvanceState(dataSet);


            _lastDataSet = dataSet;*/
        }

        private void CheckAndAdvanceState(SimulatorDataSet dataSet)
        {
            if (dataSet.SessionInfo.SessionType != SessionType.Race && _startState != StartState.Countdown)
            {
                _startState = StartState.Countdown;
                return;
            }

            switch (_startState)
            {
                case StartState.Countdown:
                    CheckAndAdvanceCountdown(dataSet);
                    return;
                case StartState.StartSequence:
                    CheckAndAdvanceStartSequence(dataSet);
                    return;
                case StartState.Started:
                    CheckAndAdvanceStarted(dataSet);
                    return;
                case StartState.WaitForLineCross:
                    CheckAndAdvanceCrossedTheLine(dataSet);
                    return;
                case StartState.StartCompleted:
                    CheckAndAdvanceStartCompleted(dataSet);
                    return;
                case StartState.StartRestartTimeout:
                    CheckAndAdvanceStartRestartTimeout(dataSet);
                    return;
            }

        }

        private void CheckAndAdvanceCrossedTheLine(SimulatorDataSet dataSet)
        {
            if (dataSet.LeaderInfo.TotalDistance < 300)
            {
                Logger.Info("Leader distance less than 300 - crossed the line");
                _startState = StartState.Started;
            }
        }

        private void CheckAndAdvanceCountdown(SimulatorDataSet dataSet)
        {
            if ((dataSet.PlayerInfo != null && !dataSet.PlayerInfo.InPits) || dataSet.LeaderInfo.Speed > Velocity.Zero)
            {
                Logger.Info("Moving to Start Sequence");
                _startState = StartState.StartSequence;
            }
        }

        private void CheckAndAdvanceStartSequence(SimulatorDataSet dataSet)
        {
            if (dataSet.PlayerInfo != null && dataSet.PlayerInfo.InPits)
            {
                Logger.Info("Player in pits - moving to start sequence");
                _startState = StartState.Countdown;
                dataSet.SessionInfo.SessionType = SessionType.Na;
            }

            if (dataSet.LeaderInfo.Speed > Velocity.FromKph(2))
            {
                Logger.Info("Player moving - moving to started");
                _startState = StartState.WaitForLineCross;
                dataSet.SessionInfo.SessionType = SessionType.Na;
            }
        }

        private void CheckAndAdvanceStarted(SimulatorDataSet dataSet)
        {
            if (dataSet.PlayerInfo != null && dataSet.PlayerInfo.InPits)
            {
                Logger.Info("Player in pits - moving to start sequence");
                _startState = StartState.Countdown;
                dataSet.SessionInfo.SessionType = SessionType.Na;
            }

            if (dataSet.LeaderInfo.TotalDistance > 500)
            {
                Logger.Info("Leader moved more than 500m - moving to start completed");
                _startState = StartState.StartCompleted;
            }

        }

        private void CheckAndAdvanceStartCompleted(SimulatorDataSet dataSet)
        {
            if (dataSet.LeaderInfo.TotalDistance < 200)
            {
                Logger.Info("Leader completed less than 400m - moving to StartRestartTimeout");
                _startState = StartState.StartRestartTimeout;
                _restartTimeoutEnd = dataSet.SessionInfo.SessionTime.Add(TimeSpan.FromSeconds(3));
            }

        }

        private void CheckAndAdvanceStartRestartTimeout(SimulatorDataSet dataSet)
        {
            if (dataSet.LeaderInfo.TotalDistance < 400 && dataSet.SessionInfo.SessionTime > _restartTimeoutEnd)
            {
                _startState = StartState.Countdown;
                Logger.Info("Leader completed less than 400m - moving to countdown");
                dataSet.SessionInfo.SessionType = SessionType.Na;
            }

            if (dataSet.LeaderInfo.CompletedLaps > 1)
            {
                Logger.Info("Leader completed whole lap - moving to start completed");
                _startState = StartState.StartCompleted;
            }

        }

        private bool ShouldObserve(SimulatorDataSet dataSet)
        {
            return !(_lastDataSet == null || (dataSet.SessionInfo.SessionType != SessionType.Race && _startState == StartState.Countdown));
        }

        public void ResetStartState()
        {
            _startState = StartState.Countdown;
        }
    }
}