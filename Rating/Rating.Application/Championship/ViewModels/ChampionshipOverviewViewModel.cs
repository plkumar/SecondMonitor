﻿namespace SecondMonitor.Rating.Application.Championship.ViewModels
{
    using System;
    using Common.DataModel.Championship;
    using SecondMonitor.ViewModels;

    public class ChampionshipOverviewViewModel : AbstractViewModel<ChampionshipDto>
    {
        public ChampionshipState ChampionshipState { get; set; }

        public string Name { get; private set; }

        public string Progress { get; private set; }

        public string Position { get; private set; }

        public string NextTrack { get; set; }

        public string Simulator { get; set; }

        public string ClassName { get; set; }

        protected override void ApplyModel(ChampionshipDto model)
        {
            Name = model.ChampionshipName;
            Progress = string.IsNullOrEmpty(model.NextTrack) ? "Completed" : "Next Race: " + model.CurrentRace + "/" + model.TotalRaces + " " + model.NextTrack;
            Position = model.Position == 0 ?  "-" : "Pos: " + model.Position + "/" + model.TotalDrivers;
            NextTrack = model.NextTrack;
            Simulator = model.SimulatorName;
            ChampionshipState = model.ChampionshipState;
            ClassName = model.ClassName;
        }

        public override ChampionshipDto SaveToNewModel()
        {
            throw new NotImplementedException();
        }
    }
}