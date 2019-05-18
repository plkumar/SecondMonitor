namespace SecondMonitor.AssettoCorsaConnector
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using DataModel.Snapshot;
    using PluginManager.GameConnector;

    public class ContentManagerConnector : IGameConnector
    {
        private static readonly string[] AcExecutables = { "acs_x86", "acs" };
        private static readonly string[] CmExecutables = { "Content Manager" };
        private Task _daemonTask;
        private CancellationTokenSource _cancellationTokenSource;

        public event EventHandler<DataEventArgs> DataLoaded;
        public event EventHandler<EventArgs> ConnectedEvent;
        public event EventHandler<EventArgs> Disconnected;
        public event EventHandler<DataEventArgs> SessionStarted;
        public event EventHandler<MessageArgs> DisplayMessage;

        public bool IsConnected
        {
            get;
            private set;
        }

        public bool IsProcessRunning()
        {
            bool gameRunning = AcExecutables.Select(Process.GetProcessesByName).Any(y => y.Length > 0);
            bool cmRunning = CmExecutables.Select(Process.GetProcessesByName).Any(y => y.Length > 0);
            return !gameRunning && cmRunning;
        }

        public bool TryConnect()
        {
            return IsProcessRunning();
        }

        public async Task FinnishConnectorAsync()
        {
            _cancellationTokenSource.Cancel();
            try
            {
                await _daemonTask;
            }
            catch (TaskCanceledException)
            {

            }

            _daemonTask = null;
        }

        public void StartConnectorLoop()
        {
            if (_daemonTask != null)
            {
                throw new InvalidOperationException("Daemon is already running");
            }

            IsConnected = true;
            RaiseConnectedEvent();
            _cancellationTokenSource = new CancellationTokenSource();
            _daemonTask = LoopMethod(_cancellationTokenSource.Token);
        }

        private async Task LoopMethod(CancellationToken token)
        {
            while (IsProcessRunning())
            {
                await Task.Delay(1000, token);
                RaiseDataLoadedEvent(new SimulatorDataSet("Assetto Corsa"));
            }

            RaiseDisconnectedEvent();
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