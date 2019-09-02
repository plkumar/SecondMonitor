namespace SecondMonitor.ViewModels.PluginsSettings
{
    using System.Collections.Generic;
    using System.Linq;
    using Factory;
    using PluginsConfiguration.Common.DataModel;

    public class PluginsConfigurationViewModel : AbstractViewModel<PluginsConfiguration>, IPluginsConfigurationViewModel
    {
        private readonly IViewModelFactory _viewModelFactory;
        private IRemoteConfigurationViewModel _remoteConfigurationViewModel;
        private IReadOnlyCollection<IPluginConfigurationViewModel> _pluginConfigurations;
        private F12019ConfigurationViewModel _f12019ConfigurationViewModel;

        public PluginsConfigurationViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public IRemoteConfigurationViewModel RemoteConfigurationViewModel
        {
            get => _remoteConfigurationViewModel;
            private set => SetProperty(ref _remoteConfigurationViewModel, value);
        }

        public IReadOnlyCollection<IPluginConfigurationViewModel> PluginConfigurations
        {
            get => _pluginConfigurations;
            private set => SetProperty(ref _pluginConfigurations, value);
        }

        public F12019ConfigurationViewModel F12019ConfigurationViewModel
        {
            get => _f12019ConfigurationViewModel;
            set => SetProperty(ref _f12019ConfigurationViewModel, value);
        }

        protected override void ApplyModel(PluginsConfiguration model)
        {
            IRemoteConfigurationViewModel newViewModel = _viewModelFactory.Create<IRemoteConfigurationViewModel>();
            newViewModel.FromModel(model.RemoteConfiguration);
            RemoteConfigurationViewModel = newViewModel;

            List<IPluginConfigurationViewModel> newPluginsViewModel = new List<IPluginConfigurationViewModel>(model.PluginsConfigurations.Count);
            foreach (PluginConfiguration modelPluginsConfiguration in model.PluginsConfigurations)
            {
                IPluginConfigurationViewModel newPluginConfigurationViewModel = _viewModelFactory.Create<IPluginConfigurationViewModel>();
                newPluginConfigurationViewModel.FromModel(modelPluginsConfiguration);
                newPluginsViewModel.Add(newPluginConfigurationViewModel);
            }

            PluginConfigurations = newPluginsViewModel;

            F12019ConfigurationViewModel f12019ConfigurationViewModel = _viewModelFactory.Create<F12019ConfigurationViewModel>();
            f12019ConfigurationViewModel.FromModel(model.F12019Configuration);
            F12019ConfigurationViewModel = f12019ConfigurationViewModel;
        }

        public override PluginsConfiguration SaveToNewModel()
        {
            return new PluginsConfiguration()
            {
                RemoteConfiguration = RemoteConfigurationViewModel.SaveToNewModel(),
                F12019Configuration = F12019ConfigurationViewModel.SaveToNewModel(),
                PluginsConfigurations = PluginConfigurations.Select(x => x.SaveToNewModel()).ToList(),

            };
        }
    }
}