using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Toolkit.Base.Log;
using Toolkit.Base.Util;

namespace JsonFormat.Converter
{
    class StringToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string colorString && ColorUtil.IsValidStringColor(colorString))
            {
                try
                {
                    var obj = ColorConverter.ConvertFromString(colorString);
                    if (obj is Color color)
                    {
                        if (parameter is string param)
                        {
                            if (param == "!")
                            {
                                color.R = (byte)(255 - color.R);
                                color.G = (byte)(255 - color.G);
                                color.B = (byte)(255 - color.B);
                            }
                            else if (param == "Binary")
                            {
                                color = ((color.R + color.G + color.B) / 3 > 127) ? Colors.Black : Colors.White;
                            }
                        }
                        return new SolidColorBrush(color);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex, $"字符串转画笔失败，参数：{value}", MethodBase.GetCurrentMethod()?.GetMethodName());
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
