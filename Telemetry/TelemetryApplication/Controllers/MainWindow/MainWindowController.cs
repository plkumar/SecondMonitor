﻿namespace SecondMonitor.Telemetry.TelemetryApplication.Controllers.MainWindow
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using AggregatedChart;
    using GraphPanel;
    using LapPicker;
    using MapView;
    using SecondMonitor.ViewModels.Factory;
    using SecondMonitor.ViewModels.Settings;
    using Settings;
    using Snapshot;
    using Synchronization;
    using TelemetryLoad;
    using ViewModels;

    public class MainWindowController : IMainWindowController
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly ITelemetryLoadController _telemetryLoadController;
        private readonly ILapPickerController _lapPickerController;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IMainWindowViewModel _mainWindowViewModel;
        private readonly ISnapshotSectionController _snapshotSectionController;
        private readonly IMapViewController _mapViewController;
        private readonly ITelemetryViewsSynchronization _telemetryViewsSynchronization;
        private readonly IAggregatedChartsController _aggregatedChartsController;
        private readonly IGraphPanelController _leftGraphPanelController;
        private readonly IGraphPanelController _rightGraphPanelController;
        private readonly ISettingsController _settingsController;

        public MainWindowController(ISettingsProvider settingsProvider, ITelemetryLoadController telemetryLoadController, ILapPickerController lapPickerController, IViewModelFactory viewModelFactory, IMainWindowViewModel mainWindowViewModel,
            ISnapshotSectionController snapshotSectionController, IMapViewController mapViewController, ITelemetryViewsSynchronization telemetryViewsSynchronization, IGraphPanelController[] graphPanelControllers, IAggregatedChartsController aggregatedChartsController, ISettingsController settingsController)
        {
            _settingsProvider = settingsProvider;
            _telemetryLoadController = telemetryLoadController;
            _lapPickerController = lapPickerController;
            _viewModelFactory = viewModelFactory;
            _mainWindowViewModel = mainWindowViewModel;
            _snapshotSectionController = snapshotSectionController;
            _mapViewController = mapViewController;
            _telemetryViewsSynchronization = telemetryViewsSynchronization;
            _aggregatedChartsController = aggregatedChartsController;
            _settingsController = settingsController;

            _leftGraphPanelController = graphPanelControllers.First(x => x.IsLetPanel);
            _rightGraphPanelController = graphPanelControllers.First(x => !x.IsLetPanel);

            _snapshotSectionController.MainWindowViewModel = _mainWindowViewModel;
            _mapViewController.MapViewViewModel = _mainWindowViewModel.MapViewViewModel;
        }

        public Window MainWindow { get; set; }

        public async Task LoadTelemetrySession(string telemetryIdentifier)
        {
            await _telemetryLoadController.LoadRecentSessionAsync(telemetryIdentifier);
        }

        public async Task LoadLastTelemetrySession()
        {
            await _telemetryLoadController.LoadLastSessionAsync();
        }

        public async Task StartControllerAsync()
        {
            MainWindow.DataContext = _mainWindowViewModel;
            ShowMainWindow();
            await StartChildControllers();
        }

        public async Task StopControllerAsync()
        {
            await StopChildControllers();
        }

        private async Task StartChildControllers()
        {
            Subscribe();
            await _settingsController.StartControllerAsync();
            await _telemetryLoadController.StartControllerAsync();
            await _leftGraphPanelController.StartControllerAsync();;
            await _rightGraphPanelController.StartControllerAsync();;
            await _lapPickerController.StartControllerAsync();
            await _snapshotSectionController.StartControllerAsync();
            await _mapViewController.StartControllerAsync();
            await _aggregatedChartsController.StartControllerAsync();
        }

        private async Task StopChildControllers()
        {
            await _telemetryLoadController.StopControllerAsync();
            await _leftGraphPanelController.StopControllerAsync();
            await _rightGraphPanelController.StopControllerAsync();
            await _lapPickerController.StopControllerAsync();
            await _snapshotSectionController.StopControllerAsync();
            await _mapViewController.StopControllerAsync();;
            await _aggregatedChartsController.StopControllerAsync();
            await _settingsController.StopControllerAsync();
            UnSubscribe();
        }

        private void ShowMainWindow()
        {
            MainWindow.Show();
        }

        private void Subscribe()
        {
            _telemetryViewsSynchronization.LapLoadingStarted += TelemetryViewsSynchronizationOnLapLoadingStarted;
            _telemetryViewsSynchronization.LapLoadingFinished += _telemetryViewsSynchronization_LapLoadingFinished;
        }

        private void UnSubscribe()
        {
            _telemetryViewsSynchronization.LapLoadingStarted -= TelemetryViewsSynchronizationOnLapLoadingStarted;
            _telemetryViewsSynchronization.LapLoadingFinished -= _telemetryViewsSynchronization_LapLoadingFinished;
        }

        private void _telemetryViewsSynchronization_LapLoadingFinished(object sender, EventArgs e)
        {
            _mainWindowViewModel.IsBusy = false;
        }

        private void TelemetryViewsSynchronizationOnLapLoadingStarted(object sender, EventArgs e)
        {
            _mainWindowViewModel.IsBusy = true;
        }
    }
}