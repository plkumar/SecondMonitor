namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extractors;
    using Filter;
    using ViewModels.LoadedLapCache;

    public class PowerCurveProvider : AbstractStintScatterPlot2YAxisProvider
    {
        public PowerCurveProvider(ILoadedLapsCache loadedLapsCache, FullThrottleFilter fullThrottleFilter, InGearFilter inGearFilter, PositiveTorqueFilter positiveTorqueFilter,
            RpmToTorqueExtractor y1AxisDataExtractor, RpmToPowerExtractor y2AxisDataExtractor) : base(loadedLapsCache, y1AxisDataExtractor, y2AxisDataExtractor, new List<ITelemetryFilter>() {fullThrottleFilter, inGearFilter, positiveTorqueFilter})
        {
        }

        public override string ChartName => "Power Curve";
        public override AggregatedChartKind Kind => AggregatedChartKind.ScatterPlot;
        protected override string Y1Title => "Torque";
        protected override string Y2Title => "Power";

        protected override void OnNewScatterPlot(ScatterPlot2YAxis scatterPlot)
        {
            double maximumPower = scatterPlot.ScatterPlotSeries.SelectMany(x => x.DataPoints).Max(x => x.Y);
            double maximumTorque = scatterPlot.ScatterPlotY2Series.SelectMany(x => x.DataPoints).Max(x => x.Y);
            double maximumRpm = scatterPlot.ScatterPlotY2Series.SelectMany(x => x.DataPoints).Max(x => x.X) * 1.1;
            double maximum = Math.Max(maximumTorque, maximumPower) * 1.1;
            scatterPlot.YAxis.SetCustomRange(0, maximum);
            scatterPlot.YAxis.Title = Y1Title;
            scatterPlot.Y2Axis.SetCustomRange(0, maximum);
            scatterPlot.Y2Axis.Title = Y2Title;
            scatterPlot.XAxis.SetCustomRange(0, maximumRpm);

            base.OnNewScatterPlot(scatterPlot);
        }
    }
}