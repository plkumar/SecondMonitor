namespace SecondMonitor.Rating.Application.Rating.RatingProvider.FieldRatingProvider.ReferenceRatingProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataModel.Summary;

    public class AverageTimeReferenceRatingProvider : IReferenceRatingProvider
    {
        public TimeSpan GetReferenceTime(IEnumerable<Driver> orderedDrivers)
        {
            double avgTimeInSeconds = orderedDrivers.Where(x => !x.IsPlayer && x.BestPersonalLap != null).Average(x => x.BestPersonalLap.LapTime.TotalSeconds);
            return TimeSpan.FromSeconds(avgTimeInSeconds);
        }

        public int GetReferenceDriverIndex(int fieldSize)
        {
            return fieldSize / 2;
        }
    }
}