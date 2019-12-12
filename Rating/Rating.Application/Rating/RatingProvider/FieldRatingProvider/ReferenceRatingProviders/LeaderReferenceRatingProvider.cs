namespace SecondMonitor.Rating.Application.Rating.RatingProvider.FieldRatingProvider.ReferenceRatingProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataModel.Summary;

    public class LeaderReferenceRatingProvider : IReferenceRatingProvider
    {
        public TimeSpan GetReferenceTime(IEnumerable<Driver> orderedDrivers)
        {
            IEnumerable<Driver> driversWithTime = orderedDrivers.Where(x => x.BestPersonalLap != null && x.IsPlayer == false);
            return driversWithTime.FirstOrDefault()?.BestPersonalLap?.LapTime ??  TimeSpan.Zero;
        }

        public int GetReferenceDriverIndex(int fieldSize)
        {
            return 1;
        }
    }
}