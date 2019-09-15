namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Calendar.Predefined
{
    using System.Collections.Generic;
    using Common.Championship.Calendar;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;

    public class PredefinedCalendarSelectionViewModel : AbstractViewModel<CalendarTemplateGroup>
    {
        private readonly IViewModelFactory _viewModelFactory;
        private IReadOnlyCollection<CalendarTemplateGroupViewModel> _treeRoot;
        private AbstractViewModel _selectedItem;
        private CalendarPreviewViewModel _calendarPreviewViewModel;

        public PredefinedCalendarSelectionViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
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