namespace SecondMonitor.WindowsControls.WPF.CarSettingsControl
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using DataModel.BasicProperties;

    public class TyreCompoundSettingsControl : Control
    {
        private static readonly DependencyProperty CompoundNameProperty = DependencyProperty.Register("CompoundName", typeof(string), typeof(TyreCompoundSettingsControl));
        private static readonly DependencyProperty TemperatureUnitProperties = DependencyProperty.Register("TemperatureUnit", typeof(TemperatureUnits), typeof(TyreCompoundSettingsControl));
        private static readonly DependencyProperty PressureUnitsProperty = DependencyProperty.Register("PressureUnits", typeof(PressureUnits), typeof(TyreCompoundSettingsControl));

        private static readonly DependencyProperty FrontMinimalIdealTyreTemperatureProperty = DependencyProperty.Register("FrontMinimalIdealTyreTemperature", typeof(Temperature), typeof(TyreCompoundSettingsControl));
        private static readonly DependencyProperty FrontMaximumIdealTyreTemperatureProperty = DependencyProperty.Register("FrontMaximumIdealTyreTemperature", typeof(Temperature), typeof(TyreCompoundSettingsControl));
        private static readonly DependencyProperty FrontMinimalIdealTyrePressureProperty = DependencyProperty.Register("FrontMinimalIdealTyrePressure", typeof(Pressure), typeof(TyreCompoundSettingsControl));
        private static readonly DependencyProperty FrontMaximumIdealTyrePressureProperty = DependencyProperty.Register("FrontMaximumIdealTyrePressure", typeof(Pressure), typeof(TyreCompoundSettingsControl));

        private static readonly DependencyProperty RearMinimalIdealTyreTemperatureProperty = DependencyProperty.Register("RearMinimalIdealTyreTemperature", typeof(Temperature), typeof(TyreCompoundSettingsControl));
        private static readonly DependencyProperty RearMaximumIdealTyreTemperatureProperty = DependencyProperty.Register("RearMaximumIdealTyreTemperature", typeof(Temperature), typeof(TyreCompoundSettingsControl));
        private static readonly DependencyProperty RearMinimalIdealTyrePressureProperty = DependencyProperty.Register("RearMinimalIdealTyrePressure", typeof(Pressure), typeof(TyreCompoundSettingsControl));
        private static readonly DependencyProperty RearMaximumIdealTyrePressureProperty = DependencyProperty.Register("RearMaximumIdealTyrePressure", typeof(Pressure), typeof(TyreCompoundSettingsControl));

        private static readonly DependencyProperty IsGlobalCompoundProperty = DependencyProperty.Register("IsGlobalCompound", typeof(bool), typeof(TyreCompoundSettingsControl));
        private static readonly DependencyProperty CopyCompoundCommandProperty = DependencyProperty.Register("CopyCompoundCommand", typeof(ICommand), typeof(TyreCompoundSettingsControl));
        private static readonly DependencyProperty NoWearLimitProperty = DependencyProperty.Register("NoWearLimit", typeof(double), typeof(TyreCompoundSettingsControl));

        private static readonly DependencyProperty LowWearLimitProperty = DependencyProperty.Register("LowWearLimit", typeof(double), typeof(TyreCompoundSettingsControl));
        private static readonly DependencyProperty HeavyWearLimitProperty = DependencyProperty.Register("HeavyWearLimit", typeof(double), typeof(TyreCompoundSettingsControl));

        public TemperatureUnits TemperatureUnit
        {
            get => (TemperatureUnits)GetValue(TemperatureUnitProperties);
            set => SetValue(TemperatureUnitProperties, value);
        }

        public PressureUnits PressureUnits
        {
            get => (PressureUnits)GetValue(PressureUnitsProperty);
            set => SetValue(PressureUnitsProperty, value);
        }

        public string CompoundName
        {
            get => (string)GetValue(CompoundNameProperty);
            set => SetValue(CompoundNameProperty, value);
        }

        public bool IsGlobalCompound
        {
            get => (bool)GetValue(IsGlobalCompoundProperty);
            set => SetValue(IsGlobalCompoundProperty, value);
        }

        public double NoWearLimit
        {
            get => (double) GetValue(NoWearLimitProperty);
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
            get => (Temperature)GetValue(FrontMinimalIdealTyreTemperatureProperty);
            set => SetValue(FrontMinimalIdealTyreTemperatureProperty, value);
        }

        public Temperature FrontMaximumIdealTyreTemperature
        {
            get => (Temperature)GetValue(FrontMaximumIdealTyreTemperatureProperty);
            set => SetValue(FrontMaximumIdealTyreTemperatureProperty, value);
        }

        public Pressure FrontMinimalIdealTyrePressure
        {
            get => (Pressure)GetValue(FrontMinimalIdealTyrePressureProperty);
            set => SetValue(FrontMinimalIdealTyrePressureProperty, value);
        }

        public Pressure FrontMaximumIdealTyrePressure
        {
            get => (Pressure)GetValue(FrontMaximumIdealTyrePressureProperty);
            set => SetValue(FrontMaximumIdealTyrePressureProperty, value);
        }

        public Temperature RearMinimalIdealTyreTemperature
        {
            get => (Temperature)GetValue(RearMinimalIdealTyreTemperatureProperty);
            set => SetValue(RearMinimalIdealTyreTemperatureProperty, value);
        }

        public Temperature RearMaximumIdealTyreTemperature
        {
            get => (Temperature)GetValue(RearMaximumIdealTyreTemperatureProperty);
            set => SetValue(RearMaximumIdealTyreTemperatureProperty, value);
        }

        public Pressure RearMinimalIdealTyrePressure
        {
            get => (Pressure)GetValue(RearMinimalIdealTyrePressureProperty);
            set => SetValue(RearMinimalIdealTyrePressureProperty, value);
        }

        public Pressure RearMaximumIdealTyrePressure
        {
            get => (Pressure)GetValue(RearMaximumIdealTyrePressureProperty);
            set => SetValue(RearMaximumIdealTyrePressureProperty, value);
        }

        public ICommand CopyCompoundCommand
        {
            get => (ICommand)GetValue(CopyCompoundCommandProperty);
            set => SetValue(CopyCompoundCommandProperty, value);
        }
    }
}