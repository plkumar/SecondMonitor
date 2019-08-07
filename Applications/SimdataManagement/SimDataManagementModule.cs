namespace SecondMonitor.SimdataManagement
{
    using Ninject.Modules;
    using SimSettings;
    using WheelDiameterWizard;

    public class SimDataManagementModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICarSpecificationProvider>().To<CarSpecificationProvider>().InSingletonScope();
            Bind<IWheelDiameterWizardController>().To<WheelDiameterWizardController>();
        }
    }
}