namespace SecondMonitor.ViewModels.RaceSuggestion
{
    using System.Collections.Generic;
    using System.Windows.Input;

    public interface IRaceSuggestionViewModel : IViewModel
    {
        bool IsVisible { get; set; }

        bool RandomizeSimulator { get; set; }
        bool RandomizeClass { get; set; }
        bool RandomizeTrack { get; set; }

        string SelectedSimulator { get; set; }
        string SelectedClass { get; set; }

        string SelectedTrack { get; set; }

        IEnumerable<string> AvailableSimulators { get; set; }

        IEnumerable<string> AvailableClasses { get; set; }

        IEnumerable<string> AvailableTracks { get; set; }

        ICommand RandomizeCommand { get; set; }
    }

}