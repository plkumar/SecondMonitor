namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.GraphPanel
{
    using System.Collections.Generic;
    using DataExtractor;
    using DataModel.BasicProperties;
    using DataModel.Telemetry;

    public class WorldYawGraphViewModel : AbstractSingleSeriesGraphViewModel
    {
       public WorldYawGraphViewModel(IEnumerable<ISingleSeriesDataExtractor> dataExtractors) : base(dataExtractors)
        {
        }

       public override string Title => "World Yaw";

       protected override string YUnits => "°";

       protected override double YTickInterval => 45;

        protected override bool CanYZoom => true;

        protected override double GetYValue(TimedTelemetrySnapshot value)
        {
            return value.PlayerData.CarInfo.WorldOrientation.Yaw.GetValueInUnits(AngleUnits.Degrees);
        }
    }
}