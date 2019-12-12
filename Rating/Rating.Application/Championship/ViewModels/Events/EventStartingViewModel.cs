namespace SecondMonitor.Rating.Application.Championship.ViewModels.Events
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class EventStartingViewModel : AbstractViewModel
    {
        private ICommand _closeCommand;

        public EventStartingViewModel(IViewModelFactory viewModelFactory)
        {
            Screens = new List<IViewModel>();
            EventTitleViewModel = viewModelFactory.Create<EventTitleViewModel>();
        }

        public EventTitleViewModel EventTitleViewModel { get; }

        public List<IViewModel> Screens { get; }

        public ICommand CloseCommand
        {
            get => _closeCommand;
            set => SetProperty(ref _closeCommand, value);
        }

    }
}