﻿namespace SecondMonitor.Rating.Application.Championship.Controller
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;
    using Common.DataModel.Championship;
    using SecondMonitor.ViewModels;
    using SecondMonitor.ViewModels.Controllers;
    using SecondMonitor.ViewModels.Factory;
    using ViewModels.Selection;

    public class ChampionshipSelectionController : AbstractChildController<IChampionshipController>, IChampionshipSelectionController
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IWindowService _windowService;
        private ChampionshipsSelectionViewModel _championshipsSelectionViewModel;
        private Window _selectionWindow;

        public ChampionshipSelectionController(IViewModelFactory viewModelFactory, IWindowService windowService)
        {
            _viewModelFactory = viewModelFactory;
            _windowService = windowService;
        }

        public override Task StartControllerAsync()
        {
            return Task.CompletedTask;
        }

        public override Task StopControllerAsync()
        {
            return  Task.CompletedTask;
        }

        public void ShowOrFocusSelectionDialog(IEnumerable<ChampionshipDto> championships)
        {
            if (_selectionWindow != null && _championshipsSelectionViewModel != null)
            {
                _selectionWindow.Focus();
                return;
            }

            _championshipsSelectionViewModel = _viewModelFactory.Create<ChampionshipsSelectionViewModel>();
            _championshipsSelectionViewModel.FromModel(championships);
            _selectionWindow = _windowService.OpenWindow(_championshipsSelectionViewModel, "Select Championship", WindowState.Normal, SizeToContent.WidthAndHeight, WindowStartupLocation.CenterOwner, SelectionWindowOnClose);
        }

        private void SelectionWindowOnClose()
        {
            _selectionWindow = null;
        }
    }
}