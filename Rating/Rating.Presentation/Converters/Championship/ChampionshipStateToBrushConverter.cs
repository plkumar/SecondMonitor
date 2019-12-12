namespace SecondMonitor.Rating.Presentation.Converters.Championship
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;
    using Common.DataModel.Championship;

    public class ChampionshipStateToBrushConverter : IValueConverter
    {
        private static readonly  ResourceDictionary ColorResourceDictionary = new ResourceDictionary
        {
            Source = new Uri(@"pack://application:,,,/Rating.Presentation;component/RatingColors.xaml",UriKind.RelativeOrAbsolute)
        };


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ChampionshipState championshipState)
            {
                switch (championshipState)
                {
                    case ChampionshipState.NotStarted:
                        return ColorResourceDictionary["PotentialChampionshipRace"];
                    case ChampionshipState.Started:
                        return ColorResourceDictionary["ChampionshipRace"];
                    case ChampionshipState.Finished:
                        return ColorResourceDictionary["NoChampionship"];
                    case ChampionshipState.LastSessionCanceled:
                        return ColorResourceDictionary["LastSessionCanceled"];
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