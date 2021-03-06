﻿namespace SecondMonitor.Rating.Application.Rating.RatingProvider.FieldRatingProvider.ReferenceRatingProviders
{
    using System;
    using System.Collections.Generic;
    using DataModel.Summary;

    public interface IReferenceRatingProvider
    {
        TimeSpan GetReferenceTime(IEnumerable<Driver> orderedDrivers);

        int GetReferenceDriverIndex(int fieldSize);
    }
}