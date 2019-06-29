using System.Windows.Forms;

namespace SecondMonitor.WindowsControls.WinForms.PlotViewWrapper
{
    using OxyPlot.WindowsForms;

    public partial class PlotViewWrapper : UserControl
    {
        public PlotViewWrapper()
        {
            InitializeComponent();
        }

        public Panel GetMainPanel()
        {
            return MainPanel;
        }

        public PlotView GetPlotView()
        {
            return plotView;
        }
    }
}
