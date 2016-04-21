using System;
using System.Windows;
using System.Windows.Data;

namespace WindowsServicePanel.Xaml.ValueConverters
{
    public class BooleanToRunningStoppedConverter : IValueConverter
    {
        private const String TrueWord = "Running";
        private const String TrueWordLower = "running";
        private const String FalseWord = "Stopped";
        private const String FalseWordLower = "stopped";

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            var boolValue = System.Convert.ToBoolean(value);
            return boolValue ? TrueWord : FalseWord;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            switch (value.ToString().Trim().ToLower())
            {
                case TrueWordLower:
                    return true;
                case FalseWordLower:
                    return false;
                default:
                    return false;
            }
        }
    }
}