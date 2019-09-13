namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Calendar
{
    public interface ICalendarEntryViewModelFactory
    {
        AbstractCalendarEntryViewModel Create(AbstractTrackTemplateViewModel trackTemplate);
    }
}