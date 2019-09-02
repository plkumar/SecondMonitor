namespace SecondMonitor.ViewModels.PluginsSettings
{
    using PluginsConfiguration.Common.DataModel;

    public class F12019ConfigurationViewModel : AbstractViewModel<F12019Configuration>
    {
        private int _port;

        public int Port
        {
            get => _port;
            set => SetProperty(ref _port, value);
        }

        protected override void ApplyModel(F12019Configuration model)
        {
            Port = model.Port;
        }

        public override F12019Configuration SaveToNewModel()
        {
            return new F12019Configuration() {Port = Port};
        }
    }
}