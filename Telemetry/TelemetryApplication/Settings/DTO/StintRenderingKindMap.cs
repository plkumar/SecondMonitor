namespace SecondMonitor.Telemetry.TelemetryApplication.Settings.DTO
{
    using System.Collections.Generic;
    using System.Linq;

    public class StintRenderingKindMap
    {
        private readonly List<(StintRenderingKind stintRenderingKind, string humanReadableValue)> _translationList;

        public StintRenderingKindMap()
        {
            _translationList = new List<(StintRenderingKind stintRenderingKind, string humanReadableValue)>();
            _translationList.Add((StintRenderingKind.None, "None (All Laps in single chart)"));
            _translationList.Add((StintRenderingKind.SingleChart, "Separate series for each stint"));
            _translationList.Add((StintRenderingKind.MultipleCharts, "Separate chart for each stint"));
        }

        public string ToHumanReadable(StintRenderingKind stintRenderingKind)
        {
            return _translationList.First(x => x.stintRenderingKind == stintRenderingKind).humanReadableValue;
        }

        public StintRenderingKind FromHumanReadable(string humanString)
        {
            return _translationList.Find(x => x.humanReadableValue == humanString).stintRenderingKind;
        }

        public IEnumerable<string> GetAllHumanReadableValue()
        {
            return _translationList.Select(x => x.humanReadableValue);
        }
    }
}