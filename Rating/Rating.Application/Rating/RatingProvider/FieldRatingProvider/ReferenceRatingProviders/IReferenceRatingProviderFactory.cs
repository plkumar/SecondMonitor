namespace SecondMonitor.Rating.Application.Rating.RatingProvider.FieldRatingProvider.ReferenceRatingProviders
{
    public interface IReferenceRatingProviderFactory
    {
        IReferenceRatingProvider CreateReferenceRatingProvider();

        string[] GetAvailableReferenceRatingsProviders();
    }
}