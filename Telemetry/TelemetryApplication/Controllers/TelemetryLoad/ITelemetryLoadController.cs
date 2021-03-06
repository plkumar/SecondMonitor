﻿namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.TelemetryLoad
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using SecondMonitor.ViewModels.Controllers;
    using TelemetryManagement.DTO;

    public interface ITelemetryLoadController : IController
    {
        Task<IReadOnlyCollection<SessionInfoDto>> GetAllRecentSessionInfoAsync();
        Task<IReadOnlyCollection<SessionInfoDto>> GetAllArchivedSessionInfoAsync();
        Task RefreshLoadedSessions();

        Task<SessionInfoDto> LoadRecentSessionAsync(string sessionIdentifier);
        Task<SessionInfoDto> LoadRecentSessionAsync(SessionInfoDto sessionInfoDto);
        Task<SessionInfoDto> AddRecentSessionAsync(SessionInfoDto sessionInfoDto);
        Task<SessionInfoDto> LoadLastSessionAsync();

        Task<LapTelemetryDto> LoadLap(LapSummaryDto lapSummaryDto);
        Task<LapTelemetryDto> LoadLap(FileInfo file, string customDisplayName);

        Task UnloadLap(LapSummaryDto lapSummaryDto);
        Task ArchiveSession(SessionInfoDto sessionInfoDto);
        Task OpenSessionFolder(SessionInfoDto sessionInfoDto);
        void DeleteSession(SessionInfoDto sessionInfoDto);
    }
}