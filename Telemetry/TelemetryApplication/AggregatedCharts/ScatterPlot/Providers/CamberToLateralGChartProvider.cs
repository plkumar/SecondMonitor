namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.ScatterPlot.Providers
{
    using Controllers.Synchronization;
    using Extractors;
    using ViewModels.LoadedLapCache;

    public class CamberToLateralGChartProvider : AbstractWheelChartProvider
    {
        public CamberToLateralGChartProvider(CamberToLateralGExtractor dataExtractor, ILoadedLapsCache loadedLaps, IDataPointSelectionSynchronization dataPointSelectionSynchronization) : base(dataExtractor, loadedLaps, dataPointSelectionSynchronization)
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