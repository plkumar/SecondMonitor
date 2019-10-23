namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.DataModel.Championship;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Drivers;
    using Filters;
    using Pool;
    using SecondMonitor.ViewModels.Controllers;
    using SecondMonitor.ViewModels.Factory;
    using SecondMonitor.ViewModels.SessionEvents;
    using ViewModels.IconState;

    public class ChampionshipController : IChampionshipController
    {
        private readonly ISessionEventProvider _sessionEventProvider;
        private readonly IChampionshipsPool _championshipsPool;
        private readonly IChampionshipEligibilityEvaluator _championshipEligibilityEvaluator;
        private readonly IChampionshipOverviewController _championshipOverviewController;
        private readonly IChampionshipEventController _championshipEventController;
        private readonly IChampionshipSelectionController _championshipSelectionController;
        private List<ChampionshipDto> _championshipCandidates;

        public ChampionshipController(IViewModelFactory viewModelFactory, IChildControllerFactory childControllerFactory, ISessionEventProvider sessionEventProvider, IChampionshipsPool championshipsPool, IChampionshipEligibilityEvaluator championshipEligibilityEvaluator)
        {
            _championshipCandidates = new List<ChampionshipDto>();
            _sessionEventProvider = sessionEventProvider;
            _championshipsPool = championshipsPool;
            _championshipEligibilityEvaluator = championshipEligibilityEvaluator;
            ChampionshipIconStateViewModel =  viewModelFactory.Create<ChampionshipIconStateViewModel>();
            SetChampionshipIconToNone();
            _championshipOverviewController = childControllerFactory.Create<IChampionshipOverviewController, IChampionshipController>(this);
            _championshipEventController = childControllerFactory.Create<IChampionshipEventController, IChampionshipController>(this);
            _championshipSelectionController = childControllerFactory.Create<IChampionshipSelectionController, IChampionshipController>(this);
        }

        public ChampionshipIconStateViewModel ChampionshipIconStateViewModel { get; }


        public async Task StartControllerAsync()
        {
            _sessionEventProvider.DriversAdded += ReEvaluateChampionships;
            _sessionEventProvider.DriversRemoved += ReEvaluateChampionships;
            _sessionEventProvider.PlayerFinishStateChanged += ReEvaluateChampionships;
            _sessionEventProvider.PlayerPropertiesChanged += ReEvaluateChampionships;
            await StartChildControllersAsync();
        }

        public async Task StopControllerAsync()
        {
            _sessionEventProvider.DriversAdded -= ReEvaluateChampionships;
            _sessionEventProvider.DriversRemoved -= ReEvaluateChampionships;
            _sessionEventProvider.PlayerFinishStateChanged -= ReEvaluateChampionships;
            _sessionEventProvider.PlayerPropertiesChanged -= ReEvaluateChampionships;
            await StopChildControllersAsync();
        }

        public void OpenChampionshipWindow()
        {
            if (_championshipEventController.IsChampionshipActive && ChampionshipIconStateViewModel.ChampionshipIconState == ChampionshipIconState.ChampionshipInProgress)
            {
                OpenRunningChampionshipDetail();
                return;
            }

            if (_championshipCandidates.Count > 0)
            {
                OpenCandidatesSelector();
                return;
            }
            _championshipOverviewController.OpenChampionshipOverviewWindow();
        }

        private void OpenRunningChampionshipDetail()
        {
            _championshipOverviewController.OpenChampionshipDetailsWindow(_championshipEventController.CurrentChampionship);
        }

        public void StartNextEvent(ChampionshipDto championship)
        {
            _championshipCandidates.Clear();
            _championshipEventController.StartNextEvent(championship);
            SetIconToRunning();
        }

        public void EventFinished(ChampionshipDto championship)
        {
            if (championship != null)
            {
                _championshipsPool.UpdateChampionship(championship);
            }

            ReEvaluateChampionships(_sessionEventProvider.LastDataSet);
        }

        protected async Task StartChildControllersAsync()
        {
            await _championshipOverviewController.StartControllerAsync();
            await _championshipEventController.StartControllerAsync();
        }

        protected async Task StopChildControllersAsync()
        {
            await _championshipOverviewController.StopControllerAsync();
            await _championshipOverviewController.StopControllerAsync();
        }

        private void ReEvaluateChampionships(object sender, DataSetArgs e)
        {
            ReEvaluateChampionships(e.DataSet);
        }

        private void ReEvaluateChampionships(object sender, DriversArgs e)
        {
            ReEvaluateChampionships(e.DataSet);
        }

        private void SetChampionshipIconToNone()
        {
            _championshipCandidates.Clear();
            ChampionshipIconStateViewModel.ChampionshipIconState = ChampionshipIconState.None;
            ChampionshipIconStateViewModel.TooltipText = "Opens all championships overview.";
        }

        private void ReEvaluateChampionships(SimulatorDataSet dataSet)
        {
            if (dataSet.SessionInfo.SessionType == SessionType.Na)
            {
                SetChampionshipIconToNone();
                return;
            }

            if (dataSet.PlayerInfo?.FinishStatus == DriverFinishStatus.Finished)
            {
                SetChampionshipIconToNone();
                return;
            }

            if (_championshipEventController.IsChampionshipActive)
            {
                SetIconToRunning();
                return;
            }

            if (_championshipEventController.TryResumePreviousChampionship(dataSet))
            {
                SetIconToRunning();
                return;
            }

            List<ChampionshipDto> perfectlyMatchingChampionships = new List<ChampionshipDto>();
            List<ChampionshipDto> matchingChampionships = new List<ChampionshipDto>();

            foreach (ChampionshipDto championship in _championshipsPool.GetAllChampionshipDtos())
            {
                RequirementResultKind evaluationResult = _championshipEligibilityEvaluator.EvaluateChampionship(championship, dataSet);
                switch (evaluationResult)
                {
                    case RequirementResultKind.DoesNotMatch:
                        continue;
                    case RequirementResultKind.PerfectMatch:
                        perfectlyMatchingChampionships.Add(championship);
                        matchingChampionships.Add(championship);
                        break;
                    case RequirementResultKind.CanMatch:
                        matchingChampionships.Add(championship);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (perfectlyMatchingChampionships.Count == 1)
            {
                StartNextEvent(perfectlyMatchingChampionships[0]);
                return;
            }

            _championshipCandidates = matchingChampionships;
            if (_championshipCandidates.Count == 0)
            {
                SetChampionshipIconToNone();
                return;
            }


            ChampionshipIconStateViewModel.ChampionshipIconState = ChampionshipIconState.PotentialChampionship;
            ChampionshipIconStateViewModel.TooltipText = "Opens championships eligible for current session";
        }

        private void SetIconToRunning()
        {

            ChampionshipIconStateViewModel.ChampionshipIconState = ChampionshipIconState.ChampionshipInProgress;
            ChampionshipIconStateViewModel.TooltipText = "Opens the overview of the currently running championship";
        }

        private void OpenCandidatesSelector()
        {
            _championshipSelectionController.ShowOrFocusSelectionDialog(_championshipCandidates);
        }
    }
}