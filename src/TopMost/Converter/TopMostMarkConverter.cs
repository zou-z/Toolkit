using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TopMost.Converter
{
    internal class TopMostMarkConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isTopMost)
            {
                return isTopMost ? Brushes.LightGreen : Brushes.Transparent;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
