namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WindowsControls.Properties;
    using DataModel.BasicProperties;
    using DataModel.Telemetry;
    using Filter;
    using SecondMonitor.ViewModels.Settings;
    using TelemetryManagement.DTO;
    using TelemetryManagement.StoryBoard;

    public abstract class AbstractTelemetryDataExtractor
    {
        protected AbstractTelemetryDataExtractor(ISettingsProvider settingsProvider)
        {
            VelocityUnitsSmall = settingsProvider.DisplaySettingsViewModel.VelocityUnitsVerySmall;
            VelocityUnits = settingsProvider.DisplaySettingsViewModel.VelocityUnits;
            DistanceUnitsSmall = settingsProvider.DisplaySettingsViewModel.DistanceUnitsVerySmall;
            ForceUnits = settingsProvider.DisplaySettingsViewModel.ForceUnits;
            AngleUnits = settingsProvider.DisplaySettingsViewModel.AngleUnits;
        }

        public VelocityUnits VelocityUnits { get; }
        public VelocityUnits VelocityUnitsSmall { get; }
        public DistanceUnits DistanceUnitsSmall { get; }
        public ForceUnits ForceUnits { get; }
        public AngleUnits AngleUnits { get; }

        protected TimedValue[] ExtractTimedValuesOfLoadedLaps(IEnumerable<LapTelemetryDto> loadedLaps, Func<TimedTelemetrySnapshot, double> extractionFunc, [CanBeNull] IReadOnlyCollection<ITelemetryFilter> filters)
        {
            IEnumerable<TimedValue> timedValues = loadedLaps.SelectMany(x => ExtractTimedValues(x.DataPoints, extractionFunc, filters));
            return timedValues.ToArray();
        }

        private static List<TimedValue> ExtractTimedValues(List<TimedTelemetrySnapshot> snapshots, Func<TimedTelemetrySnapshot, double> extractDataFunc, IReadOnlyCollection<ITelemetryFilter> filters)
        {
            List<TimedValue> timedValues = new List<TimedValue>(snapshots.Count);
            for (int i = 0; i < snapshots.Count - 1; i++)
            {
                TimedTelemetrySnapshot firstSnapshot = snapshots[i];
                TimedTelemetrySnapshot secondSnapshot = snapshots[i + 1];

                if (filters?.Any(x => !x.Accepts(firstSnapshot) || !x.Accepts(secondSnapshot)) == true)
                {
                    continue;
                }

                double value = (extractDataFunc(firstSnapshot) + extractDataFunc(secondSnapshot)) / 2;

                timedValues.Add(new TimedValue(value, firstSnapshot, secondSnapshot));
            }

            return timedValues;
        }
    }
}