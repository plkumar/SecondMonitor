namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation
{
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class CalendarDefinitionViewModel : AbstractViewModel
    {
        public CalendarDefinitionViewModel(IViewModelFactory viewModelFactory)
        {
            CalendarViewModel = viewModelFactory.Create<CreatedCalendarViewModel>();
            AvailableTracksViewModel = viewModelFactory.Create<AvailableTracksViewModel>();
        }

        public CreatedCalendarViewModel CalendarViewModel { get; }
        public AvailableTracksViewModel AvailableTracksViewModel { get; }
    }
}