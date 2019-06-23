namespace SecondMonitor.WindowsControls.WPF.CarSettingsControl
{
    using DataModel.BasicProperties;

    public interface ITyreSettingsViewModel
    {
        string CompoundName { get; set; }
        bool IsGlobalCompound { get; }

        Temperature FrontMinimalIdealTyreTemperature { get; set; }
        Temperature FrontMaximumIdealTyreTemperature { get; set; }

        Pressure FrontMinimalIdealTyrePressure { get; set; }
        Pressure FrontMaximumIdealTyrePressure { get; set; }

        Temperature RearMinimalIdealTyreTemperature { get; set; }
        Temperature RearMaximumIdealTyreTemperature { get; set; }

        Pressure RearMinimalIdealTyrePressure { get; set; }
        Pressure RearMaximumIdealTyrePressure { get; set; }

        double NoWearLimit { get; set; }
        double LowWearLimit { get; set; }
        double HeavyWearLimit { get; set; }

    }
}