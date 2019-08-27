namespace SecondMonitor.SimdataManagement.DriverPresentation
{
    using System;
    using WindowsControls.Properties;
    using DataModel.BasicProperties;

    public class DriverCustomColorEnabledArgs : EventArgs
    {
        public DriverCustomColorEnabledArgs(string driverName, bool isCustomOutlineEnabled, [CanBeNull] ColorDto driverColor)
        {
            DriverName = driverName;
            IsCustomOutlineEnabled = isCustomOutlineEnabled;
            DriverColor = driverColor;
        }

        public string DriverName { get; }
        public bool IsCustomOutlineEnabled { get; }

        [CanBeNull]
        public ColorDto DriverColor { get; }
    }
}