namespace SecondMonitor.Telemetry.TelemetryApplication.Settings.DTO
{
    using System.Linq;
    using Controllers.Settings;
    using ViewModels.GraphPanel;

    public class StoredGraphsSettingsProvider : IGraphsSettingsProvider
    {
        private readonly IGraphsSettingsProvider _backupProvider;
        private readonly ISettingsController _settingsProvider;

        public StoredGraphsSettingsProvider(IGraphsSettingsProvider backupProvider, ISettingsController settingsProvider)
        {
            _backupProvider = backupProvider;
            _settingsProvider = settingsProvider;
        }

        public TelemetrySettingsDto TelemetrySettings => _settingsProvider.TelemetrySettings;

        public bool IsGraphViewModelLeft(IGraphViewModel graphViewModel)
        {
            return GetGraphSettings(graphViewModel).GraphLocation == GraphLocationKind.LeftPanel;
        }

        public bool IsGraphViewModelRight(IGraphViewModel graphViewModel)
        {
            return GetGraphSettings(graphViewModel).GraphLocation == GraphLocationKind.RightPanel;
        }

        public int GetGraphPriority(IGraphViewModel graphViewModel)
        {
            return GetGraphSettings(graphViewModel).GraphPriority;
        }

        private GraphSettingsDto GetGraphSettings(IGraphViewModel graphViewModel)
        {
            GraphSettingsDto graphSettings = TelemetrySettings.GraphSettings.FirstOrDefault(x => x.Title == graphViewModel.Title);
            if (graphSettings == null)
            {
                graphSettings = new GraphSettingsDto()
                {
                    Title = graphViewModel.Title,
                    GraphLocation = _backupProvider.IsGraphViewModelLeft(graphViewModel) ? GraphLocationKind.LeftPanel : GraphLocationKind.RightPanel,
                    GraphPriority = _backupProvider.GetGraphPriority(graphViewModel)
                };
                TelemetrySettings.GraphSettings.Add(graphSettings);
                _settingsProvider.SetTelemetrySettings(TelemetrySettings, RequestedAction.RefreshCharts);
            }

            return graphSettings;
        }
    }
}