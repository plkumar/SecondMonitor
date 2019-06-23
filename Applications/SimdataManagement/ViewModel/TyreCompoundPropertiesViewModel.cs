using System.Windows;
using SecondMonitor.WindowsControls.WPF.CarSettingsControl;

namespace SecondMonitor.SimdataManagement.ViewModel
{
    using DataModel.BasicProperties;
    using DataModel.OperationalRange;
    using ViewModels;
    public class TyreCompoundPropertiesViewModel : AbstractViewModel<TyreCompoundProperties>, ITyreSettingsViewModel
    {
        private static readonly DependencyProperty NoWearLimitProperty = DependencyProperty.Register("NoWearLimit", typeof(double), typeof(TyreCompoundPropertiesViewModel));
        private static readonly DependencyProperty LowWearLimitProperty = DependencyProperty.Register("LowWearLimit", typeof(double), typeof(TyreCompoundPropertiesViewModel));
        private static readonly DependencyProperty HeavyWearLimitProperty = DependencyProperty.Register("HeavyWearLimit", typeof(double), typeof(TyreCompoundPropertiesViewModel));

        private string _compoundName;

        private Pressure _minimumIdealTyrePressure;
        private Pressure _maximumIdealTyrePressure;

        private Temperature _minimumIdealTyreTemperature;
        private Temperature _maximumIdealTyreTemperature;

        private bool _isGlobalCompound;
        private Temperature _rearMinimalIdealTyreTemperature;
        private Temperature _rearMaximumIdealTyreTemperature;
        private Pressure _rearMinimalIdealTyrePressure;
        private Pressure _rearMaximumIdealTyrePressure;

        public bool IsGlobalCompound
        {
            get => _isGlobalCompound;
            set
            {
                _isGlobalCompound = value;
                NotifyPropertyChanged();
            }
        }

        public string CompoundName
        {
            get => _compoundName;
            set
            {
                _compoundName = value;
                NotifyPropertyChanged();
            }
        }

        public Pressure FrontMinimalIdealTyrePressure
        {
            get => _minimumIdealTyrePressure;
            set
            {
                _minimumIdealTyrePressure = value;
                NotifyPropertyChanged();
            }
        }

        public Pressure FrontMaximumIdealTyrePressure
        {
            get => _maximumIdealTyrePressure;
            set
            {
                _maximumIdealTyrePressure = value;
                NotifyPropertyChanged();
            }
        }

        public Temperature RearMinimalIdealTyreTemperature
        {
            get => _rearMinimalIdealTyreTemperature;
            set => SetProperty(ref _rearMinimalIdealTyreTemperature, value);
        }

        public Temperature RearMaximumIdealTyreTemperature
        {
            get => _rearMaximumIdealTyreTemperature;
            set => SetProperty(ref _rearMaximumIdealTyreTemperature, value);
        }

        public Pressure RearMinimalIdealTyrePressure
        {
            get => _rearMinimalIdealTyrePressure;
            set => SetProperty(ref _rearMinimalIdealTyrePressure, value);
        }

        public Pressure RearMaximumIdealTyrePressure
        {
            get => _rearMaximumIdealTyrePressure;
            set => SetProperty(ref _rearMaximumIdealTyrePressure, value);
        }

        public double NoWearLimit
        {
            get => (double)GetValue(NoWearLimitProperty);
            set => SetValue(NoWearLimitProperty, value);
        }

        public double LowWearLimit
        {
            get => (double)GetValue(LowWearLimitProperty);
            set => SetValue(LowWearLimitProperty, value);
        }

        public double HeavyWearLimit
        {
            get => (double)GetValue(HeavyWearLimitProperty);
            set => SetValue(HeavyWearLimitProperty, value);
        }

        public Temperature FrontMinimalIdealTyreTemperature
        {
            get => _minimumIdealTyreTemperature;
            set
            {
                _minimumIdealTyreTemperature = value;
                NotifyPropertyChanged();
            }
        }

        public Temperature FrontMaximumIdealTyreTemperature
        {
            get => _maximumIdealTyreTemperature;
            set
            {
                _maximumIdealTyreTemperature = value;
                NotifyPropertyChanged();
            }
        }

        protected override void ApplyModel(TyreCompoundProperties model)
        {
            CompoundName = model.CompoundName;
            NoWearLimit = model.NoWearLimit * 100.0;
            LowWearLimit = model.LowWearLimit * 100.0;
            HeavyWearLimit = model.HeavyWearLimit * 100.0;

            FrontMinimalIdealTyrePressure = Pressure.FromKiloPascals(model.FrontIdealPressure.InKpa - model.FrontIdealPressureWindow.InKpa);
            FrontMaximumIdealTyrePressure = Pressure.FromKiloPascals(model.FrontIdealPressure.InKpa + model.FrontIdealPressureWindow.InKpa);

            FrontMinimalIdealTyreTemperature = Temperature.FromCelsius(model.FrontIdealTemperature.InCelsius - model.FrontIdealTemperatureWindow.InCelsius);
            FrontMaximumIdealTyreTemperature = Temperature.FromCelsius(model.FrontIdealTemperature.InCelsius + model.FrontIdealTemperatureWindow.InCelsius);

            RearMinimalIdealTyrePressure = Pressure.FromKiloPascals(model.RearIdealPressure.InKpa - model.RearIdealPressureWindow.InKpa);
            RearMaximumIdealTyrePressure = Pressure.FromKiloPascals(model.RearIdealPressure.InKpa + model.RearIdealPressureWindow.InKpa);

            RearMinimalIdealTyreTemperature = Temperature.FromCelsius(model.RearIdealTemperature.InCelsius - model.RearIdealTemperatureWindow.InCelsius);
            RearMaximumIdealTyreTemperature = Temperature.FromCelsius(model.RearIdealTemperature.InCelsius + model.RearIdealTemperatureWindow.InCelsius);
        }

        public override TyreCompoundProperties SaveToNewModel()
        {
            return new TyreCompoundProperties()
                       {
                           CompoundName = CompoundName,
                           LowWearLimit =  LowWearLimit / 100.0,
                           NoWearLimit = NoWearLimit / 100.0,
                           HeavyWearLimit = HeavyWearLimit / 100.0,
                           FrontIdealPressure = Pressure.FromKiloPascals((FrontMinimalIdealTyrePressure.InKpa + FrontMaximumIdealTyrePressure.InKpa) * 0.5),
                           FrontIdealPressureWindow = Pressure.FromKiloPascals((FrontMaximumIdealTyrePressure.InKpa - FrontMinimalIdealTyrePressure.InKpa) * 0.5),
                           FrontIdealTemperature = Temperature.FromCelsius((FrontMinimalIdealTyreTemperature.InCelsius + FrontMaximumIdealTyreTemperature.InCelsius) * 0.5),
                           FrontIdealTemperatureWindow = Temperature.FromCelsius((FrontMaximumIdealTyreTemperature.InCelsius - FrontMinimalIdealTyreTemperature.InCelsius) * 0.5),

                           RearIdealPressure = Pressure.FromKiloPascals((RearMinimalIdealTyrePressure.InKpa + RearMaximumIdealTyrePressure.InKpa) * 0.5),
                           RearIdealPressureWindow = Pressure.FromKiloPascals((RearMaximumIdealTyrePressure.InKpa - RearMinimalIdealTyrePressure.InKpa) * 0.5),
                           RearIdealTemperature = Temperature.FromCelsius((RearMinimalIdealTyreTemperature.InCelsius + RearMaximumIdealTyreTemperature.InCelsius) * 0.5),
                           RearIdealTemperatureWindow = Temperature.FromCelsius((RearMaximumIdealTyreTemperature.InCelsius - RearMinimalIdealTyreTemperature.InCelsius) * 0.5),
            };
        }
    }
}