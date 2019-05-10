namespace SecondMonitor.ViewModels.Controllers
{
    using AutoUpdaterDotNET;

    public class AutoUpdateController
    {
        public void CheckForUpdate()
        {
#if !DEBUG
            AutoUpdater.Start("https://gitlab.com/winzarten/SecondMonitor/raw/master/AutoUpdater.xml");
#endif
        }
    }
}