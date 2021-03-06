﻿namespace SecondMonitor.Rating.Application.Championship.Filters
{
    using Common.DataModel.Championship;
    using Common.DataModel.Championship.Events;
    using DataModel.Snapshot;

    public class DistanceRequirement : IChampionshipCondition
    {
        public string GetDescription(ChampionshipDto championshipDto)
        {
            if (championshipDto.ChampionshipState == ChampionshipState.Finished)
            {
                return string.Empty;
            }

            SessionDto sessionDto = championshipDto.Events[championshipDto.CurrentEventIndex].Sessions[championshipDto.CurrentSessionIndex];
            return $"Session length should be {sessionDto.DistanceDescription}, but this is not enforced.";
        }

        public RequirementResultKind Evaluate(ChampionshipDto championshipDto, SimulatorDataSet dataSet)
        {
            return RequirementResultKind.PerfectMatch;
        }
    }
}