namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.MainWindow.GraphPanel
{
    using System.Linq;
    using DataModel.Extensions;
    using SecondMonitor.ViewModels.Settings;
    using Settings;
    using Synchronization;
    using Synchronization.Graphs;
    using TelemetryApplication.Settings;
    using ViewModels;
    using ViewModels.GraphPanel;

    public class LeftGraphPanelController : AbstractGraphPanelController
    {
        private readonly IGraphViewModelsProvider _graphViewModelsProvider;

        public LeftGraphPanelController(IGraphViewModelsProvider graphViewModelsProvider, IMainWindowViewModel mainWindowViewModel, ITelemetryViewsSynchronization telemetryViewsSynchronization, ILapColorSynchronization lapColorSynchronization,
            ISettingsProvider settingsProvider, IGraphViewSynchronization graphViewSynchronization, ISettingsController telemetrySettings)
            : base(mainWindowViewModel, telemetryViewsSynchronization, lapColorSynchronization, settingsProvider, graphViewSynchronization, telemetrySettings)
        {
            _graphViewModelsProvider = graphViewModelsProvider;
        }

        public override bool IsLetPanel => true;

        protected override IGraphViewModel[] Graphs { get; set; }

        protected override void ReloadGraphCollection()
        {
            IGraphViewModel[] newGraphs = _graphViewModelsProvider.GetLeftSideViewModels().OrderBy(x => x.priority).Select(y => y.graphViewModel).ToArray();
            MainWindowViewModel.ClearLeftPanelGraphs();
            Graphs?.ForEach(x => x.Dispose());
            Graphs = newGraphs;
            MainWindowViewModel.AddToLeftPanelGraphs(Graphs);
        }
    }
}