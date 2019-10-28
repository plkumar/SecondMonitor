using System.Windows.Controls;

namespace SecondMonitor.WindowsControls.WPF.HelpButton
{
    using System.Diagnostics;
    using System.Windows;

    /// <summary>
    /// Interaction logic for HelpButton.xaml
    /// </summary>
    public partial class HelpButton : UserControl
    {
        public static readonly DependencyProperty HelpUrlProperty = DependencyProperty.Register("HelpUrl", typeof(string), typeof(HelpButton), new PropertyMetadata(default(string)));

        public string HelpUrl
        {
            get => (string) GetValue(HelpUrlProperty);
            set => SetValue(HelpUrlProperty, value);
        }

        public HelpButton()
        {
            InitializeComponent();
        }

        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {
            //Uri url = new Uri(HelpUrl);
            Process.Start(new ProcessStartInfo(HelpUrl));
            e.Handled = true;
        }
    }
}
