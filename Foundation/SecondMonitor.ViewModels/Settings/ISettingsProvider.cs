﻿namespace SecondMonitor.ViewModels.Settings
{
    using ViewModel;

    public interface ISettingsProvider
    {
        DisplaySettingsViewModel DisplaySettingsViewModel { get; }

        string TelemetryRepositoryPath { get; }

        string MapRepositoryPath { get; }

        string RatingsRepositoryPath { get; }

        string SimulatorContentRepository { get; }

        string TrackRecordsPath { get; }

        string CarSpecificationPath { get; }

        string ChampionshipRepositoryPath { get; }

    }
}