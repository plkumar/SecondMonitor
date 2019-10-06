namespace ControlTestingApp.ViewModels
{
    using System.Collections.Generic;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.SplashScreen;

    public class SequenceViewTestViewModel
    {
        public SequenceViewTestViewModel()
        {
            Views = new List<IViewModel>()
            {
                new SplashScreenViewModel(){PrimaryInformation = "First View Model"},
                new SplashScreenViewModel(){PrimaryInformation = "Second View Model"},
                new SplashScreenViewModel(){PrimaryInformation = "Third View Model"}
            };
        }

        public List<IViewModel> Views { get; }
    }
}