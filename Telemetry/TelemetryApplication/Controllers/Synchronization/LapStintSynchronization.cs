namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.Synchronization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataModel.Extensions;
    using ViewModels.LoadedLapCache;

    public class LapStintSynchronization : ILapStintSynchronization
    {
        private readonly ILoadedLapsCache _loadedLapsCache;
        private readonly Dictionary<string, int> _lapToStintMap;
        public event EventHandler<LapStintArg> LapStintChanged;

        public LapStintSynchronization(ITelemetryViewsSynchronization telemetryViewsSynchronization, ILoadedLapsCache loadedLapsCache)
        {
            telemetryViewsSynchronization.LapLoaded += TelemetryViewsSynchronizationOnLapLoaded;
            _loadedLapsCache = loadedLapsCache;
            _lapToStintMap = new Dictionary<string, int>();
        }

        private void TelemetryViewsSynchronizationOnLapLoaded(object sender, LapTelemetryArgs e)
        {
            if (_lapToStintMap.TryGetValue(e.LapTelemetry.LapSummary.Id, out int stintNumber))
            {
                e.LapTelemetry.LapSummary.Stint = stintNumber;
            }
        }

        public void SetStintNumberForLap(string lapId, int stint)
        {
            _lapToStintMap[lapId] = stint;
            _loadedLapsCache.LoadedLaps.Where(x => x.LapSummary.Id == lapId).ForEach(x => x.LapSummary.Stint = stint);
            LapStintChanged?.Invoke(this, new LapStintArg(lapId, stint));

        }
    }
}