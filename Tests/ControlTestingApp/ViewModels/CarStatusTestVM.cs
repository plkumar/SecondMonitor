﻿namespace ControlTestingApp.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using SecondMonitor.Contracts.Commands;
    using SecondMonitor.DataModel.BasicProperties;
    using SecondMonitor.WindowsControls.WPF.CarSettingsControl;

    public class CarStatusTestVM : INotifyPropertyChanged
    {
        private readonly RelayCommandWithCondition _copyCompoundCommand;
        private readonly ObservableCollection<ITyreSettingsViewModel> _tyres;
        private double _tyreCondition = 50.0;
        private TemperatureUnits _temperatureUnits;
        private PressureUnits _pressureUnits = PressureUnits.Atmosphere;

        private ITyreSettingsViewModel _selectedTyreSettingsViewModel;

        public CarStatusTestVM()
        {
            TyreCoreRawTemperature = 50;
            BrakeRawTemperature = 200;
            _copyCompoundCommand = new RelayCommandWithCondition(CopyCompound, () => SelectedTyreSettingsViewModel?.IsGlobalCompound ?? false);
            _tyres = new ObservableCollection<ITyreSettingsViewModel>() { new TestTyreViewModel("Compound 1"), new TestTyreViewModel("Compound 2") { IsGlobalCompound = true, FrontMinimalIdealTyreTemperature = Temperature.FromCelsius(120) }, new TestTyreViewModel("Compound 3") };
            SelectedTyreSettingsViewModel = _tyres.First();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public double TyreCondition
        {
            get => _tyreCondition;
            set
            {
                _tyreCondition = value;
                NotifyPropertyChanged();
            }
        }

        public TemperatureUnits TemperatureUnits
        {
            get => _temperatureUnits;
            set
            {
                _temperatureUnits = value;
                NotifyPropertyChanged();
            }
        }

        public PressureUnits PressureUnits
        {
            get => _pressureUnits;
            set
            {
                _pressureUnits = value;
                NotifyPropertyChanged();
            }
        }

        public ITyreSettingsViewModel SelectedTyreSettingsViewModel
        {
            get => _selectedTyreSettingsViewModel;
            set
            {
                _selectedTyreSettingsViewModel = value;
                _copyCompoundCommand.NotifyCanExecuteChange();
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<ITyreSettingsViewModel> Tyres => _tyres;

        public ICommand ChangeTemperatureUnitsCommand => new RelayCommand(ChangeTemperatureUnits);

        public ICommand ChangePressureUnitsCommand => new RelayCommand(ChangePressureUnits);

        public ICommand CopyCompoundCommand => _copyCompoundCommand;

        private void CopyCompound()
        {
            TestTyreViewModel newViewModel = new TestTyreViewModel(SelectedTyreSettingsViewModel.CompoundName)
                                                 {
                                                     IsGlobalCompound = false, FrontMinimalIdealTyreTemperature = Temperature.FromCelsius(SelectedTyreSettingsViewModel.FrontMinimalIdealTyreTemperature.InCelsius),
                                                     FrontMaximumIdealTyreTemperature = Temperature.FromCelsius(SelectedTyreSettingsViewModel.FrontMaximumIdealTyreTemperature.InCelsius),
                                                     FrontMinimalIdealTyrePressure = Pressure.FromKiloPascals(SelectedTyreSettingsViewModel.FrontMinimalIdealTyrePressure.InKpa),
                                                     FrontMaximumIdealTyrePressure = Pressure.FromKiloPascals(SelectedTyreSettingsViewModel.FrontMaximumIdealTyrePressure.InKpa)
                                                 };
            Tyres.Add(newViewModel);
            ITyreSettingsViewModel previouslySelected = SelectedTyreSettingsViewModel;
            SelectedTyreSettingsViewModel = newViewModel;
            _tyres.Remove(previouslySelected);

        }

        public Temperature IdealBrakeTemperature { get; private set; }

        public Temperature BrakeTemperatureWindow { get; private set; }

        public double TyreCoreRawTemperature
        {
            set
            {
                IdealBrakeTemperature = Temperature.FromCelsius(value);
                NotifyPropertyChanged(nameof(IdealBrakeTemperature));
            }
        }

        public double BrakeRawTemperature
        {
            set
            {
                BrakeTemperatureWindow = Temperature.FromCelsius(value);
                NotifyPropertyChanged(nameof(BrakeTemperatureWindow));
            }
        }

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ChangeTemperatureUnits()
        {
            if (TemperatureUnits == TemperatureUnits.Celsius)
            {
                TemperatureUnits = TemperatureUnits.Kelvin;
                return;
            }

            if (TemperatureUnits == TemperatureUnits.Kelvin)
            {
                TemperatureUnits = TemperatureUnits.Fahrenheit;
                return;
            }

            if (TemperatureUnits == TemperatureUnits.Fahrenheit)
            {
                TemperatureUnits = TemperatureUnits.Celsius;
                return;
            }

        }

        private void ChangePressureUnits()
        {
            switch (PressureUnits)
            {
                case PressureUnits.Kpa:
                    PressureUnits = PressureUnits.Atmosphere;
                    break;
                case PressureUnits.Atmosphere:
                    PressureUnits = PressureUnits.Bar;
                    break;
                case PressureUnits.Bar:
                    PressureUnits = PressureUnits.Psi;
                    break;
                case PressureUnits.Psi:
                    PressureUnits = PressureUnits.Kpa;
                    break;
                default:
                    break;
            }
        }

        private class TestTyreViewModel : ITyreSettingsViewModel
        {
            public TestTyreViewModel(string name)
            {
                CompoundName = name;
                FrontMinimalIdealTyreTemperature = Temperature.FromCelsius(80);
                FrontMaximumIdealTyreTemperature = Temperature.FromCelsius(20);

                FrontMinimalIdealTyrePressure = Pressure.FromKiloPascals(120);
                FrontMaximumIdealTyrePressure = Pressure.FromKiloPascals(10);
                NoWearLimit = 10;
                LowWearLimit = 25;
                HeavyWearLimit = 70;
            }

            public string CompoundName { get; set; }

            public bool IsGlobalCompound { get; internal set; }

            public Temperature FrontMinimalIdealTyreTemperature { get; set; }

            public Temperature FrontMaximumIdealTyreTemperature { get; set; }

            public Pressure FrontMinimalIdealTyrePressure { get; set; }

            public Pressure FrontMaximumIdealTyrePressure { get; set; }
            public Temperature RearMinimalIdealTyreTemperature { get; set; }
            public Temperature RearMaximumIdealTyreTemperature { get; set; }
            public Pressure RearMinimalIdealTyrePressure { get; set; }
            public Pressure RearMaximumIdealTyrePressure { get; set; }
            public double NoWearLimit { get; set; }
            public double LowWearLimit { get; set; }
            public double HeavyWearLimit { get; set; }
        }
    }
}