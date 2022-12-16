using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace JsonFormat.Converter
{
    internal class TextFontFamilyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string fontFamily)
            {
                return new FontFamily(fontFamily);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
