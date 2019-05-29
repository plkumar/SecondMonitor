namespace SecondMonitor.ViewModels.RaceSuggestion
{
    using System.Collections.Generic;
    using System.Windows.Input;

    public class RaceSuggestionViewModel : AbstractViewModel, IRaceSuggestionViewModel
    {
        private bool _isVisible;
        private bool _randomizeSimulator;
        private bool _randomizeClass;
        private bool _randomizeTrack;
        private string _selectedSimulator;
        private string _selectedClass;
        private string _selectedTrack;
        private IEnumerable<string> _availableSimulators;
        private IEnumerable<string> _availableClasses;
        private IEnumerable<string> _availableTracks;
        private ICommand _randomizeCommand;

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public bool RandomizeSimulator
        {
            get => _randomizeSimulator;
            set => SetProperty(ref _randomizeSimulator, value);
        }

        public bool RandomizeClass
        {
            get => _randomizeClass;
            set => SetProperty(ref _randomizeClass, value);
        }

        public bool RandomizeTrack
        {
            get => _randomizeTrack;
            set => SetProperty(ref _randomizeTrack, value);
        }

        public string SelectedSimulator
        {
            get => _selectedSimulator;
            set => SetProperty(ref _selectedSimulator, value);
        }

        public string SelectedClass
        {
            get => _selectedClass;
            set => SetProperty(ref _selectedClass, value);
        }

        public string SelectedTrack
        {
            get => _selectedTrack;
            set => SetProperty(ref _selectedTrack, value);
        }

        public IEnumerable<string> AvailableSimulators
        {
            get => _availableSimulators;
            set => SetProperty(ref _availableSimulators, value);
        }

        public IEnumerable<string> AvailableClasses
        {
            get => _availableClasses;
            set => SetProperty(ref _availableClasses, value);
        }

        public IEnumerable<string> AvailableTracks
        {
            get => _availableTracks;
            set => SetProperty(ref _availableTracks, value);
        }

        public ICommand RandomizeCommand
        {
            get => _randomizeCommand;
            set => SetProperty(ref _randomizeCommand, value);
        }
    }
}