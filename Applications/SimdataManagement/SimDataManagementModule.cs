namespace SecondMonitor.SimdataManagement
{
    using Contracts.TrackMap;
    using Ninject.Modules;
    using SimSettings;
    using WheelDiameterWizard;

    public class SimDataManagementModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICarSpecificationProvider>().To<CarSpecificationProvider>().InSingletonScope();
            Bind<IWheelDiameterWizardController>().To<WheelDiameterWizardController>();
            Bind<ITrackDtoManipulator>().To<TrackDtoManipulator>();
        }
    }
}