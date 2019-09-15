namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Calendar.Predefined
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Championship.Calendar;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class CalendarTemplateGroupViewModel : AbstractViewModel<CalendarTemplateGroup>
    {
        private readonly IViewModelFactory _viewModelFactory;
        private string _title;
        private bool _hasChildGroups;
        private bool _hasChildEntries;

        private IReadOnlyCollection<CalendarTemplateGroupViewModel> _childGroups;
        private IReadOnlyCollection<CalendarTemplateViewModel> _childCalendarEntries;

        public CalendarTemplateGroupViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public bool HasChildGroups
        {
            get => _hasChildGroups;
            set => SetProperty(ref _hasChildGroups, value);
        }

        public bool HasChildEntries
        {
            get => _hasChildEntries;
            set => SetProperty(ref _hasChildEntries, value);
        }

        public IReadOnlyCollection<CalendarTemplateGroupViewModel> ChildGroups
        {
            get => _childGroups;
            set => SetProperty(ref _childGroups, value);
        }

        public IReadOnlyCollection<CalendarTemplateViewModel> ChildCalendarEntries
        {
            get => _childCalendarEntries;
            set => SetProperty(ref _childCalendarEntries, value);
        }

        public IReadOnlyCollection<AbstractViewModel> AllChildEntries => ChildGroups.OfType<AbstractViewModel>().Concat(ChildCalendarEntries).ToList();

        protected override void ApplyModel(CalendarTemplateGroup model)
        {
            Title = model.GroupName;
            HasChildEntries = model.ChildCalendars.Count > 0;
            HasChildGroups = model.ChildGroups.Count > 0;
            ChildGroups = model.ChildGroups.OrderBy(x => x.GroupName).Select(x =>
            {
                var newViewModel = _viewModelFactory.Create<CalendarTemplateGroupViewModel>();
                newViewModel.FromModel(x);
                return newViewModel;
            }).ToList();

            ChildCalendarEntries = model.ChildCalendars.OrderBy(x => x.CalendarName).Select(x =>
            {
                var newViewModel = _viewModelFactory.Create<CalendarTemplateViewModel>();
                newViewModel.FromModel(x);
                return newViewModel;
            }).ToList();
        }

        public override CalendarTemplateGroup SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}