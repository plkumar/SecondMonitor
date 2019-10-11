namespace SecondMonitor.Rating.Application.Championship.ViewModels.Events
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using SecondMonitor.ViewModels;

    public class EventStartingViewModel : AbstractViewModel
    {
        private ICommand _closeCommand;

        public EventStartingViewModel()
        {
            Screens = new List<IViewModel>();
        }

        public string ChampionshipName { get; set; }

        public string EventName { get; set; }

        public string EventIndex { get; set; }

        public string SessionName { get; set; }

        public string SessionIndex { get; set; }

        public List<IViewModel> Screens { get; }

        public ICommand CloseCommand
        {
            get => _closeCommand;
            set => SetProperty(ref _closeCommand, value);
        }

    }
}