using JsonFormat.Service;
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
            SetBinding(JsonDocumentProperty, new Binding("JsonDocument") { Mode = BindingMode.OneWay });
        }

        public string GetText()
        {
            if (richTextBox == null) return string.Empty;
            string text = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
            return text.Trim();
        }

        public IJsonFormat? GetDataContext()
        {
            if (DataContext is IJsonFormat jsonFormat)
            {
                return jsonFormat;
            }
            return null;
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
