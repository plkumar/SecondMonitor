namespace SecondMonitor.Rating.Application.Rating.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Controller.RaceObserver.States;
    using Rating;
    using SecondMonitor.ViewModels;

    public interface IRatingApplicationViewModel : IViewModel
    {
        bool IsVisible { get; set; }
        bool IsEnabled { get; set; }
        string CollapsedMessage { get; set; }

        IRatingViewModel SimulatorRating { get; }
        IRatingViewModel ClassRating { get; }
        IRatingViewModel DifficultyRating { get; }

        string SessionTextInformation { get; set; }
        SessionKind SessionKind { get; set; }
        SessionPhaseKind SessionPhaseKind { get; set; }
        ObservableCollection<string> SelectableClasses { get; }
        string SelectedClass { get; set; }
        bool IsClassSelectionEnable { get; set; }
        int Difficulty { get; set; }
        bool UseSuggestedDifficulty { get; set; }
        bool IsRateRaceCheckboxChecked { get; }

        ICommand ShowAllHistoryCommand { get; set; }

        ICommand ShowClassHistoryCommand { get; set; }

        ICommand ShowAllRatings { get; set; }

        void AddSelectableClass(string className);
        void ClearSelectableClasses();
        void InitializeAiDifficultySelection(int minimumLevel, int maximumLevel);


    }
}