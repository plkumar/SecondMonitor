namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.SettingsWindow
{
    using System.Collections.Generic;
    using System.Linq;
    using SecondMonitor.ViewModels;
    using Settings.DTO;

    public class AggregatedChartSettingsViewModel : AbstractViewModel<AggregatedChartSettingsDto>, IAggregatedChartSettingsViewModel
    {
        private readonly StintRenderingKindMap _stintRenderingKindMap;
        private string _selectedStintRenderingKind;

        public AggregatedChartSettingsViewModel()
        {
            _stintRenderingKindMap = new StintRenderingKindMap();
            AllowedStintRenderingKind = _stintRenderingKindMap.GetAllHumanReadableValue().ToList();
        }

        public List<string> AllowedStintRenderingKind { get; }

        public string SelectedStintRenderingKind
        {
            get => _selectedStintRenderingKind;
            set => SetProperty(ref _selectedStintRenderingKind, value);
        }

        protected override void ApplyModel(AggregatedChartSettingsDto model)
        {
            SelectedStintRenderingKind = _stintRenderingKindMap.ToHumanReadable(model.StintRenderingKind);
        }

        public override AggregatedChartSettingsDto SaveToNewModel()
        {
            return new AggregatedChartSettingsDto()
            {
                StintRenderingKind = _stintRenderingKindMap.FromHumanReadable(SelectedStintRenderingKind)
            };
        }
    }
}
