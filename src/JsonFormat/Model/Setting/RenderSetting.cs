using System.Windows.Media;

namespace JsonFormat.Model.Setting
{
    internal class RenderSetting
    {
        // 字体类型
        public string FontFamily { get; set; } = "Consolas";

        // 字体大小
        public int FontSize { get; set; } = 15;

        // 缩进的空格数
        public int IndentSpaceCount { get; set; } = 4;

        // 字符颜色
        public string CharColor { get; set; } = "#FFFFFF";

        // 键名颜色
        public string KeyColor { get; set; } = "#FFFFFF";

        // 字符串颜色
        public string StringColor { get; set; } = "#FFFFFF";

        // 数字颜色
        public string NumberColor { get; set; } = "#FFFFFF";

        // 布尔值和null颜色
        public string BooleanNullColor { get; set; } = "#FFFFFF";
    }
}
