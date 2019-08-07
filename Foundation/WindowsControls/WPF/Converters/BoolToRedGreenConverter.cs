namespace SecondMonitor.WindowsControls.WPF.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    public class BoolToRedGreenConverter : IValueConverter
    {
        private static readonly ResourceDictionary ColorResourceDictionary = new ResourceDictionary
        {
            Source = new Uri(@"pack://application:,,,/WindowsControls;component/WPF/Colors.xaml", UriKind.RelativeOrAbsolute)
        };

        private static readonly SolidColorBrush RedBrush;
        private static readonly SolidColorBrush GreenBrush;

        static BoolToRedGreenConverter()
        {
            RedBrush = (SolidColorBrush)ColorResourceDictionary["LightRed02Brush"];
            GreenBrush = (SolidColorBrush)ColorResourceDictionary["Green01Brush"];
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool boolValue))
            {
                return Brushes.Transparent;
            }

            return boolValue ? GreenBrush : RedBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}