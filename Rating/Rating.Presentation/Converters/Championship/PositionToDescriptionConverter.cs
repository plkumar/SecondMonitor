namespace SecondMonitor.Rating.Presentation.Converters.Championship
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class PositionToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int position)
            {
                switch (position)
                {
                    case 1:
                        return "1st Place";
                    case 2:
                        return "2nd Place";
                    case 3:
                        return "3rd Place";
                    default:
                        return string.Empty;
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}