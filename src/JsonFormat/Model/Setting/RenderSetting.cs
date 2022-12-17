using System.Windows.Media;

namespace JsonFormat.Model.Setting
{
    internal class RenderSetting
    {
        // 字体类型
        public string FontFamily { get; set; } = GetDefaultFontFamily();

        // 字体大小
        public int FontSize { get; set; } = GetDefaultFontSize();

        // 缩进的空格数
        public int IndentSpaceCount { get; set; } = GetDefaultIndentSpaceCount();

        // 字符颜色
        public string CharColor { get; set; } = GetDefaultCharColor();

        // 键名颜色
        public string KeyColor { get; set; } = GetDefaultKeyColor();

        // 字符串颜色
        public string StringColor { get; set; } = GetDefaultStringColor();

        // 数字颜色
        public string NumberColor { get; set; } = GetDefaultNumberColor();

        // 布尔值和null颜色
        public string BooleanNullColor { get; set; } = GetDefaultBooleanNullColor();

        public static string GetDefaultFontFamily()
        {
            return "Consolas";
        }

        public static int GetDefaultFontSize()
        {
            return 15;
        }

        public static int GetMinFontSize()
        {
            return 6;
        }

        public static int GetMaxFontSize()
        {
            return 24;
        }

        public static int GetDefaultIndentSpaceCount()
        {
            return 4;
        }

        public static int GetMinIndentSpaceCount()
        {
            return 0;
        }

        public static int GetMaxIndentSpaceCount()
        {
            return 8;
        }

        public static string GetDefaultCharColor()
        {
            return "#B3BEBE";
        }

        public static string GetDefaultKeyColor()
        {
            return "#C7BA7A";
        }

        public static string GetDefaultStringColor()
        {
            return "#C89D82";
        }

        public static string GetDefaultNumberColor()
        {
            return "#97C79D";
        }

        public static string GetDefaultBooleanNullColor()
        {
            return "#346EAB";
        }
    }
}
