namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.Synchronization
{
    using System;
    using System.Collections.Generic;
    using DataModel.Telemetry;

    public interface IDataPointSelectionSynchronization
    {
        event EventHandler<TimedTelemetryArgs> OnPointsSelected;
        event EventHandler<TimedTelemetryArgs> OnPointsDeselected;

        IEnumerable<TimedTelemetrySnapshot> SelectedPoints { get; }


        void SelectPoints(IEnumerable<TimedTelemetrySnapshot> timedTelemetrySnapshots);
        void DeSelectPoints(IEnumerable<TimedTelemetrySnapshot> timedTelemetrySnapshots);
        void DeselectAllPoints();
    }
}