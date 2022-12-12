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
        }

        public string GetText()
        {
            if (richTextBox == null) return string.Empty;
            return new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
        }

        public IJsonFormat? GetDataContext()
        {
            if (DataContext is IJsonFormat jsonFormat)
            {
                return jsonFormat;
            }
            return null;
        }

        private readonly RichTextBoxEx? richTextBox;
    }
}
