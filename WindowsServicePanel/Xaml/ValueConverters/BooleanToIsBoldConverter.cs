using System;
using System.Windows;
using System.Windows.Data;

namespace WindowsServicePanel.Xaml.ValueConverters
{
    public class BooleanToIsBoldConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            var boolValue = System.Convert.ToBoolean(value);
            return boolValue
                ? FontWeight.FromOpenTypeWeight(500)
                : FontWeight.FromOpenTypeWeight(100);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            switch (value.ToString().Trim().ToLower())
            {
                case "Bold":
                    return true;
                default:
                    return false;
            }
        }
    }
}