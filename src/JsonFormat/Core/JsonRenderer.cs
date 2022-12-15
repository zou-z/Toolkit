using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace JsonFormat.Core
{
    internal class JsonRenderer
    {
        public class RenderConfig
        {
            // 字体类型
            public FontFamily FontFamily { get; set; } = new FontFamily("Consolas");

            // 字体大小
            public double FontSize { get; set; } = 15;

            // 缩进的空格数
            public int IndentSpaceCount { get; set; } = 4;

            // 字符颜色
            public Color CharColor { get; set; } = Colors.White;

            // 键名颜色
            public Color KeyColor { get; set; } = Colors.White;

            // 字符串颜色
            public Color StringColor { get; set; } = Colors.White;

            // 数字颜色
            public Color NumberColor { get; set; } = Colors.White;

            // 布尔值和null颜色
            public Color BooleanNullColor { get; set; } = Colors.White;
        }

        public JsonRenderer(RenderConfig renderConfig)
        {
            flowDocumentUtil = new FlowDocumentUtil(renderConfig);
        }

        public FlowDocument Render(object jsonObject)
        {
            flowDocumentUtil.Init();
            RenderJsonValue(jsonObject, true);
            return flowDocumentUtil.GetFlowDocument();
        }

        private void RenderArray(List<object> list, bool isLastOne)
        {
            flowDocumentUtil.AppendSpaceIndent();
            flowDocumentUtil.AppendChar("[", true);
            flowDocumentUtil.IncreaseIndentCount();
            for (int i = 0; i < list.Count; ++i)
            {
                RenderJsonValue(list[i], i + 1 == list.Count);
            }
            flowDocumentUtil.DecreaseIndentCount();
            flowDocumentUtil.AppendSpaceIndent();
            flowDocumentUtil.AppendChar(isLastOne ? "]" : "],", true);
        }

        private void RenderObject(Dictionary<string, object> dict, bool isLastOne)
        {
            flowDocumentUtil.AppendSpaceIndent();
            flowDocumentUtil.AppendChar("{", true);
            flowDocumentUtil.IncreaseIndentCount();
            for (int i = 0; i < dict.Count; ++i)
            {
                KeyValuePair<string, object> keyValuePair = dict.ElementAt(i);
                bool _isLastOne = i + 1 == dict.Count;
                flowDocumentUtil.AppendSpaceIndent();
                flowDocumentUtil.AppendKey($"\"{keyValuePair.Key}\"", false);
                if (keyValuePair.Value is List<object> list)
                {
                    flowDocumentUtil.AppendChar(": ", true);
                    RenderArray(list, _isLastOne);
                }
                else if (keyValuePair.Value is Dictionary<string, object> _dict)
                {
                    flowDocumentUtil.AppendChar(": ", true);
                    RenderObject(_dict, _isLastOne);
                }
                else
                {
                    flowDocumentUtil.AppendChar(": ", false);
                    RenderJsonNormalValue(keyValuePair.Value, _isLastOne);
                }
            }
            flowDocumentUtil.DecreaseIndentCount();
            flowDocumentUtil.AppendSpaceIndent();
            flowDocumentUtil.AppendChar(isLastOne ? "}" : "},", true);
        }

        private void RenderJsonValue(object jsonObject, bool isLastOne)
        {
            if (jsonObject is List<object> list)
            {
                RenderArray(list, isLastOne);
            }
            else if (jsonObject is Dictionary<string, object> dict)
            {
                RenderObject(dict, isLastOne);
            }
            else
            {
                flowDocumentUtil.AppendSpaceIndent();
                RenderJsonNormalValue(jsonObject, isLastOne);
            }
        }

        private void RenderJsonNormalValue(object jsonObject, bool isLastOne)
        {
            if (jsonObject is string str)
            {
                flowDocumentUtil.AppendString(str, isLastOne);
            }
            else if (jsonObject is bool b)
            {
                flowDocumentUtil.AppendBoolean(b, isLastOne);
            }
            else if (jsonObject is double number)
            {
                flowDocumentUtil.AppendNumber(number, isLastOne);
            }
            if (!isLastOne)
            {
                flowDocumentUtil.AppendChar(",", true);
            }
        }

        private class FlowDocumentUtil
        {
            public FlowDocumentUtil(RenderConfig renderConfig)
            {
                this.renderConfig = renderConfig;
            }

            public void Init()
            {
                flowDocument ??= new FlowDocument();
                flowDocument?.Blocks.Clear();
                currentIndentCount = 0;
            }

            public FlowDocument GetFlowDocument()
            {
                return flowDocument ??= new FlowDocument();
            }

            // 增加缩进数
            public void IncreaseIndentCount()
            {
                ++currentIndentCount;
            }

            // 减少缩进数
            public void DecreaseIndentCount()
            {
                --currentIndentCount;
            }

            // 添加空格缩进
            public void AppendSpaceIndent()
            {
                int spaceCount = currentIndentCount * renderConfig.IndentSpaceCount;
                if (spaceCount > 0)
                {
                    string indent = "";
                    while ((--spaceCount) >= 0)
                    {
                        indent += " ";
                    }

                    currentParagraph ??= new Paragraph();
                    currentParagraph.Inlines.Add(new Run
                    {
                        Text = indent,
                        FontFamily = renderConfig.FontFamily,
                        FontSize = renderConfig.FontSize,
                        Foreground = Brushes.White,
                    });
                }
            }

            // 添加字符
            public void AppendChar(string character, bool isLineEnd)
            {
                AppendText(character, renderConfig.CharColor, isLineEnd);
            }

            // 添加键名
            public void AppendKey(string key, bool isLineEnd)
            {
                AppendText(key, renderConfig.KeyColor, isLineEnd);
            }

            // 添加字符串
            public void AppendString(string str, bool isLineEnd)
            {
                AppendText(str, renderConfig.StringColor, isLineEnd);
            }

            // 添加数字
            public void AppendNumber(double number, bool isLineEnd)
            {
                AppendText(number.ToString(), renderConfig.NumberColor, isLineEnd);
            }

            // 添加布尔值
            public void AppendBoolean(bool b, bool isLineEnd)
            {
                AppendText(b.ToString().ToLower(), renderConfig.BooleanNullColor, isLineEnd);
            }

            // 添加null
            public void AppendNull(bool isLineEnd)
            {
                AppendText("null", renderConfig.BooleanNullColor, isLineEnd);
            }

            private void AppendText(string text, Color textColor, bool isLineEnd)
            {
                currentParagraph ??= new Paragraph();
                currentParagraph.Inlines.Add(new Run
                {
                    Text = text,
                    FontFamily = renderConfig.FontFamily,
                    FontSize = renderConfig.FontSize,
                    Foreground = new SolidColorBrush(textColor),
                });
                if (isLineEnd)
                {
                    flowDocument?.Blocks.Add(currentParagraph);
                    currentParagraph = null;
                }
            }

            private FlowDocument? flowDocument = null;
            private Paragraph? currentParagraph = null;
            private int currentIndentCount = 0;
            private readonly RenderConfig renderConfig;
        }

        private readonly FlowDocumentUtil flowDocumentUtil;
    }
}
