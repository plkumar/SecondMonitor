namespace SecondMonitor.Rating.Application.RatingProvider.FieldRatingProvider
{
    public interface IReferenceRatingProviderFactory
    {
        IReferenceRatingProvider CreateReferenceRatingProvider(string providerName);
    }
}