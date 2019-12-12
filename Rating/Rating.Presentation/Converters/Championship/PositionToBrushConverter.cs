namespace SecondMonitor.Rating.Presentation.Converters.Championship
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    public class PositionToBrushConverter : IValueConverter
    {
        private static readonly ResourceDictionary ColorResourceDictionary = new ResourceDictionary
        {
            Source = new Uri(@"pack://application:,,,/Rating.Presentation;component/RatingColors.xaml", UriKind.RelativeOrAbsolute)
        };


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int position)
            {
                switch (position)
                {
                    case 1:
                        return ColorResourceDictionary["FirstPlaceBrush"];
                    case 2:
                        return ColorResourceDictionary["SecondPlaceBrush"];
                    case 3:
                        return ColorResourceDictionary["ThirdPlaceBrush"];
                    default:
                        return Brushes.White;
                }
            }

            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}