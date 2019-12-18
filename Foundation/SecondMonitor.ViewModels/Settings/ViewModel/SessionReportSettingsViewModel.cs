namespace SecondMonitor.ViewModels.Settings.ViewModel
{
    using Model;

    public class SessionReportSettingsViewModel : AbstractViewModel
    {

        private bool _export;
        private bool _autoOpen;

        public bool Export
        {
            get => _export;
            set => SetProperty(ref _export, value);
        }

        public bool AutoOpen
        {
            get => _autoOpen;
            set => SetProperty(ref _autoOpen, value);
        }

        public static SessionReportSettingsViewModel FromModel(SessionReportSettings model)
        {
            SessionReportSettingsViewModel viewModel =
                new SessionReportSettingsViewModel() { Export = model.Export, AutoOpen = model.AutoOpen };
            return viewModel;
        }

        public SessionReportSettings ToModel()
        {
            return new SessionReportSettings() { AutoOpen = AutoOpen, Export = Export, };
        }
    }
}