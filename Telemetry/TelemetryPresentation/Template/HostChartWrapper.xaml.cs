using System.Windows.Controls;

namespace SecondMonitor.TelemetryPresentation.Template
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Forms.Integration;
    using System.Windows.Media;
    using WindowsControls.WPF;
    using OxyPlot;
    using OxyPlot.WindowsForms;

    /// <summary>
    /// Interaction logic for HostChartWrapper.xaml
    /// </summary>
    public partial class HostChartWrapper : UserControl
    {
        public static readonly DependencyProperty PlotModelProperty = DependencyProperty.Register(
            "PlotModel", typeof(PlotModel), typeof(HostChartWrapper), new PropertyMetadata(default(PlotModel), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HostChartWrapper hostChartWrapper = (HostChartWrapper) d;
            WindowsFormsHost formHost = hostChartWrapper.FormsHost;
            if (formHost == null)
            {
                return;
            }
            PlotView pv = formHost.Child as OxyPlot.WindowsForms.PlotView;
            pv.Model = e.NewValue as PlotModel;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            WindowsFormsHost formHost = VisualHelper.FindVisualChildren<WindowsFormsHost>(this).FirstOrDefault();
            if (formHost == null)
            {
                return;
            }
            PlotView pv = formHost.Child as OxyPlot.WindowsForms.PlotView;
            pv.Model = PlotModel;
        }

        public PlotModel PlotModel
        {
            get => (PlotModel) GetValue(PlotModelProperty);
            set => SetValue(PlotModelProperty, value);
        }

        public HostChartWrapper()
        {
            InitializeComponent();
        }


    }
}
