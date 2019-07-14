namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.Settings
{
    using System;

    public class SettingChangedArgs : EventArgs
    {
        public SettingChangedArgs(RequestedAction requestedAction)
        {
            RequestedAction = requestedAction;
        }

        public RequestedAction RequestedAction { get; }
    }
}