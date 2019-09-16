namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Calendar
{
    using Common.Championship.Calendar;

    public interface ICalendarEntryViewModelFactory
    {
        AbstractCalendarEntryViewModel Create(AbstractTrackTemplateViewModel trackTemplate);
        AbstractCalendarEntryViewModel Create(EventTemplate eventTemplate, string simulatorName);
    }
}