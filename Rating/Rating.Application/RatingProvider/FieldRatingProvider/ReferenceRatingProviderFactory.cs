namespace SecondMonitor.Rating.Application.RatingProvider.FieldRatingProvider
{
    using Ninject;
    using Ninject.Syntax;

    public class ReferenceRatingProviderFactory : IReferenceRatingProviderFactory
    {
        private readonly IResolutionRoot _resolutionRoot;

        public ReferenceRatingProviderFactory(IResolutionRoot resolutionRoot)
        {
            _resolutionRoot = resolutionRoot;
        }

        public IReferenceRatingProvider CreateReferenceRatingProvider(string providerName)
        {
            return _resolutionRoot.Get<IReferenceRatingProvider>(providerName);
        }
    }
}