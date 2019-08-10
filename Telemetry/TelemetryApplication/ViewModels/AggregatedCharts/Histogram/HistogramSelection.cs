namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.AggregatedCharts.Histogram
{
    using System.Collections.Generic;
    using DataModel.Telemetry;
    using OxyPlot.Annotations;

    internal class HistogramSelection
    {
        internal HistogramSelection(RectangleAnnotation selectionAnnotation)
        {
            SelectionAnnotation = selectionAnnotation;
            SelectedPoints = new HashSet<TimedTelemetrySnapshot>();
        }
        internal HashSet<TimedTelemetrySnapshot> SelectedPoints { get; }
        internal RectangleAnnotation SelectionAnnotation { get; }
    }
}