namespace SecondMonitor.PluginManager.GameConnector
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using DataModel.Snapshot;
    using NLog;

    public abstract class AbstractGameConnector : IGameConnector
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public event EventHandler<DataEventArgs> DataLoaded;

        public event EventHandler<EventArgs> ConnectedEvent;

        public event EventHandler<EventArgs> Disconnected;

        public event EventHandler<DataEventArgs> SessionStarted;

        public event EventHandler<MessageArgs> DisplayMessage;

        public abstract bool IsConnected { get; }

        public int TickTime { get; set; }

        private readonly string[] _executables;

        private Task _daemonTask;
        private CancellationTokenSource _cancellationTokenSource;
        private Stopwatch _lastCheck;

        protected string ProcessName { get; private set; }
        protected Process Process { get; private set; }

        protected AbstractGameConnector(string[] executables)
        {
            this._executables = executables;
            ProcessName = string.Empty;
            TickTime = 16;
        }

        protected bool ShouldDisconnect
        {
            get;
            set;
        }

        protected abstract string ConnectorName { get; }

        public bool IsProcessRunning()
        {
            if (!string.IsNullOrWhiteSpace(ProcessName))
            {
                if (_lastCheck.ElapsedMilliseconds < 5000)
                {
                    return true;
                }

                _lastCheck.Restart();
                if (Process.GetProcessesByName(ProcessName).Length > 0)
                {
                    return true;
                }

                Process = null;
                ProcessName = string.Empty;
                _lastCheck.Stop();
                return false;
            }

            foreach (var processName in _executables)
            {
                var processes = Process.GetProcessesByName(processName);
                if (processes.Length <= 0)
                {
                    continue;
                }

                Process = processes[0];
                ProcessName = processName;
                _lastCheck = Stopwatch.StartNew();
                return true;
            }

            return false;
        }

        public bool TryConnect()
        {
            return Connect();
        }

        public async Task FinnishConnectorAsync()
        {
            _cancellationTokenSource.Cancel();
            try
            {
                await _daemonTask;

            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error in connector");
            }

            _daemonTask = null;
        }

        private bool Connect()
        {
            if (!IsProcessRunning())
            {
                return false;
            }

            try
            {
                OnConnection();
                if (!IsConnected)
                {
                    return false;
                }

                RaiseConnectedEvent();
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }


        protected abstract void OnConnection();

        protected abstract void ResetConnector();

        protected abstract Task DaemonMethod(CancellationToken cancellationToken);


        public void StartConnectorLoop()
        {
            if (_daemonTask != null)
            {
                throw new InvalidOperationException("Daemon is already running");
            }

            _cancellationTokenSource = new CancellationTokenSource();
            ResetConnector();
            ShouldDisconnect = false;
            _daemonTask = DaemonMethod(_cancellationTokenSource.Token);

        }


        protected void SendMessageToClients(string message)
        {
            SendMessageToClients(message, null);
        }

        protected void SendMessageToClients(string message, Action action)
        {
            MessageArgs args = new MessageArgs(message, action);
            DisplayMessage?.Invoke(this, args);
        }

        protected void RaiseDataLoadedEvent(SimulatorDataSet data)
        {
            DataEventArgs args = new DataEventArgs(data);
            DataLoaded?.Invoke(this, args);
        }

        protected void RaiseConnectedEvent()
        {
            EventArgs args = new EventArgs();
            ConnectedEvent?.Invoke(this, args);
        }

        protected void RaiseDisconnectedEvent()
        {
            EventArgs args = new EventArgs();
            Disconnected?.Invoke(this, args);
        }

        protected void RaiseSessionStartedEvent(SimulatorDataSet data)
        {
            DataEventArgs args = new DataEventArgs(data);
            EventHandler<DataEventArgs> handler = SessionStarted;
            handler?.Invoke(this, args);
        }
    }
}