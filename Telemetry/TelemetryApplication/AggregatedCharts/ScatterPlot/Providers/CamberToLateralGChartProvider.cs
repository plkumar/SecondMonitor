namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class CamberToLateralGChartProvider : AbstractWheelChartProvider
    {
        public CamberToLateralGChartProvider(CamberToLateralGExtractor dataExtractor, ILoadedLapsCache loadedLaps) : base(dataExtractor, loadedLaps)
        {
        }

        public override string ChartName => "Camper / Lateral Acceleration";

        protected override AxisDefinition CreateYAxisDefinition()
        {
            var axisDefinition = base.CreateYAxisDefinition();
            axisDefinition.Minimum = -8;
            axisDefinition.Maximum = 1;
            axisDefinition.UseCustomRange = true;
            return axisDefinition;
        }
    }
}