namespace SecondMonitor.ViewModels.PitBoard
{
    public class RacePitBoardViewModel : AbstractViewModel
    {
        private string _position;
        private string _lap;
        private string _gapAhead;
        private string _gapAheadChange;
        private string _gapBehind;
        private string _gapBehindChange;

        public string Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        public string Lap
        {
            get => _lap;
            set => SetProperty(ref _lap, value);
        }

        public string GapAhead
        {
            get => _gapAhead;
            set => SetProperty(ref _gapAhead, value);
        }

        public string GapAheadChange
        {
            get => _gapAheadChange;
            set => SetProperty(ref _gapAheadChange, value);
        }

        public string GapBehind
        {
            get => _gapBehind;
            set => SetProperty(ref _gapBehind, value);
        }

        public string GapBehindChange
        {
            get => _gapBehindChange;
            set => SetProperty(ref _gapBehindChange, value);
        }
    }
}