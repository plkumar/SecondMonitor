namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Histogram.Providers
{
    using System.Collections.Generic;
    using Controllers.Settings;
    using DataModel.BasicProperties;
    using Extractors;
    using Filter;
    using SecondMonitor.ViewModels.Factory;
    using Settings.DTO.CarProperties;
    using TelemetryManagement.DTO;
    using ViewModels.AggregatedCharts.Histogram;
    using ViewModels.LoadedLapCache;

    public class CamberHistogramProvider : AbstractWheelHistogramProvider<CamberWheelsChartViewModel, CamberHistogramChartViewModel>
    {
        private readonly CamberHistogramExtractor _camberHistogramExtractor;
        private readonly LoadedWheelFilter _loadedWheelFilter;
        private readonly CamberFilter _camberFilter;
        private readonly ISettingsController _settingsController;
        private CarPropertiesDto _currentCar;

        public CamberHistogramProvider(CamberHistogramExtractor camberHistogramExtractor, ILoadedLapsCache loadedLapsCache, IViewModelFactory viewModelFactory,  LoadedWheelFilter loadedWheelFilter, CamberFilter camberFilter, ISettingsController settingsController) : base(camberHistogramExtractor, loadedLapsCache, viewModelFactory, new IWheelTelemetryFilter[] {loadedWheelFilter, camberFilter})
        {
            _camberHistogramExtractor = camberHistogramExtractor;
            _loadedWheelFilter = loadedWheelFilter;
            _camberFilter = camberFilter;
            _settingsController = settingsController;
        }

        public override string ChartName => "Camber Histogram";
        public override AggregatedChartKind Kind => AggregatedChartKind.Histogram;
        protected override bool ResetCommandVisible => true;
        protected override void OnNewViewModel(CamberWheelsChartViewModel newViewModel)
        {
            _currentCar = _settingsController.GetCarPropertiesForCurrentCar();

            newViewModel.IsLoadedChecked = _currentCar.ChartsProperties.CamberHistogram.IsLoadedSelected;
            newViewModel.IsUnloadedChecked = _currentCar.ChartsProperties.CamberHistogram.IsUnloadedSelected;
            newViewModel.FromG = _currentCar.ChartsProperties.CamberHistogram.FromG;
            newViewModel.ToG = _currentCar.ChartsProperties.CamberHistogram.ToG;
            newViewModel.BandSize = _currentCar.ChartsProperties.CamberHistogram.BandSize.GetValueInUnits(_camberHistogramExtractor.AngleUnits);
            newViewModel.FromCamber = _currentCar.ChartsProperties.CamberHistogram.FromCamber.GetValueInUnits(_camberHistogramExtractor.AngleUnits);
            newViewModel.ToCamber = _currentCar.ChartsProperties.CamberHistogram.ToCamber.GetValueInUnits(_camberHistogramExtractor.AngleUnits);

            ((CamberHistogramChartViewModel) newViewModel.FrontLeftChartViewModel).IdealCamber = _currentCar.FrontLeftTyre.IdealCamber.GetValueInUnits(_camberHistogramExtractor.AngleUnits);
            ((CamberHistogramChartViewModel)newViewModel.FrontRightChartViewModel).IdealCamber = _currentCar.FrontRightTyre.IdealCamber.GetValueInUnits(_camberHistogramExtractor.AngleUnits);
            ((CamberHistogramChartViewModel)newViewModel.RearLeftChartViewModel).IdealCamber = _currentCar.RearLeftTyre.IdealCamber.GetValueInUnits(_camberHistogramExtractor.AngleUnits);
            ((CamberHistogramChartViewModel)newViewModel.RearRightChartViewModel).IdealCamber = _currentCar.RearRightTyre.IdealCamber.GetValueInUnits(_camberHistogramExtractor.AngleUnits);

            ((CamberHistogramChartViewModel)newViewModel.FrontLeftChartViewModel).AngleUnits = newViewModel.Unit;
            ((CamberHistogramChartViewModel)newViewModel.FrontRightChartViewModel).AngleUnits = newViewModel.Unit;
            ((CamberHistogramChartViewModel)newViewModel.RearLeftChartViewModel).AngleUnits = newViewModel.Unit;
            ((CamberHistogramChartViewModel)newViewModel.RearRightChartViewModel).AngleUnits = newViewModel.Unit;
        }

        protected override void RefreshHistogram(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, CamberWheelsChartViewModel wheelsChart)
        {
            _currentCar.ChartsProperties.CamberHistogram.IsLoadedSelected = wheelsChart.IsLoadedChecked;
            _currentCar.ChartsProperties.CamberHistogram.IsUnloadedSelected = wheelsChart.IsUnloadedChecked;
            _currentCar.ChartsProperties.CamberHistogram.FromG = wheelsChart.FromG;
            _currentCar.ChartsProperties.CamberHistogram.ToG = wheelsChart.ToG;
            _currentCar.ChartsProperties.CamberHistogram.BandSize = Angle.GetFromValue(wheelsChart.BandSize, _camberHistogramExtractor.AngleUnits);
            _currentCar.ChartsProperties.CamberHistogram.FromCamber = Angle.GetFromValue(wheelsChart.FromCamber, _camberHistogramExtractor.AngleUnits);
            _currentCar.ChartsProperties.CamberHistogram.ToCamber = Angle.GetFromValue(wheelsChart.ToCamber, _camberHistogramExtractor.AngleUnits);

            _currentCar.FrontLeftTyre.IdealCamber = Angle.GetFromValue(((CamberHistogramChartViewModel)wheelsChart.FrontLeftChartViewModel).IdealCamber, _camberHistogramExtractor.AngleUnits);
            _currentCar.FrontRightTyre.IdealCamber = Angle.GetFromValue(((CamberHistogramChartViewModel)wheelsChart.FrontRightChartViewModel).IdealCamber, _camberHistogramExtractor.AngleUnits);
            _currentCar.RearLeftTyre.IdealCamber = Angle.GetFromValue(((CamberHistogramChartViewModel)wheelsChart.RearLeftChartViewModel).IdealCamber, _camberHistogramExtractor.AngleUnits);
            _currentCar.RearRightTyre.IdealCamber = Angle.GetFromValue(((CamberHistogramChartViewModel)wheelsChart.RearRightChartViewModel).IdealCamber, _camberHistogramExtractor.AngleUnits);
            base.RefreshHistogram(loadedLaps, bandSize, wheelsChart);
        }

        protected override void ResetHistogramParameters(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, CamberWheelsChartViewModel wheelsChart)
        {
            _currentCar = _settingsController.ResetAndGetCarPropertiesForCurrentCar();
            OnNewViewModel(wheelsChart);
            base.ResetHistogramParameters(loadedLaps, bandSize, wheelsChart);
        }

        protected override Histogram ExtractFlHistogram(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, CamberWheelsChartViewModel wheelsChart)
        {
            Filters.ForEach(x => x.FilterFrontLeft());
            _camberHistogramExtractor.IdealCamber = ((CamberHistogramChartViewModel)wheelsChart.FrontLeftChartViewModel).IdealCamber;
            return _camberHistogramExtractor.ExtractHistogramFrontLeft(loadedLaps, bandSize, Filters);
        }

        protected override Histogram ExtractFrHistogram(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, CamberWheelsChartViewModel wheelsChart)
        {
            Filters.ForEach(x => x.FilterFrontRight());
            _camberHistogramExtractor.IdealCamber = ((CamberHistogramChartViewModel)wheelsChart.FrontRightChartViewModel).IdealCamber;
            return _camberHistogramExtractor.ExtractHistogramFrontRight(loadedLaps, bandSize, Filters);
        }

        protected override Histogram ExtractRlHistogram(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, CamberWheelsChartViewModel wheelsChart)
        {
            Filters.ForEach(x => x.FilterRearLeft());
            _camberHistogramExtractor.IdealCamber = ((CamberHistogramChartViewModel)wheelsChart.RearLeftChartViewModel).IdealCamber;
            return _camberHistogramExtractor.ExtractHistogramRearLeft(loadedLaps, bandSize, Filters);
        }

        protected override Histogram ExtractRrHistogram(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, CamberWheelsChartViewModel wheelsChart)
        {
            Filters.ForEach(x => x.FilterRearRight());
            _camberHistogramExtractor.IdealCamber = ((CamberHistogramChartViewModel)wheelsChart.RearRightChartViewModel).IdealCamber;
            return _camberHistogramExtractor.ExtractHistogramRearRight(loadedLaps, bandSize, Filters);
        }

        protected override void BeforeHistogramFilling(CamberWheelsChartViewModel wheelsChart)
        {
            _loadedWheelFilter.MinimumG = wheelsChart.FromG;
            _loadedWheelFilter.MaximumG = wheelsChart.ToG;
            _loadedWheelFilter.IncludeLoaded = wheelsChart.IsLoadedChecked;
            _loadedWheelFilter.IncludeUnloaded = wheelsChart.IsUnloadedChecked;

            _camberFilter.AngleUnits = _camberHistogramExtractor.AngleUnits;
            _camberFilter.MinimumCamber = wheelsChart.FromCamber;
            _camberFilter.MaximumCamber = wheelsChart.ToCamber;
            base.BeforeHistogramFilling(wheelsChart);
        }
    }
}