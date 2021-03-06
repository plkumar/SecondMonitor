﻿namespace SecondMonitor.ViewModels.CarStatus
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;

    using DataModel.BasicProperties;
    using DataModel.Snapshot;
    using DataModel.Snapshot.Systems;
    using Properties;

    public class WheelStatusViewModel : DependencyObject, IWheelStatusViewModel, INotifyPropertyChanged
    {
        private static readonly DependencyProperty TemperatureUnitsProperty = DependencyProperty.Register("TemperatureUnits", typeof(TemperatureUnits), typeof(WheelStatusViewModel));
        private static readonly DependencyProperty PressureUnitsProperty = DependencyProperty.Register("PressureUnits", typeof(PressureUnits), typeof(WheelStatusViewModel));
        private static readonly DependencyProperty TyreConditionProperty = DependencyProperty.Register("TyreCondition", typeof(double), typeof(WheelStatusViewModel));
        private static readonly DependencyProperty TyreCoreTemperatureProperty = DependencyProperty.Register("TyreCoreTemperature", typeof(OptimalQuantity<Temperature>), typeof(WheelStatusViewModel));
        private static readonly DependencyProperty TyreLeftTemperatureProperty = DependencyProperty.Register("TyreLeftTemperature", typeof(OptimalQuantity<Temperature>), typeof(WheelStatusViewModel));
        private static readonly DependencyProperty TyreCenterTemperatureProperty = DependencyProperty.Register("TyreCenterTemperature", typeof(OptimalQuantity<Temperature>), typeof(WheelStatusViewModel));
        private static readonly DependencyProperty TyreRightTemperatureProperty = DependencyProperty.Register("TyreRightTemperature", typeof(OptimalQuantity<Temperature>), typeof(WheelStatusViewModel));
        private static readonly DependencyProperty BrakeTemperatureProperty = DependencyProperty.Register("BrakeTemperature", typeof(OptimalQuantity<Temperature>), typeof(WheelStatusViewModel));
        private static readonly DependencyProperty TyreSlippingIndicationProperty = DependencyProperty.Register("TyreSlippingIndication", typeof(bool), typeof(WheelStatusViewModel));
        private static readonly DependencyProperty TyrePressureProperty = DependencyProperty.Register("TyrePressure", typeof(OptimalQuantity<Pressure>), typeof(WheelStatusViewModel));
        private static readonly DependencyProperty IsLeftWheelProperty = DependencyProperty.Register("IsLeftWheel", typeof(bool), typeof(WheelStatusViewModel));
        private static readonly DependencyProperty TyreCompoundProperty = DependencyProperty.Register("TyreCompound", typeof(string), typeof(WheelStatusViewModel));
        private static readonly DependencyProperty TyreNoWearWearLimitProperty = DependencyProperty.Register("TyreNoWearWearLimit", typeof(double), typeof(WheelStatusViewModel));
        private static readonly DependencyProperty TyreMildWearLimitProperty = DependencyProperty.Register("TyreMildWearLimit", typeof(double), typeof(WheelStatusViewModel));
        private static readonly DependencyProperty TyreHeavyWearLimitProperty = DependencyProperty.Register("TyreHeavyWearLimit", typeof(double), typeof(WheelStatusViewModel));
        public static readonly DependencyProperty WheelCamberProperty = DependencyProperty.Register("WheelCamber", typeof(double), typeof(WheelStatusViewModel), new PropertyMetadata(default(double)));
        public static readonly DependencyProperty WearAtRaceEndProperty = DependencyProperty.Register("WearAtRaceEnd", typeof(double), typeof(WheelStatusViewModel), new PropertyMetadata(default(double)));
        public static readonly DependencyProperty LapsUntilHeavyWearProperty = DependencyProperty.Register("LapsUntilHeavyWear", typeof(int), typeof(WheelStatusViewModel), new PropertyMetadata(default(int)));

        private readonly TyreLifeTimeMonitor _tyreLifeTimeMonitor;

        public double WheelCamber
        {
            get => (double) GetValue(WheelCamberProperty);
            set => SetValue(WheelCamberProperty, value);
        }

        public int LapsUntilHeavyWear
        {
            get => (int)GetValue(LapsUntilHeavyWearProperty);
            set => SetValue(LapsUntilHeavyWearProperty, value);
        }

        public WheelStatusViewModel(bool isLeft)
        {
            IsLeftWheel = isLeft;
        }

        public WheelStatusViewModel(bool isLeft, SessionRemainingCalculator sessionRemainingCalculator, IPaceProvider paceProvider) : this(isLeft)
        {
            _tyreLifeTimeMonitor = new TyreLifeTimeMonitor(paceProvider, sessionRemainingCalculator);
        }

        public string TyreCompound
        {
            get => (string)GetValue(TyreCompoundProperty);
            set => SetValue(TyreCompoundProperty, value);
        }

        public bool TyreSlippingIndication
        {
            get => (bool)GetValue(TyreSlippingIndicationProperty);
            set => SetValue(TyreSlippingIndicationProperty, value);
        }

        public TemperatureUnits TemperatureUnits
        {
            get => (TemperatureUnits)GetValue(TemperatureUnitsProperty);
            set => SetValue(TemperatureUnitsProperty, value);
        }

        public PressureUnits PressureUnits
        {
            get => (PressureUnits)GetValue(PressureUnitsProperty);
            set => SetValue(PressureUnitsProperty, value);
        }

        public double TyreCondition
        {
            get => (double)GetValue(TyreConditionProperty);
            set => SetValue(TyreConditionProperty, value);
        }

        public double TyreNoWearWearLimit
        {
            get => (double)GetValue(TyreNoWearWearLimitProperty);
            set => SetValue(TyreNoWearWearLimitProperty, value);
        }

        public double TyreMildWearLimit
        {
            get => (double)GetValue(TyreMildWearLimitProperty);
            set => SetValue(TyreMildWearLimitProperty, value);
        }

        public double TyreHeavyWearLimit
        {
            get => (double)GetValue(TyreHeavyWearLimitProperty);
            set => SetValue(TyreHeavyWearLimitProperty, value);
        }

        public OptimalQuantity<Temperature> TyreCoreTemperature
        {
            get => (OptimalQuantity<Temperature>)GetValue(TyreCoreTemperatureProperty);
            set => SetValue(TyreCoreTemperatureProperty, value);
        }

        public OptimalQuantity<Temperature> TyreLeftTemperature
        {
            get => (OptimalQuantity<Temperature>)GetValue(TyreLeftTemperatureProperty);
            set => SetValue(TyreLeftTemperatureProperty, value);
        }

        public OptimalQuantity<Temperature> TyreCenterTemperature
        {
            get => (OptimalQuantity<Temperature>)GetValue(TyreCenterTemperatureProperty);
            set => SetValue(TyreCenterTemperatureProperty, value);
        }

        public OptimalQuantity<Temperature> TyreRightTemperature
        {
            get => (OptimalQuantity<Temperature>)GetValue(TyreRightTemperatureProperty);
            set => SetValue(TyreRightTemperatureProperty, value);
        }

        public OptimalQuantity<Temperature> BrakeTemperature
        {
            get => (OptimalQuantity<Temperature>)GetValue(BrakeTemperatureProperty);
            set => SetValue(BrakeTemperatureProperty, value);
        }

        public OptimalQuantity<Pressure> TyrePressure
        {
            get => (OptimalQuantity<Pressure>)GetValue(TyrePressureProperty);
            set => SetValue(TyrePressureProperty, value);
        }

        public bool IsLeftWheel
        {
            get => (bool)GetValue(IsLeftWheelProperty);
            set => SetValue(IsLeftWheelProperty, value);
        }

        public double WearAtRaceEnd
        {
            get => (double)GetValue(WearAtRaceEndProperty);
            set => SetValue(WearAtRaceEndProperty, value);
        }

        public void ApplyWheelCondition(WheelInfo wheelInfo)
        {

            TyreCondition = wheelInfo.Detached ? 0.1 : 100 * (1 -wheelInfo.TyreWear.ActualWear);
            TyreNoWearWearLimit = 100 * (1 - wheelInfo.TyreWear.NoWearWearLimit);
            TyreMildWearLimit = 100 * (1 - wheelInfo.TyreWear.LightWearLimit);
            TyreHeavyWearLimit = 100 * (1 - wheelInfo.TyreWear.HeavyWearLimit);

            if (wheelInfo.TyreCoreTemperature.ActualQuantity.InCelsius > -200 && (TyreCoreTemperature == null || Math.Abs(TyreCoreTemperature.ActualQuantity.RawValue - wheelInfo.TyreCoreTemperature.ActualQuantity.RawValue) > 0.5))
            {
                TyreCoreTemperature = wheelInfo.TyreCoreTemperature;
            }

            if (TyreLeftTemperature == null || Math.Abs(TyreLeftTemperature.ActualQuantity.RawValue - wheelInfo.LeftTyreTemp.ActualQuantity.RawValue) > 0.5)
            {
                TyreLeftTemperature = wheelInfo.LeftTyreTemp;
            }

            if (wheelInfo.CenterTyreTemp.ActualQuantity.InCelsius > -200 && (TyreCenterTemperature == null || Math.Abs(TyreCenterTemperature.ActualQuantity.RawValue - wheelInfo.CenterTyreTemp.ActualQuantity.RawValue) > 0.5))
            {
                TyreCenterTemperature = wheelInfo.CenterTyreTemp;
            }

            if (TyreRightTemperature == null || Math.Abs(TyreRightTemperature.ActualQuantity.RawValue - wheelInfo.RightTyreTemp.ActualQuantity.RawValue) > 0.5)
            {

                TyreRightTemperature = wheelInfo.RightTyreTemp;
            }

            if (BrakeTemperature == null || Math.Abs(BrakeTemperature.ActualQuantity.RawValue - wheelInfo.BrakeTemperature.ActualQuantity.RawValue) > 0.5)
            {
                BrakeTemperature = wheelInfo.BrakeTemperature;
            }

            TyrePressure = wheelInfo.TyrePressure;
            TyreCompound = wheelInfo.TyreType;
            TyreSlippingIndication = wheelInfo.Detached;
            //WheelCamber = wheelInfo.Camber.GetValueInUnits(AngleUnits.Degrees);
        }

        public void ApplyWheelCondition(SimulatorDataSet dateSet, WheelInfo wheelInfo)
        {
            ApplyWheelCondition(wheelInfo);

            if (_tyreLifeTimeMonitor == null)
            {
                return;
            }

            _tyreLifeTimeMonitor.ApplyWheelInfo(dateSet, wheelInfo);
            WearAtRaceEnd = _tyreLifeTimeMonitor.WearAtRaceEnd;
            LapsUntilHeavyWear = _tyreLifeTimeMonitor.LapsUntilHeavyWear;
        }

        public void Reset()
        {
            _tyreLifeTimeMonitor?.Reset();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}