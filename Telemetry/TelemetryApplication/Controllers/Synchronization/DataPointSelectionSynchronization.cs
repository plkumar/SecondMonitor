namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.Synchronization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataModel.Extensions;
    using DataModel.Telemetry;

    public class DataPointSelectionSynchronization : IDataPointSelectionSynchronization
    {
        public event EventHandler<TimedTelemetryArgs> OnPointsSelected;
        public event EventHandler<TimedTelemetryArgs> OnPointsDeselected;


        private readonly Dictionary<TimedTelemetrySnapshot, int> _selectionCount;

        public DataPointSelectionSynchronization()
        {
            _selectionCount = new Dictionary<TimedTelemetrySnapshot, int>();
        }

        public IEnumerable<TimedTelemetrySnapshot> SelectedPoints => _selectionCount.Keys;

        public void SelectPoints(IEnumerable<TimedTelemetrySnapshot> timedTelemetrySnapshots)
        {
            var telemetrySnapshotsEnumerated = timedTelemetrySnapshots.ToArray();
            IReadOnlyCollection<TimedTelemetrySnapshot> firstTimeSelected = telemetrySnapshotsEnumerated.Except(_selectionCount.Keys).ToList();
            firstTimeSelected.ForEach(x => _selectionCount[x] = 0);
            //telemetrySnapshotsEnumerated.ForEach(x => _selectionCount[x]++);
            if (firstTimeSelected.Count == 0)
            {
                return;
            }
            OnPointsSelected?.Invoke(this, new TimedTelemetryArgs(firstTimeSelected));
        }

        public void DeSelectPoints(IEnumerable<TimedTelemetrySnapshot> timedTelemetrySnapshots)
        {
            var telemetrySnapshotsEnumerated = timedTelemetrySnapshots.ToArray();
            var previouslySelected = telemetrySnapshotsEnumerated.Where(x => _selectionCount.ContainsKey(x)).ToList();
            /*IReadOnlyCollection<TimedTelemetrySnapshot> reallyDeselected = _selectionCount.Where(x => x.Value == 0).Select(x => x.Key).ToList();
            reallyDeselected.ForEach(x => _selectionCount.Remove(x));*/
            if (previouslySelected.Count == 0)
            {
                return;
            }
            previouslySelected.ForEach(x => _selectionCount.Remove(x));
            OnPointsDeselected?.Invoke(this, new TimedTelemetryArgs(previouslySelected));
        }

        public void DeselectAllPoints()
        {
            if (_selectionCount.Count == 0)
            {
                return;
            }
            OnPointsDeselected?.Invoke(this, new TimedTelemetryArgs(_selectionCount.Keys));
            _selectionCount.Clear();
        }
    }
}