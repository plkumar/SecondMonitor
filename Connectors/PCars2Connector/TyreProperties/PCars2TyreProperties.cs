namespace SecondMonitor.PCars2Connector.TyreProperties
{
    using System.Collections.Generic;
    using DataModel.BasicProperties;
    using DataModel.Snapshot.Drivers;
    using PluginManager.GameConnector.WheelInformation;

    public class PCars2TyreProperties : AbstractCodeDefinedIdealWheelQuantitiesByClass
    {
        private static readonly TyresProperties FormulaTyres = new TyresProperties(new TyreProperties(85, 10, 165, 10), new TyreProperties(85, 10, 140, 10));
        private static readonly TyresProperties GtTyres = new TyresProperties(new TyreProperties(85, 10, 170, 15), new TyreProperties(85, 10, 170, 15));
        private static readonly TyresProperties TouringCarsTyres = new TyresProperties(new TyreProperties(85, 10, 210, 10), new TyreProperties(85, 10, 210, 10));
        private static readonly TyresProperties FordFusionTyres = new TyresProperties(new TyreProperties(85, 10, 250, 20), new TyreProperties(85, 10, 250, 20));
        private static readonly TyresProperties LightSportsCarsTyres = new TyresProperties(new TyreProperties(85, 10, 155, 10), new TyreProperties(85, 10, 155, 10));
        private static readonly TyresProperties RoadCarsTyres = new TyresProperties(new TyreProperties(85, 10, 210, 10), new TyreProperties(85, 10, 210, 10));
        private static readonly TyresProperties VintageFormula = new TyresProperties(new TyreProperties(85, 10, 170, 30), new TyreProperties(85, 10, 170, 30));

        public PCars2TyreProperties()
        {
            ClassTyrePropertiesMap = new Dictionary<string, TyresProperties>()
            {
                {"CanAm", VintageFormula},
                {"F5", FormulaTyres}, {"Ferrari F355 Series", GtTyres}, {"Ferrari Series", GtTyres},
                {"Formula A", FormulaTyres}, {"Formula C", FormulaTyres}, {"Formula Renault", FormulaTyres}, {"Formula X", FormulaTyres}, {"IndyCar", FormulaTyres},
                {"G40 Junior", GtTyres}, {"Group 4", GtTyres}, {"Group 5", GtTyres}, {"Group A", TouringCarsTyres}, {"Group B", TouringCarsTyres}, {"Group C1", GtTyres},
                {"GT1", GtTyres}, {"GT2", GtTyres}, {"GT3", GtTyres}, {"GT4", GtTyres}, {"GT5", GtTyres}, {"GTE", GtTyres}, {"GTO", GtTyres},
                {"LMP1", GtTyres}, {"LMP1 2016", GtTyres}, {"LMP2", GtTyres}, {"LMP3", GtTyres}, {"LMP900", GtTyres},
                {"Megane Trophy", TouringCarsTyres}, {"Modern Stockcar", FordFusionTyres},
                {"Road A", RoadCarsTyres}, {"Road B", RoadCarsTyres}, {"Road C", RoadCarsTyres}, {"Road D", RoadCarsTyres}, {"Road E", RoadCarsTyres}, {"Road F", RoadCarsTyres}, {"Road G", RoadCarsTyres},
                {"RS01 Trophy", GtTyres}, {"Super Trofeo", GtTyres}, {"Touring Car", TouringCarsTyres}, {"Track Day A", LightSportsCarsTyres}, {"Track Day B", LightSportsCarsTyres},
                {"V8 Supercars", TouringCarsTyres}, {"Vintage F1A", VintageFormula}, {"Vintage F1B", VintageFormula}, {"Vintage F1C", VintageFormula},
                {"Vintage F1D", VintageFormula}, {"Vintage F3 A", VintageFormula}, {"Vintage Indycar", VintageFormula}, {"Vintage Prototype A", GtTyres},
                {"Vintage Prototype B", GtTyres}, {"Vintage Touring-GT A", GtTyres}, {"Vintage Touring-GT B", TouringCarsTyres}, {"Vintage Touring-GT C", TouringCarsTyres},  {"Vintage Touring-GT D", TouringCarsTyres},
            };
        }

        protected override OptimalQuantity<Pressure> GetIdealTyrePressureFront(DriverInfo driver)
        {
            var optimalPressure = base.GetIdealTyrePressureFront(driver);
            return optimalPressure;
        }

        protected override OptimalQuantity<Temperature> GetIdealTyreTemperaturesFront(DriverInfo driver)
        {
            if (driver.CarInfo.WheelsInfo.FrontLeft.TyreType.ToLower().Contains("hard"))
            {
                return new OptimalQuantity<Temperature>()
                {
                    IdealQuantity = Temperature.FromCelsius(87.5),
                    IdealQuantityWindow = Temperature.FromCelsius(17.5),
                };
            }

            if (driver.CarInfo.WheelsInfo.FrontLeft.TyreType.ToLower().Contains("wet"))
            {
                return new OptimalQuantity<Temperature>()
                {
                    IdealQuantity = Temperature.FromCelsius(70),
                    IdealQuantityWindow = Temperature.FromCelsius(25),
                };
            }

            return base.GetIdealTyreTemperaturesFront(driver);
        }

        protected override OptimalQuantity<Pressure> GetIdealTyrePressureRear(DriverInfo driver)
        {
            var optimalPressure = base.GetIdealTyrePressureRear(driver);
            return optimalPressure;
        }

        protected override OptimalQuantity<Temperature> GetIdealTyreTemperaturesRear(DriverInfo driver)
        {
            if (driver.CarInfo.WheelsInfo.FrontLeft.TyreType.ToLower().Contains("hard"))
            {
                return new OptimalQuantity<Temperature>()
                {
                    IdealQuantity = Temperature.FromCelsius(87.5),
                    IdealQuantityWindow = Temperature.FromCelsius(17.5),
                };
            }

            if (driver.CarInfo.WheelsInfo.FrontLeft.TyreType.ToLower().Contains("wet"))
            {
                return new OptimalQuantity<Temperature>()
                {
                    IdealQuantity = Temperature.FromCelsius(70),
                    IdealQuantityWindow = Temperature.FromCelsius(25),
                };
            }

            var optimalTemperature = base.GetIdealTyreTemperaturesRear(driver);
            return optimalTemperature;
        }

        protected override Dictionary<string, TyresProperties> ClassTyrePropertiesMap { get; }
    }
}