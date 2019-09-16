namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Calendar.Predefined
{
    using System.Collections.Generic;
    using Common.Championship.Calendar;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class PredefinedCalendarSelectionViewModel : AbstractDialogViewModel<CalendarTemplateGroup>
    {
        private readonly IViewModelFactory _viewModelFactory;
        private IReadOnlyCollection<CalendarTemplateGroupViewModel> _treeRoot;
        private AbstractViewModel _selectedItem;
        private CalendarPreviewViewModel _calendarPreviewViewModel;
        private bool _autoReplaceTracks;
        private bool _useEventNames;

        public PredefinedCalendarSelectionViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            _autoReplaceTracks = true;
            _useEventNames = true;
        }

        public IReadOnlyCollection<CalendarTemplateGroupViewModel> TreeRoot
        {
            get => _treeRoot;
            set => SetProperty(ref _treeRoot, value);
        }

        public AbstractViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                RefreshSelectedEventDetails();
            }
        }

        public bool AutoReplaceTracks
        {
            get => _autoReplaceTracks;
            set => SetProperty(ref _autoReplaceTracks, value);
        }

        public bool UseEventNames
        {
            get => _useEventNames;
            set => SetProperty(ref _useEventNames, value);
        }

        public bool IsOkButtonEnabled => CalendarPreviewViewModel != null;

        public CalendarPreviewViewModel CalendarPreviewViewModel
        {
            get => _calendarPreviewViewModel;
            set => SetProperty(ref _calendarPreviewViewModel, value);
        }

        private void RefreshSelectedEventDetails()
        {
            if (_selectedItem is CalendarTemplateViewModel calendarTemplateViewModel)
            {
                CalendarPreviewViewModel = _viewModelFactory.Create<CalendarPreviewViewModel>();
                CalendarPreviewViewModel.FromModel(calendarTemplateViewModel.OriginalModel);
            }else
            {
                CalendarPreviewViewModel = null;
            }
            NotifyPropertyChanged(nameof(IsOkButtonEnabled));
        }

        protected override void ApplyModel(CalendarTemplateGroup model)
        {
            var treeRoot = _viewModelFactory.Create<CalendarTemplateGroupViewModel>();
            treeRoot.FromModel(model);
            TreeRoot = new[] {treeRoot};
        }

        public override CalendarTemplateGroup SaveToNewModel()
        {
            throw new System.NotImplementedException();
        }
    }
}