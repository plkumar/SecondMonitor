namespace ControlTestingApp.ViewModels
{
    using SecondMonitor.Contracts.NInject;
    using SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Calendar.Predefined;
    using SecondMonitor.Rating.Common.Championship.Calendar.Templates.CalendarGroups;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class MainTestWindowViewModel : AbstractViewModel
    {
        private readonly KernelWrapper _kernelWrapper;
        private readonly IViewModelFactory _viewModelFactory;
        public MainTestWindowViewModel()
        {
            _kernelWrapper = new KernelWrapper();
            _viewModelFactory = _kernelWrapper.Get<IViewModelFactory>();

            CalendarTemplateGroupViewModel = _viewModelFactory.Create<PredefinedCalendarSelectionViewModel>();
            CalendarTemplateGroupViewModel.FromModel(AllGroups.MainGroup);

            SequenceViewTestViewModel = new SequenceViewTestViewModel();
        }

        public PredefinedCalendarSelectionViewModel CalendarTemplateGroupViewModel { get; }

        public SequenceViewTestViewModel SequenceViewTestViewModel { get;  }
    }
}