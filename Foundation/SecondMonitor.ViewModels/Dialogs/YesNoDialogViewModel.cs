namespace SecondMonitor.ViewModels.Dialogs
{
    public class YesNoDialogViewModel : AbstractDialogViewModel, IDialogViewModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
    }
}