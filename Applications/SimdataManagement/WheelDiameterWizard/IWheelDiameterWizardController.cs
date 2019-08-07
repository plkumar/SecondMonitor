namespace SecondMonitor.SimdataManagement.WheelDiameterWizard
{
    using System;
    using DataModel.Snapshot;
    using ViewModel;
    using ViewModels.Controllers;

    public interface IWheelDiameterWizardController : IController
    {
        event EventHandler<EventArgs> WizardCompleted;
        void OpenWizard(CarModelPropertiesViewModel propertiesToDetermine);

        void ProcessDataSet(SimulatorDataSet dataSet);
    }
}