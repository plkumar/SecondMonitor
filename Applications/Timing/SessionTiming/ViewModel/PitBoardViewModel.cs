namespace SecondMonitor.Timing.SessionTiming.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using DataModel.Extensions;
    using Drivers.Presentation.ViewModel;
    using Presentation.ViewModel;
    using ViewModels;
    using ViewModels.PitBoard;

    public class PitBoardViewModel : AbstractViewModel
    {
        private bool _isPitBoardVisible;
        private readonly Dictionary<string, TimeSpan> _lastGapToPlayer;

        public PitBoardViewModel()
        {
            PitBoard = new RacePitBoardViewModel();
            _lastGapToPlayer = new Dictionary<string, TimeSpan>();
        }

        public bool IsPitBoardVisible
        {
            get => _isPitBoardVisible;
            set => SetProperty(ref _isPitBoardVisible, value);
        }

        public RacePitBoardViewModel PitBoard { get; }

        public void UpdatePitBoard(IEnumerable<DriverTimingViewModel> driverTimingViewModels)
        {
            var orderedDriverTimings = driverTimingViewModels.OrderBy(x => x.DriverTiming.Position).ToArray();
            var player = orderedDriverTimings.FirstOrDefault(x => x.IsPlayer);
            if (player == null)
            {
                return;
            }

            PitBoard.Lap = "L" + (player.DriverTiming.CompletedLaps + 1);
            PitBoard.Position = "P" + player.PositionInClass;

            int playerIndex = Array.IndexOf(orderedDriverTimings, player);

            if (playerIndex > 0)
            {
                var driverBefore = orderedDriverTimings[playerIndex - 1];
                PitBoard.GapAhead = driverBefore.GapToPlayer.Duration().FormatTimeSpanOnlySecondNoMiliseconds(false);
                PitBoard.GapAheadChange = (-GetGapForDriverChange(driverBefore)).FormatTimeSpanOnlySecondNoMiliseconds(true);
            }
            else
            {
                PitBoard.GapAhead = string.Empty;
                PitBoard.GapAheadChange = string.Empty;
            }

            if (playerIndex < orderedDriverTimings.Length - 1)
            {
                var driverAfter = orderedDriverTimings[playerIndex + 1];
                PitBoard.GapBehind = driverAfter.GapToPlayer.Duration().FormatTimeSpanOnlySecondNoMiliseconds(false);
                PitBoard.GapBehindChange = GetGapForDriverChange(driverAfter).FormatTimeSpanOnlySecondNoMiliseconds(true);
            }
            else
            {
                PitBoard.GapBehind = string.Empty;
                PitBoard.GapBehindChange = string.Empty;
            }

            UpdateGapToPlayer(orderedDriverTimings);
        }

        public async Task ShowPitBoard(TimeSpan visibleTime, CancellationToken cancellationToken)
        {
            IsPitBoardVisible = true;
            await Task.Delay(visibleTime, cancellationToken);
            IsPitBoardVisible = false;
        }

        private TimeSpan GetGapForDriverChange(DriverTimingViewModel driver)
        {
            if (_lastGapToPlayer.TryGetValue(driver.Name, out TimeSpan timeSpan))
            {
                return driver.GapToPlayer - timeSpan;
            }

            return driver.GapToPlayer.Duration();
        }

        private void UpdateGapToPlayer(DriverTimingViewModel[] driverTimingViewModels)
        {
            foreach (DriverTimingViewModel driverTimingViewModel in driverTimingViewModels)
            {
                if (driverTimingViewModel.IsPlayer)
                {
                    continue;
                }

                _lastGapToPlayer[driverTimingViewModel.Name] = driverTimingViewModel.GapToPlayer;
            }
        }

        public void Reset()
        {
            _lastGapToPlayer.Clear();
        }
    }
}