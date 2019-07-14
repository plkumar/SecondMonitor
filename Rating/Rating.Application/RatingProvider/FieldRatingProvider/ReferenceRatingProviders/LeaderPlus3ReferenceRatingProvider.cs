namespace SecondMonitor.Rating.Application.RatingProvider.FieldRatingProvider.ReferenceRatingProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataModel.Summary;

    public class LeaderPlus3ReferenceRatingProvider : IReferenceRatingProvider
    {
        public TimeSpan GetReferenceTime(IEnumerable<Driver> orderedDrivers)
        {
            IEnumerable<Driver> driversWithTime = orderedDrivers.Where(x => x.BestPersonalLap != null && x.IsPlayer == false);
            double timeInSeconds = driversWithTime.FirstOrDefault()?.BestPersonalLap?.LapTime.TotalSeconds ?? 0;
            timeInSeconds = timeInSeconds * 0.97;
            return TimeSpan.FromSeconds(timeInSeconds);
        }

        public int GetReferenceDriverIndex(int fieldSize)
        {
            return 1;
        }
    }
}