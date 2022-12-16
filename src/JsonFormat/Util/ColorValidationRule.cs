using System.Globalization;
using System.Windows.Controls;
using Toolkit.Base.Util;

namespace JsonFormat.Util
{
    internal class ColorValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string colorString && ColorUtil.IsValidStringColor(colorString))
            {
                return new ValidationResult(true, null);
            }
            return new ValidationResult(false, "无效的颜色值");
        }
    }
}
