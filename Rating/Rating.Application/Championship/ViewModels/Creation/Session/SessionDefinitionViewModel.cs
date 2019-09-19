namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Session
{
    using SecondMonitor.ViewModels;

    public class SessionDefinitionViewModel : AbstractViewModel
    {
        private string _originalSessionName;
        private string _customSessionName;

        private int _pos1Points;
        private int _pos2Points;
        private int _pos3Points;
        private int _pos4Points;
        private int _pos5Points;
        private int _pos6Points;
        private int _pos7Points;
        private int _pos8Points;
        private int _pos9Points;
        private int _pos10Points;


        public int Pos1Points
        {
            get => _pos1Points;
            set => SetProperty(ref _pos1Points, value);
        }

        public int Pos2Points
        {
            get => _pos2Points;
            set => SetProperty(ref _pos2Points, value);
        }

        public int Pos3Points
        {
            get => _pos3Points;
            set => SetProperty(ref _pos3Points, value);
        }

        public int Pos4Points
        {
            get => _pos4Points;
            set => SetProperty(ref _pos4Points, value);
        }

        public int Pos5Points
        {
            get => _pos5Points;
            set => SetProperty(ref _pos5Points, value);
        }

        public int Pos6Points
        {
            get => _pos6Points;
            set => SetProperty(ref _pos6Points, value);
        }

        public int Pos7Points
        {
            get => _pos7Points;
            set => SetProperty(ref _pos7Points, value);
        }

        public int Pos8Points
        {
            get => _pos8Points;
            set => SetProperty(ref _pos8Points, value);
        }

        public int Pos9Points
        {
            get => _pos9Points;
            set => SetProperty(ref _pos9Points, value);
        }

        public int Pos10Points
        {
            get => _pos10Points;
            set => SetProperty(ref _pos10Points, value);
        }


        public string OriginalSessionName
        {
            get => _originalSessionName;
            set => SetProperty(ref _originalSessionName, value, nameof(CustomSessionName));
        }

        public string CustomSessionName
        {
            get => string.IsNullOrWhiteSpace(_customSessionName) ? _originalSessionName : _customSessionName;
            set => SetProperty(ref _customSessionName, value);
        }

    }
}