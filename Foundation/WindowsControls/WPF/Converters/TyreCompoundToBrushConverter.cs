namespace SecondMonitor.WindowsControls.WPF.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    public class TyreCompoundToBrushConverter : IValueConverter
    {
        private static readonly ResourceDictionary ColorResourceDictionary = new ResourceDictionary
        {
            Source = new Uri(@"pack://application:,,,/WindowsControls;component/WPF/Colors.xaml", UriKind.RelativeOrAbsolute)
        };

        private static readonly SolidColorBrush SuperSoftBrush;
        private static readonly SolidColorBrush SoftBrush;
        private static readonly SolidColorBrush MediumBrush;
        private static readonly SolidColorBrush HardBrush;
        private static readonly SolidColorBrush IntermediateBrush;
        private static readonly SolidColorBrush WetBrush;

        /*<SolidColorBrush x:Shared="False" x:Key="TyreSuperSoft" Color="#ff00ff"/>
        <SolidColorBrush x:Shared="False" x:Key="TyreSoft" Color="#e60000"/>
        <SolidColorBrush x:Shared="False" x:Key="TyreMedium" Color="#e6e600"/>
        <SolidColorBrush x:Shared="False" x:Key="TyreHard" Color="#ffffff"/>
        <SolidColorBrush x:Shared="False" x:Key="TyreIntermediate" Color="#33cc33"/>
        <SolidColorBrush x:Shared="False" x:Key="TyreWet" Color="#0066ff"/>*/

        static TyreCompoundToBrushConverter()
        {
            SuperSoftBrush = (SolidColorBrush)ColorResourceDictionary["TyreSuperSoft"];
            SoftBrush = (SolidColorBrush)ColorResourceDictionary["TyreSoft"];
            MediumBrush = (SolidColorBrush)ColorResourceDictionary["TyreMedium"];
            HardBrush = (SolidColorBrush)ColorResourceDictionary["TyreHard"];
            IntermediateBrush = (SolidColorBrush)ColorResourceDictionary["TyreIntermediate"];
            WetBrush = (SolidColorBrush)ColorResourceDictionary["TyreWet"];
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string stringValue))
            {
                return Brushes.Transparent;
            }

            stringValue = stringValue.ToLower();
            if (stringValue.Contains("supersoft"))
            {
                return SuperSoftBrush;
            }

            if (stringValue.Contains("soft") || stringValue.Contains("option"))
            {
                return SoftBrush;
            }

            if (stringValue.Contains("medium")  || stringValue.Contains("slick"))
            {
                return MediumBrush;
            }

            if (stringValue.Contains("hard") || stringValue.Contains("prime"))
            {
                return HardBrush;
            }

            if (stringValue.Contains("intermediate"))
            {
                return IntermediateBrush;
            }

            if (stringValue.Contains("wet"))
            {
                return WetBrush;
            }

            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}