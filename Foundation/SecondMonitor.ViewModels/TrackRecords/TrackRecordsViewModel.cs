namespace SecondMonitor.ViewModels.TrackRecords
{
    using System.Windows.Input;
    using Factory;

    public class TrackRecordsViewModel : AbstractViewModel, ITrackRecordsViewModel
    {
        private bool _isVisible;

        public TrackRecordsViewModel(IViewModelFactory viewModelFactory)
        {
            VehicleRecord = viewModelFactory.Create<IRecordViewModel>();
            ClassRecord = viewModelFactory.Create<IRecordViewModel>();
            TrackRecord = viewModelFactory.Create<IRecordViewModel>();
        }
        public IRecordViewModel VehicleRecord { get;}
        public IRecordViewModel ClassRecord { get;  }
        public IRecordViewModel TrackRecord { get;  }
        public ICommand OpenTracksRecordsCommand { get; set; }
        public ICommand OpenClassRecordsCommand { get; set; }
        public ICommand OpenVehiclesRecordsCommands { get; set; }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }
    }
}