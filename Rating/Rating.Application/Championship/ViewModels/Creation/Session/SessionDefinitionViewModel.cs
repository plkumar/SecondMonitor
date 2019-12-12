namespace SecondMonitor.Rating.Application.Championship.ViewModels.Creation.Session
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Championship.Scoring.Templates;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Factory;
    using SessionLength;

    public class SessionDefinitionViewModel : AbstractViewModel
    {
        private readonly IViewModelFactory _viewModelFactory;
        private string _originalSessionName;
        private string _customSessionName;
        private ScoringTemplate _selectedScoringTemplate;

        private int[] _points;


        private ISessionLengthDefinitionViewModel _selectedSessionLengthDefinitionViewModel;

        public SessionDefinitionViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            AvailableSessionLengthDefinitionViewModels = _viewModelFactory.CreateAll<ISessionLengthDefinitionViewModel>().ToList();
            SelectedSessionLengthDefinitionViewModel = AvailableSessionLengthDefinitionViewModels.First();
            _points = new int[15];
            ScoringTemplates = Common.Championship.Scoring.Templates.ScoringTemplates.AllTemplates.OrderBy(x => x.Name).ToArray();
        }

        public IReadOnlyCollection<ISessionLengthDefinitionViewModel> AvailableSessionLengthDefinitionViewModels { get; set; }

        public ISessionLengthDefinitionViewModel SelectedSessionLengthDefinitionViewModel
        {
            get => _selectedSessionLengthDefinitionViewModel;
            set => SetProperty(ref _selectedSessionLengthDefinitionViewModel, value);
        }

        public ScoringTemplate[] ScoringTemplates { get; }

        public ScoringTemplate SelectedScoringTemplate
        {
            get => _selectedScoringTemplate;
            set
            {
                SetProperty(ref _selectedScoringTemplate, value);
                ApplyScoringTemplate();
            }
        }

        public int Pos1Points
        {
            get => _points[0];
            set => SetProperty(ref _points[0], value);
        }

        public int Pos2Points
        {
            get => _points[1];
            set => SetProperty(ref _points[1], value);
        }

        public int Pos3Points
        {
            get => _points[2];
            set => SetProperty(ref _points[2], value);
        }

        public int Pos4Points
        {
            get => _points[3];
            set => SetProperty(ref _points[3], value);
        }

        public int Pos5Points
        {
            get => _points[4];
            set => SetProperty(ref _points[4], value);
        }

        public int Pos6Points
        {
            get => _points[5];
            set => SetProperty(ref _points[5], value);
        }

        public int Pos7Points
        {
            get => _points[6];
            set => SetProperty(ref _points[6], value);
        }

        public int Pos8Points
        {
            get => _points[7];
            set => SetProperty(ref _points[7], value);
        }

        public int Pos9Points
        {
            get => _points[8];
            set => SetProperty(ref _points[8], value);
        }

        public int Pos10Points
        {
            get => _points[9];
            set => SetProperty(ref _points[9], value);
        }

        public int Pos11Points
        {
            get => _points[10];
            set => SetProperty(ref _points[10], value);
        }

        public int Pos12Points
        {
            get => _points[11];
            set => SetProperty(ref _points[11], value);
        }

        public int Pos13Points
        {
            get => _points[12];
            set => SetProperty(ref _points[12], value);
        }

        public int Pos14Points
        {
            get => _points[13];
            set => SetProperty(ref _points[13], value);
        }

        public int Pos15Points
        {
            get => _points[14];
            set => SetProperty(ref _points[14], value);
        }

        public int[] Scoring => _points;


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

        private void ApplyScoringTemplate()
        {
            ScoringTemplate scoringTemplate = _selectedScoringTemplate;

            if (scoringTemplate == null)
            {
                return;
            }

            _points = Enumerable.Repeat(0, 15).ToArray();
            for (int i = 0; i < scoringTemplate.Scoring.Length; i++)
            {
                _points[i] = scoringTemplate.Scoring[i];
            }
            NotifyPropertyChanged(string.Empty);
        }

    }
}