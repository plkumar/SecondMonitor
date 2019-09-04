namespace SecondMonitor.Rating.Presentation.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;
    using Application.Championship.ViewModels.IconState;

    public class ChampionshipIconStateToBrushConverter : IValueConverter
    {
        private static readonly  ResourceDictionary ColorResourceDictionary = new ResourceDictionary
        {
            Source = new Uri(@"pack://application:,,,/Rating.Presentation;component/RatingColors.xaml",UriKind.RelativeOrAbsolute)
        };


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ChampionshipIconState championshipIconState)
            {
                switch (championshipIconState)
                {
                    case ChampionshipIconState.None:
                        return ColorResourceDictionary["NoChampionship"];
                    case ChampionshipIconState.ChampionshipInProgress:
                        return ColorResourceDictionary["ChampionshipRace"];
                    case ChampionshipIconState.PotentialChampionship:
                        return ColorResourceDictionary["PotentialChampionshipRace"];
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