namespace SecondMonitor.Rating.Application.RatingProvider.FieldRatingProvider.ReferenceRatingProviders
{
    using System.Linq;
    using Ninject;
    using Ninject.Syntax;
    using SecondMonitor.ViewModels.Settings;
    using SecondMonitor.ViewModels.Settings.ViewModel;

    public class ReferenceRatingProviderFactory : IReferenceRatingProviderFactory
    {
        private readonly IResolutionRoot _resolutionRoot;
        private readonly IKernel _kernel;
        private readonly RatingSettingsViewModel _ratingSettings;

        public ReferenceRatingProviderFactory(IResolutionRoot resolutionRoot, IKernel kernel, ISettingsProvider settingsProvider)
        {
            _resolutionRoot = resolutionRoot;
            _kernel = kernel;
            _ratingSettings = settingsProvider.DisplaySettingsViewModel.RatingSettingsViewModel;
        }

        public IReferenceRatingProvider CreateReferenceRatingProvider()
        {
            string providerName = _ratingSettings.SelectedReferenceRatingProvider;
            IReferenceRatingProvider referenceRatingProvider = _resolutionRoot.TryGet<IReferenceRatingProvider>(providerName);
            if (referenceRatingProvider == null)
            {
                var binding = _kernel.GetBindings(typeof(IReferenceRatingProvider)).First();
                providerName = binding.Metadata.Name;
                _ratingSettings.SelectedReferenceRatingProvider = providerName;
                return _resolutionRoot.Get<IReferenceRatingProvider>(providerName);

            }

            return referenceRatingProvider;
        }

        public string[] GetAvailableReferenceRatingsProviders()
        {
            return _kernel.GetBindings(typeof(IReferenceRatingProvider)).Select(x => x.Metadata.Name).ToArray();
        }
    }
}