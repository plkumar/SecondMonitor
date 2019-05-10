namespace SecondMonitor.DataModel.Calculators
{
    using System;
    using BasicProperties;
    using Telemetry;

    public class YawCalculator
    {
        public Angle CalculateYaw(TimedTelemetrySnapshot dp1, TimedTelemetrySnapshot dp2)
        {
            if (dp1.PlayerData.CarInfo.WorldOrientation == null || dp2.PlayerData.CarInfo.WorldOrientation == null)
            {
                return new Angle();
            }

            double carYaw = (dp1.PlayerData.CarInfo.WorldOrientation.Yaw.InRadians + dp2.PlayerData.CarInfo.WorldOrientation.Yaw.InRadians) / 2;
            double theta = Math.Atan2(dp1.PlayerData.WorldPosition.X.InCentimeters - dp2.PlayerData.WorldPosition.X.InCentimeters, dp1.PlayerData.WorldPosition.Z.InCentimeters - dp2.PlayerData.WorldPosition.Z.InCentimeters);
            Angle angle = Angle.GetFromRadians(theta - carYaw);
            if (Math.Abs(angle.InDegrees) > 180)
            {
                double dAngle = angle.InDegrees;
                dAngle %= 360;

                // force it to be the positive remainder, so that 0 <= angle < 360
                dAngle = (dAngle + 360) % 360;

                // force into the minimum absolute value residue class, so that -180 < angle <= 180
                if (dAngle > 180)
                {
                    dAngle -= 360;
                }

                angle.InDegrees = dAngle;
            }
            return angle;
        }

    }
}