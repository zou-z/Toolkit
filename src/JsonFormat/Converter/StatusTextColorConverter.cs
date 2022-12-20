using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace JsonFormat.Converter
{
    internal class StatusTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string text && text == "\xE081" ? Brushes.LightGreen : Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
