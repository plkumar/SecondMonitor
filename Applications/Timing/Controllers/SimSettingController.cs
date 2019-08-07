namespace SecondMonitor.Timing.Controllers
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    using DataModel.OperationalRange;
    using DataModel.Snapshot;
    using SimdataManagement.SimSettings;
    using SimdataManagement.ViewModel;
    using CarSettings;
    using Contracts.Commands;
    using DataModel.Extensions;
    using Ninject;
    using Ninject.Syntax;
    using SimdataManagement.WheelDiameterWizard;
    using ViewModels.Settings.ViewModel;

    public class SimSettingController
    {
        private readonly DisplaySettingsViewModel _displaySettingsViewModel;
        private readonly IResolutionRoot _resolutionRoot;

        private readonly SimSettingAdapter _simSettingAdapter;

        private CarSettingsWindow _carSettingsWindow;
        private CarSettingsWindowViewModel _carSettingsWindowViewModel;
        private IWheelDiameterWizardController _wheelDiameterWizardController;

        public SimSettingController(DisplaySettingsViewModel displaySettingsViewModel, ICarSpecificationProvider carSpecificationProvider, IResolutionRoot resolutionRoot)
        {
            _displaySettingsViewModel = displaySettingsViewModel;
            _resolutionRoot = resolutionRoot;
            _simSettingAdapter = new SimSettingAdapter(carSpecificationProvider);
            CreateCarSettingsViewModel();
        }

        public void ApplySimSettings(SimulatorDataSet data)
        {
            data.Accept(_simSettingAdapter);
            _wheelDiameterWizardController?.ProcessDataSet(data);
        }

        public void OpenCarSettingsControl(Window parentWindow)
        {
            if (_carSettingsWindow != null)
            {
                _carSettingsWindow.Focus();
                return;
            }

            UpdateCarSettingsWindowViewModel();

            _carSettingsWindow = new CarSettingsWindow { Owner = parentWindow, DataContext = _carSettingsWindowViewModel};
            _carSettingsWindow.Closed += OnCarSettingsWindowClosed;
            _carSettingsWindow.Show();
        }

        private void UpdateCarSettingsWindowViewModel()
        {
            CarModelProperties playersCarProperties = _simSettingAdapter.LastUsedCarProperties;
            CarModelPropertiesViewModel playerCarsViewModel = new CarModelPropertiesViewModel();
            playerCarsViewModel.FromModel(playersCarProperties);

            _carSettingsWindowViewModel.CarModelPropertiesViewModel = playerCarsViewModel;

            TyreCompoundProperties lastUsedTyre = _simSettingAdapter.LastUsedCompound;
            ObservableCollection<TyreCompoundPropertiesViewModel> tyreSettingsViewModels = new ObservableCollection<TyreCompoundPropertiesViewModel>();
            playerCarsViewModel.TyreCompoundsProperties.ForEach(x => tyreSettingsViewModels.Add(x));
            foreach (TyreCompoundProperties globalTyreCompoundsProperty in _simSettingAdapter.GlobalTyreCompoundsProperties)
            {
                TyreCompoundPropertiesViewModel viewModel = new TyreCompoundPropertiesViewModel();
                viewModel.FromModel(globalTyreCompoundsProperty);
                viewModel.IsGlobalCompound = true;
                tyreSettingsViewModels.Add(viewModel);
            }

            _carSettingsWindowViewModel.TyreSettingsViewModels = tyreSettingsViewModels;
            _carSettingsWindowViewModel.SelectedTyreSettingsViewModel = tyreSettingsViewModels.First(x => x.CompoundName == lastUsedTyre.CompoundName);
        }

        private void CreateCarSettingsViewModel()
        {
            _carSettingsWindowViewModel = new CarSettingsWindowViewModel(_displaySettingsViewModel)
            {
                OkButtonCommand = new RelayCommand(SaveAndCloseWindow),
                CancelButtonCommand = new RelayCommand(CloseSettingsWindow),
                CopyCompoundToLocalCommand = new RelayCommand(CreateLocalCopyOfSelectedTyre),
                OpenTyreDiameterWizardCommand = new AsyncCommand(OpenTyreDiameterWizard)
            };
        }

        private async Task OpenTyreDiameterWizard()
        {
            if (_wheelDiameterWizardController != null)
            {
                return;
            }

            _wheelDiameterWizardController = _resolutionRoot.Get<IWheelDiameterWizardController>();
            _wheelDiameterWizardController.WizardCompleted += WheelDiameterWizardControllerOnWizardCompleted;
            await _wheelDiameterWizardController.StartControllerAsync();
            _wheelDiameterWizardController.OpenWizard(_carSettingsWindowViewModel.CarModelPropertiesViewModel);
        }

        private async void WheelDiameterWizardControllerOnWizardCompleted(object sender, EventArgs e)
        {
            await _wheelDiameterWizardController.StopControllerAsync();
            _wheelDiameterWizardController = null;
        }

        private void OnCarSettingsWindowClosed(object sender, EventArgs e)
        {
            _carSettingsWindow = null;
        }

        private void CloseSettingsWindow()
        {
            _carSettingsWindow?.Close();
        }

        private void UpdateSimSettingsFromViewModels()
        {
            _simSettingAdapter.ReplaceCarModelProperties(_carSettingsWindowViewModel.CarModelPropertiesViewModel.SaveToNewModel());
            _simSettingAdapter.GlobalTyreCompoundsProperties = _carSettingsWindowViewModel.TyreSettingsViewModels.Where(x => x.IsGlobalCompound).Select(y => y.SaveToNewModel()).ToList();
        }

        private void CreateLocalCopyOfSelectedTyre()
        {
            TyreCompoundPropertiesViewModel newCompoundPropertiesViewModel = new TyreCompoundPropertiesViewModel();
            newCompoundPropertiesViewModel.FromModel(_carSettingsWindowViewModel.SelectedTyreSettingsViewModel.SaveToNewModel());
            newCompoundPropertiesViewModel.IsGlobalCompound = false;
            _carSettingsWindowViewModel.CarModelPropertiesViewModel.TyreCompoundsProperties.Add(newCompoundPropertiesViewModel);
            _carSettingsWindowViewModel.TyreSettingsViewModels.Add(newCompoundPropertiesViewModel);
            _carSettingsWindowViewModel.SelectedTyreSettingsViewModel = newCompoundPropertiesViewModel;
        }

        private void SaveAndCloseWindow()
        {
            UpdateSimSettingsFromViewModels();
            CloseSettingsWindow();
        }


    }
}