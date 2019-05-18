﻿namespace SecondMonitor.Rating.Application.Controller.RaceObserver.States
{
    using System.Collections.Generic;
    using System.Linq;
    using Context;
    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using DataModel.Summary;
    using NLog;

    public class QualificationState : AbstractSessionTypeState
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private bool _sessionCompleted;
        public QualificationState(SharedContext sharedContext) : base(sharedContext)
        {
            _sessionCompleted = false;
        }

        public override SessionKind SessionKind { get; protected set; } = SessionKind.Qualification;
        public override SessionPhaseKind SessionPhaseKind { get; protected set; }
        public override bool CanUserSelectClass => false;
        protected override SessionType SessionType => SessionType.Qualification;
        public override bool ShowRatingChange => false;

        protected override void Initialize(SimulatorDataSet simulatorDataSet)
        {
            SharedContext.QualificationContext = new QualificationContext() { QualificationDifficulty = SharedContext.UserSelectedDifficulty };
            SessionDescription = SharedContext.QualificationContext.QualificationDifficulty.ToString();
        }

        public override bool DoSessionCompletion(SessionSummary sessionSummary)
        {
            if (_sessionCompleted)
            {
                return false;
            }

            _sessionCompleted = true;
            if (!IsSessionResultAcceptable(sessionSummary))
            {
                Logger.Info("Seesion is not acceptable to be used by rating calculations. Not enough drivers with laps times");
                SharedContext.QualificationContext = null;
                return false;
            }

            List<Driver> eligibleDrivers = FilterNotEligibleDriversAndOrder(sessionSummary);
            if (eligibleDrivers == null)
            {
                Logger.Info("Seesion is not acceptable to be used by rating calculations - no drivers");
                SharedContext.QualificationContext = null;
                return false;
            }

            Logger.Info("Seesion is acceptable to be used by rating calculations.");
            SharedContext.QualificationContext.LastQualificationResult = eligibleDrivers;
            return false;
        }

        private bool IsSessionResultAcceptable(SessionSummary sessionSummary)
        {
            return sessionSummary.Drivers.Count(x => x.BestPersonalLap != null) > sessionSummary.Drivers.Count / 2;
        }

        private List<Driver> FilterNotEligibleDriversAndOrder(SessionSummary sessionSummary)
        {
            if (!sessionSummary.IsMultiClass)
            {
                return sessionSummary.Drivers.OrderBy(x => x.FinishingPosition).ToList();
            }

            Driver player = sessionSummary.Drivers.FirstOrDefault(x => x.IsPlayer);
            return player == null ? null : sessionSummary.Drivers.Where(x => x.ClassId == player.ClassId).OrderBy(x => x.FinishingPosition).ToList();
        }

    }
}