namespace SecondMonitor.Rating.Application.RatingProvider.FieldRatingProvider
{
    using System;
    using System.Collections.Generic;
    using DataModel.Summary;

    public interface IReferenceRatingProvider
    {
        TimeSpan GetReferenceTime(IEnumerable<Driver> orderedDrivers);

        int GetReferencePosition(int fieldSize);
    }
}