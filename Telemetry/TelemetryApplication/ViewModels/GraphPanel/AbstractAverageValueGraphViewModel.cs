namespace SecondMonitor.Telemetry.TelemetryApplication.ViewModels.GraphPanel
{
    using System.Collections.Generic;
    using DataModel.Telemetry;
    using OxyPlot;
    using OxyPlot.Series;
    using TelemetryManagement.DTO;

    public abstract class AbstractAverageValueGraphViewModel : AbstractGraphViewModel
    {
        protected override List<LineSeries> GetLineSeries(LapSummaryDto lapSummary, TimedTelemetrySnapshot[] dataPoints, OxyColor color)
        {
            LineSeries newLineSeries = CreateLineSeries($"Lap {lapSummary.CustomDisplayName}", color);
            List<DataPoint> points = new List<DataPoint>(dataPoints.Length - 1);
            DataPoint oldPoint = new DataPoint();
            for (int i = 0; i < dataPoints.Length - 1; i++)
            {
                var dp1 = dataPoints[i];
                var dp2 = dataPoints[i + 1];

                DataPoint oxyPoint = new DataPoint((GetXValue(dp1) + GetXValue(dp2)) / 2, GetYValue(dp1, dp2));
                if (i != 0 && !IsValid(oldPoint, oxyPoint))
                {
                    continue;
                }

                oldPoint = oxyPoint;
                points.Add(oxyPoint);
            }

            newLineSeries.Points.AddRange(points);

            /*LineSeries newLineSeries2 = CreateLineSeries($"Lap {lapSummary.CustomDisplayName} Angle", OxyColors.Wheat);
            List<DataPoint> points2 = new List<DataPoint>(dataPoints.Length - 1);
            for (int i = 0; i < dataPoints.Length - 1; i++)
            {
                var dp1 = dataPoints[i];
                var dp2 = dataPoints[i + 1];

                double angle = (dp1.PlayerData.WorldPosition.Z.InMeters - dp2.PlayerData.WorldPosition.Z.InMeters);
                DataPoint oxyPoint = new DataPoint((GetXValue(dp1) + GetXValue(dp2)) / 2,  angle );
                points2.Add(oxyPoint);
            }

            newLineSeries2.Points.AddRange(points2);*/

            List<LineSeries> series = new List<LineSeries>(1) { newLineSeries };
            return series;
        }

        protected abstract double GetYValue(TimedTelemetrySnapshot dp1, TimedTelemetrySnapshot dp2);

        protected abstract bool IsValid(DataPoint oldPoint, DataPoint newPoint);

    }
}