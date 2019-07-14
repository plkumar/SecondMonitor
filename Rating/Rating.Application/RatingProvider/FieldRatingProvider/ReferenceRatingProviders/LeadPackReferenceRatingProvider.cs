namespace SecondMonitor.Rating.Application.RatingProvider.FieldRatingProvider.ReferenceRatingProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataModel.Summary;

    public class LeadPackReferenceRatingProvider : IReferenceRatingProvider
    {
        public TimeSpan GetReferenceTime(IEnumerable<Driver> orderedDrivers)
        {
            List<Driver> driversWithTime = orderedDrivers.Where(x => x.BestPersonalLap != null && x.IsPlayer == false).ToList();
            if (driversWithTime.Count == 0)
            {
                return TimeSpan.Zero;
            }

            double avgTimeSeconds = driversWithTime.Take(4).Average(x => x.BestPersonalLap.LapTime.TotalSeconds);
            return TimeSpan.FromSeconds(avgTimeSeconds);
        }

        public int GetReferenceDriverIndex(int fieldSize)
        {
            return Math.Min(3, fieldSize - 1);
        }
    }
}