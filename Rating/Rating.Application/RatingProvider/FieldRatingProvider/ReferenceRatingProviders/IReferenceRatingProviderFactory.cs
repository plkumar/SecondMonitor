namespace SecondMonitor.Rating.Application.RatingProvider.FieldRatingProvider.ReferenceRatingProviders
{
    public interface IReferenceRatingProviderFactory
    {
        IReferenceRatingProvider CreateReferenceRatingProvider();

        string[] GetAvailableReferenceRatingsProviders();
    }
}