namespace SecondMonitor.ViewModels.TrackRecords
{
    using System.Windows.Input;

    public interface ITrackRecordsViewModel : IViewModel
    {
        IRecordViewModel VehicleRecord { get; }
        IRecordViewModel ClassRecord { get; }
        IRecordViewModel TrackRecord { get; }

        ICommand OpenTracksRecordsCommand { get; set; }
        ICommand OpenClassRecordsCommand { get; set; }

        ICommand OpenVehiclesRecordsCommands { get; set; }

        bool IsVisible { get; set; }
    }
}