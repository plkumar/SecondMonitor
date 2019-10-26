namespace SecondMonitor.Rating.Presentation.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;
    using Common.DataModel.Championship.Events;

    public class EventStatusToBrushConverter : IValueConverter
    {
        private static readonly ResourceDictionary ColorResourceDictionary = new ResourceDictionary
        {
            Source = new Uri(@"pack://application:,,,/Rating.Presentation;component/RatingColors.xaml", UriKind.RelativeOrAbsolute)
        };


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EventStatus eventStatus)
            {
                switch (eventStatus)
                {
                    case EventStatus.NotStarted:
                        return ColorResourceDictionary["EventNotStarted"];
                    case EventStatus.InProgress:
                        return ColorResourceDictionary["EventInProgress"];
                    case EventStatus.Finished:
                        return ColorResourceDictionary["EventCompleted"];
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