namespace SecondMonitor.WindowsControls.WPF.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class IntegerToStringWithSignConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                return intValue.ToString("+#;-#;0");
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int defaultValue = parameter is int i ? i : 0;

            if (value is string stringValue)
            {
                if (int.TryParse(stringValue, out var toReturn))
                {
                    return toReturn;
                }
            }

            return defaultValue;
        }
    }
}