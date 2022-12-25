using JsonFormat.Model;
using JsonFormat.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JsonFormat.View
{
    internal partial class JsonFormatView : UserControl, IJsonFormatView
    {
        public static readonly DependencyProperty JsonDocumentProperty = DependencyProperty.Register("JsonDocument", typeof(FlowDocument), typeof(JsonFormatView), new PropertyMetadata(null, new PropertyChangedCallback(JsonDocumentChanged)));

        public FlowDocument JsonDocument
        {
            get => (FlowDocument)GetValue(JsonDocumentProperty);
            set => SetValue(JsonDocumentProperty, value);
        }

        public JsonFormatView()
        {
            InitializeComponent();
            if (Content is Grid grid && grid.Children.Count > 0)
            {
                foreach (var item in grid.Children)
                {
                    if (item is RichTextBoxEx richTextBoxEx)
                    {
                        richTextBox = richTextBoxEx;
                        break;
                    }
                }
            }
            ApplySettingsUpdate();
            SetBinding(JsonDocumentProperty, new Binding("JsonDocument") { Mode = BindingMode.OneWay });
        }

        public string GetText()
        {
            if (richTextBox == null) return string.Empty;
            string text = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
            return text.Trim();
        }

        public void ClearContent()
        {
            richTextBox?.Document.Blocks.Clear();
        }

        public void CompressedContent()
        {
            if (richTextBox != null)
            {
                var text = GetText();
                text = Util.StringUtil.StringCompression(text);
                richTextBox.Document.Blocks.Clear();
                var run = new Run
                {
                    Text = text,
                    FontSize = richTextBox.Document.FontSize,
                    FontFamily = richTextBox.Document.FontFamily,
                };
                Paragraph paragraph = new();
                paragraph.Inlines.Add(run);
                richTextBox.Document.Blocks.Add(paragraph);
            }
        }

        public void AddEscape()
        {
            if (richTextBox != null)
            {
                var text = GetText();
                text = Util.StringUtil.StringAddEscape(text);
                var data = text.Split("\r\n");
                richTextBox.Document.Blocks.Clear();
                foreach (var item in data)
                {
                    var run = new Run
                    {
                        Text = item,
                        FontSize = richTextBox.Document.FontSize,
                        FontFamily = richTextBox.Document.FontFamily,
                    };
                    Paragraph paragraph = new();
                    paragraph.Inlines.Add(run);
                    richTextBox.Document.Blocks.Add(paragraph);
                }
            }
        }

        public void RemoveEscape()
        {
            if (richTextBox != null)
            {
                var text = GetText();
                text = Util.StringUtil.StringRemoveEscape(text);
                var data = text.Split("\r\n");
                richTextBox.Document.Blocks.Clear();
                foreach (var item in data)
                {
                    var run = new Run
                    {
                        Text = item,
                        FontSize = richTextBox.Document.FontSize,
                        FontFamily = richTextBox.Document.FontFamily,
                    };
                    Paragraph paragraph = new();
                    paragraph.Inlines.Add(run);
                    richTextBox.Document.Blocks.Add(paragraph);
                }
            }
        }

        public IJsonFormat? GetDataContext()
        {
            if (DataContext is IJsonFormat jsonFormat)
            {
                return jsonFormat;
            }
            return null;
        }

        public void ApplySettingsUpdate()
        {
            if (JsonFormat.GetServices().GetService<AppSetting>() is AppSetting appSetting)
            {
                richTextBox?.ApplySettingsUpdate(appSetting.Settings.RenderSetting.FontFamily, appSetting.Settings.RenderSetting.FontSize);
            }
        }

        private static void JsonDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JsonFormatView self && self.richTextBox != null)
            {
                if (e.NewValue == null)
                {
                    self.richTextBox.Document = null;
                }
                else if (e.NewValue is FlowDocument flowDocument)
                {
                    self.richTextBox.Document = flowDocument;
                }
            }
        }

        private readonly RichTextBoxEx? richTextBox;
    }
}
