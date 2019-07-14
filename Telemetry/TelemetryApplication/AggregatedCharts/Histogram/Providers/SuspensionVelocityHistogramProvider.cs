namespace SecondMonitor.Telemetry.TelemetryApplication.AggregatedCharts.Histogram.Providers
{
    using System;
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

    public class SuspensionVelocityHistogramProvider : AbstractWheelHistogramProvider<SuspensionVelocityWheelsChartViewModel, SuspensionVelocityHistogramChartViewModel>
    {
        private readonly SuspensionVelocityHistogramDataExtractor _suspensionVelocityHistogramDataExtractor;
        private readonly SuspensionVelocityFilter _suspensionVelocityFilter;
        private readonly ISettingsController _settingsController;
        private CarPropertiesDto _carProperties;

        public SuspensionVelocityHistogramProvider(SuspensionVelocityHistogramDataExtractor suspensionVelocityHistogramDataExtractor, ILoadedLapsCache loadedLapsCache, IViewModelFactory viewModelFactory, SuspensionVelocityFilter suspensionVelocityFilter, ISettingsController settingsController)
            : base(suspensionVelocityHistogramDataExtractor, loadedLapsCache, viewModelFactory, new []{suspensionVelocityFilter} )
        {
            _suspensionVelocityHistogramDataExtractor = suspensionVelocityHistogramDataExtractor;
            _suspensionVelocityFilter = suspensionVelocityFilter;
            _settingsController = settingsController;
        }

        public override string ChartName => "Suspension Velocity Histogram";

        public override AggregatedChartKind Kind => AggregatedChartKind.Histogram;

        protected override bool ResetCommandVisible => true;

        protected override void OnNewViewModel(SuspensionVelocityWheelsChartViewModel newViewModel)
        {
            _carProperties = _settingsController.GetCarPropertiesForCurrentCar();
            newViewModel.BandSize = _carProperties.ChartsProperties.SuspensionVelocityHistogram.BandSize.GetValueInUnits(_suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);
            newViewModel.Maximum = _carProperties.ChartsProperties.SuspensionVelocityHistogram.Maximum.GetValueInUnits(_suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);
            newViewModel.Minimum = _carProperties.ChartsProperties.SuspensionVelocityHistogram.Minimum.GetValueInUnits(_suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);

            ((SuspensionVelocityHistogramChartViewModel)newViewModel.FrontLeftChartViewModel).BumpTransition = _carProperties.FrontLeftTyre.BumpTransition.GetValueInUnits(_suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);
            ((SuspensionVelocityHistogramChartViewModel)newViewModel.FrontLeftChartViewModel).ReboundTransition = _carProperties.FrontLeftTyre.ReboundTransition.GetValueInUnits(_suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);
            ((SuspensionVelocityHistogramChartViewModel)newViewModel.FrontLeftChartViewModel).Unit = Velocity.GetUnitSymbol(_suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);

            ((SuspensionVelocityHistogramChartViewModel)newViewModel.FrontRightChartViewModel).BumpTransition = _carProperties.FrontRightTyre.BumpTransition.GetValueInUnits(_suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);
            ((SuspensionVelocityHistogramChartViewModel)newViewModel.FrontRightChartViewModel).ReboundTransition = _carProperties.FrontRightTyre.ReboundTransition.GetValueInUnits(_suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);
            ((SuspensionVelocityHistogramChartViewModel)newViewModel.FrontRightChartViewModel).Unit = Velocity.GetUnitSymbol(_suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);

            ((SuspensionVelocityHistogramChartViewModel)newViewModel.RearLeftChartViewModel).BumpTransition = _carProperties.RearLeftTyre.BumpTransition.GetValueInUnits(_suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);
            ((SuspensionVelocityHistogramChartViewModel)newViewModel.RearLeftChartViewModel).ReboundTransition = _carProperties.RearLeftTyre.ReboundTransition.GetValueInUnits(_suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);
            ((SuspensionVelocityHistogramChartViewModel)newViewModel.RearLeftChartViewModel).Unit = Velocity.GetUnitSymbol(_suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);

            ((SuspensionVelocityHistogramChartViewModel)newViewModel.RearRightChartViewModel).BumpTransition = _carProperties.RearRightTyre.BumpTransition.GetValueInUnits(_suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);
            ((SuspensionVelocityHistogramChartViewModel)newViewModel.RearRightChartViewModel).ReboundTransition = _carProperties.RearRightTyre.ReboundTransition.GetValueInUnits(_suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);
            ((SuspensionVelocityHistogramChartViewModel)newViewModel.RearRightChartViewModel).Unit = Velocity.GetUnitSymbol(_suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);

            newViewModel.BandSize = _carProperties.ChartsProperties.SuspensionVelocityHistogram.BandSize.GetValueInUnits(_suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);
        }

        protected override void ResetHistogramParameters(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, SuspensionVelocityWheelsChartViewModel wheelsChart)
        {
            _carProperties = _settingsController.ResetAndGetCarPropertiesForCurrentCar();
            OnNewViewModel(wheelsChart);
            base.ResetHistogramParameters(loadedLaps, bandSize, wheelsChart);
        }

        protected override void ApplyHistogramLimits(Histogram flHistogram, Histogram frHistogram, Histogram rlHistogram, Histogram rrHistogram, SuspensionVelocityWheelsChartViewModel viewModel)
        {
            double maxY = Math.Max(flHistogram.MaximumY, Math.Max(frHistogram.MaximumY, Math.Max(rlHistogram.MaximumY, rrHistogram.MaximumY)));
            flHistogram.MaximumY = maxY;
            frHistogram.MaximumY = maxY;
            rlHistogram.MaximumY = maxY;
            rrHistogram.MaximumY = maxY;

            flHistogram.MinimumX = viewModel.Minimum;
            frHistogram.MinimumX = viewModel.Minimum;
            rlHistogram.MinimumX = viewModel.Minimum;
            rrHistogram.MinimumX = viewModel.Minimum;

            flHistogram.MaximumX = viewModel.Maximum;
            frHistogram.MaximumX = viewModel.Maximum;
            rlHistogram.MaximumX = viewModel.Maximum;
            rrHistogram.MaximumX = viewModel.Maximum;
        }

        protected override void RefreshHistogram(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, SuspensionVelocityWheelsChartViewModel wheelsChart)
        {
            _carProperties.ChartsProperties.SuspensionVelocityHistogram.BandSize = Velocity.FromUnits(wheelsChart.BandSize, _suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);
            _carProperties.ChartsProperties.SuspensionVelocityHistogram.Minimum = Velocity.FromUnits(wheelsChart.Minimum, _suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);
            _carProperties.ChartsProperties.SuspensionVelocityHistogram.Maximum = Velocity.FromUnits(wheelsChart.Maximum, _suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);

            _carProperties.FrontLeftTyre.BumpTransition = Velocity.FromUnits(((SuspensionVelocityHistogramChartViewModel) wheelsChart.FrontLeftChartViewModel).BumpTransition, _suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);
            _carProperties.FrontLeftTyre.ReboundTransition = Velocity.FromUnits(((SuspensionVelocityHistogramChartViewModel) wheelsChart.FrontLeftChartViewModel).ReboundTransition, _suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);

            _carProperties.FrontRightTyre.BumpTransition = Velocity.FromUnits(((SuspensionVelocityHistogramChartViewModel)wheelsChart.FrontRightChartViewModel).BumpTransition, _suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);
            _carProperties.FrontRightTyre.ReboundTransition = Velocity.FromUnits(((SuspensionVelocityHistogramChartViewModel)wheelsChart.FrontRightChartViewModel).ReboundTransition, _suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);

            _carProperties.RearLeftTyre.BumpTransition = Velocity.FromUnits(((SuspensionVelocityHistogramChartViewModel)wheelsChart.RearLeftChartViewModel).BumpTransition, _suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);
            _carProperties.RearLeftTyre.ReboundTransition = Velocity.FromUnits(((SuspensionVelocityHistogramChartViewModel)wheelsChart.RearLeftChartViewModel).ReboundTransition, _suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);

            _carProperties.RearRightTyre.BumpTransition = Velocity.FromUnits(((SuspensionVelocityHistogramChartViewModel)wheelsChart.RearRightChartViewModel).BumpTransition, _suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);
            _carProperties.RearRightTyre.ReboundTransition = Velocity.FromUnits(((SuspensionVelocityHistogramChartViewModel)wheelsChart.RearRightChartViewModel).ReboundTransition, _suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall);
            base.RefreshHistogram(loadedLaps, bandSize, wheelsChart);
        }

        protected override Histogram ExtractFlHistogram(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, SuspensionVelocityWheelsChartViewModel wheelsChart)
        {
            Filters.ForEach(x => x.FilterFrontLeft());
            _suspensionVelocityHistogramDataExtractor.BumpTransition = ((SuspensionVelocityHistogramChartViewModel)wheelsChart.FrontLeftChartViewModel).BumpTransition;
            _suspensionVelocityHistogramDataExtractor.ReboundTransition = ((SuspensionVelocityHistogramChartViewModel)wheelsChart.FrontLeftChartViewModel).ReboundTransition;
            return _suspensionVelocityHistogramDataExtractor.ExtractHistogramFrontLeft(loadedLaps, bandSize, Filters);
        }

        protected override Histogram ExtractFrHistogram(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, SuspensionVelocityWheelsChartViewModel wheelsChart)
        {
            Filters.ForEach(x => x.FilterFrontRight());
            _suspensionVelocityHistogramDataExtractor.BumpTransition = ((SuspensionVelocityHistogramChartViewModel)wheelsChart.FrontRightChartViewModel).BumpTransition;
            _suspensionVelocityHistogramDataExtractor.ReboundTransition = ((SuspensionVelocityHistogramChartViewModel)wheelsChart.FrontRightChartViewModel).ReboundTransition;
            return _suspensionVelocityHistogramDataExtractor.ExtractHistogramFrontRight(loadedLaps, bandSize, Filters);
        }

        protected override Histogram ExtractRlHistogram(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, SuspensionVelocityWheelsChartViewModel wheelsChart)
        {
            Filters.ForEach(x => x.FilterRearLeft());
            _suspensionVelocityHistogramDataExtractor.BumpTransition = ((SuspensionVelocityHistogramChartViewModel)wheelsChart.RearLeftChartViewModel).BumpTransition;
            _suspensionVelocityHistogramDataExtractor.ReboundTransition = ((SuspensionVelocityHistogramChartViewModel)wheelsChart.RearLeftChartViewModel).ReboundTransition;
            return _suspensionVelocityHistogramDataExtractor.ExtractHistogramRearLeft(loadedLaps, bandSize, Filters);
        }

        protected override Histogram ExtractRrHistogram(IReadOnlyCollection<LapTelemetryDto> loadedLaps, double bandSize, SuspensionVelocityWheelsChartViewModel wheelsChart)
        {
            Filters.ForEach(x => x.FilterRearRight());
            _suspensionVelocityHistogramDataExtractor.BumpTransition = ((SuspensionVelocityHistogramChartViewModel)wheelsChart.RearRightChartViewModel).BumpTransition;
            _suspensionVelocityHistogramDataExtractor.ReboundTransition = ((SuspensionVelocityHistogramChartViewModel)wheelsChart.RearRightChartViewModel).ReboundTransition;
            return _suspensionVelocityHistogramDataExtractor.ExtractHistogramRearRight(loadedLaps, bandSize, Filters);
        }

        protected override void BeforeHistogramFilling(SuspensionVelocityWheelsChartViewModel wheelsChart)
        {
            _suspensionVelocityFilter.MinimumVelocity =  wheelsChart.Minimum;
            _suspensionVelocityFilter.MaximumVelocity = wheelsChart.Maximum;
            _suspensionVelocityFilter.VelocityUnits = _suspensionVelocityHistogramDataExtractor.VelocityUnitsSmall;
            base.BeforeHistogramFilling(wheelsChart);
        }
    }
}