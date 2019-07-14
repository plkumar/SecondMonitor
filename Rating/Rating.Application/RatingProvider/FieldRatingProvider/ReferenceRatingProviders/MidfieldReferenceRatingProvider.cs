namespace SecondMonitor.Rating.Application.RatingProvider.FieldRatingProvider.ReferenceRatingProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataModel.Summary;

    public class MidfieldReferenceRatingProvider : IReferenceRatingProvider
    {
        public TimeSpan GetReferenceTime(IEnumerable<Driver> orderedDrivers)
        {
            List<Driver> orderedDriversEnumerated = orderedDrivers.ToList();
            List<Driver> driversWithTime = orderedDriversEnumerated.Where(x => x.BestPersonalLap != null && x.IsPlayer == false).ToList();
            return orderedDriversEnumerated.Count / 2 < driversWithTime.Count ? driversWithTime[orderedDriversEnumerated.Count / 2].BestPersonalLap.LapTime : driversWithTime.Last().BestPersonalLap.LapTime;
        }

        public int GetReferenceDriverIndex(int fieldSize)
        {
            return fieldSize / 2;
        }
    }
}