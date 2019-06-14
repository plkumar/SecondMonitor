namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.GraphPanel.Wheels
{
    using System;
    using DataModel.BasicProperties;
    using DataModel.Snapshot.Systems;

    public class TyreLoadGraphViewModel : AbstractWheelsGraphViewModel
    {

        public override string Title => "Tyre Load";

        protected override string YUnits => Force.GetUnitSymbol(ForceUnits);

        protected override double YTickInterval => Force.GetFromNewtons(2500).GetValueInUnits(ForceUnits);

        protected override bool CanYZoom => true;

        protected override Func<WheelInfo, double> ExtractorFunction => (x) => x.TyreLoad.GetValueInUnits(ForceUnits);
    }
}