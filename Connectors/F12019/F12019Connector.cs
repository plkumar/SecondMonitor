namespace SecondMonitor.F12019Connector
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Datamodel;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using NLog;
    using PluginManager.GameConnector;

    public class F12019Connector : AbstractGameConnector
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private bool _isConnected;
        private readonly F12019UdpReceiver _udpReceiver;
        private CancellationTokenSource _udpCancellationTokenSource;
        private readonly F12019DataConvertor _f12019DataConvertor;
        private Task _udpReceiverTask;
        private SessionType _lastSessionType;
        private TimeSpan _lastSessionTime;

        public F12019Connector() : base(new []{"F1_2019", "F1_2019_dx12" })
        {
            _udpReceiver = new F12019UdpReceiver(OnSessionStarted, OnDataLoaded);
            _f12019DataConvertor = new F12019DataConvertor();
        }

        public override bool IsConnected => _isConnected;
        protected override string ConnectorName => F12019DataConvertor.ConnectorName;
        protected override void OnConnection()
        {
            _isConnected = true;
            _udpCancellationTokenSource = new CancellationTokenSource();
            _udpReceiverTask = _udpReceiver.ReceiveLoop(_udpCancellationTokenSource.Token);
        }

        protected override void ResetConnector()
        {

        }

        protected override async Task DaemonMethod(CancellationToken cancellationToken)
        {
            RaiseSessionStartedEvent(new SimulatorDataSet(F12019DataConvertor.ConnectorName));
            while (IsProcessRunning())
            {
                await Task.Delay(1000, cancellationToken);
            }

            _udpCancellationTokenSource.Cancel();
            try
            {
                await _udpReceiverTask;
            }
            catch (OperationCanceledException)
            {

            }

            Disconnect();
            RaiseDisconnectedEvent();
        }

        private void OnSessionStarted(AllPacketsComposition rawData)
        {
            try
            {
                var convertedData = _f12019DataConvertor.ConvertData(rawData);
                _lastSessionType = convertedData.SessionInfo.SessionType;
                _lastSessionTime = convertedData.SessionInfo.SessionTime;
                RaiseSessionStartedEvent(convertedData);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void OnDataLoaded(AllPacketsComposition rawData)
        {
            try
            {
                var convertedData = _f12019DataConvertor.ConvertData(rawData);
                if (_lastSessionType != convertedData.SessionInfo.SessionType && _lastSessionTime - convertedData.SessionInfo.SessionTime > TimeSpan.FromSeconds(5))
                {
                    RaiseSessionStartedEvent(convertedData);
                }
                else
                {
                    RaiseDataLoadedEvent(convertedData);
                }

                _lastSessionType = convertedData.SessionInfo.SessionType;
                _lastSessionTime = convertedData.SessionInfo.SessionTime;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void Disconnect()
        {
            _isConnected = false;
        }
    }
}