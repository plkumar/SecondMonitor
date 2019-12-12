namespace SecondMonitor.Rating.Application.Championship.ViewModels.Events
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using SecondMonitor.ViewModels;

    public class SessionCompletedViewModel : AbstractViewModel
    {
        private ICommand _closeCommand;

        public SessionCompletedViewModel()
        {
            Screens = new List<IViewModel>();
        }

        public string Title { get; set; }
        public List<IViewModel> Screens { get; }

        public ICommand CloseCommand
        {
            get => _closeCommand;
            set => SetProperty(ref _closeCommand, value);
        }
    }
}