namespace SecondMonitor.ViewModels.WheelDiameterWizard
{
    public class WelcomeStageViewModel : AbstractViewModel
    {
        public WelcomeStageViewModel()
        {
            WelcomeText = "Welcome to the tyre diameter wizard.\nThis wizard will help you determine the diameter of the car wheels.\nFollow the instructions:";
            InstructionText = "Start Moving";
        }
        public string WelcomeText { get; }

        public string InstructionText { get; }
    }
}