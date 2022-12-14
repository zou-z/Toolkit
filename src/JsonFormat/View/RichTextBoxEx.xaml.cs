using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JsonFormat.View
{
    internal partial class RichTextBoxEx : RichTextBox
    {
        public RichTextBoxEx()
        {
            InitializeComponent();
            SnapsToDevicePixels = false;
            PreviewKeyDown += RichTextBox_PreviewKeyDown;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            UpdatePageWidth();
            UpdateLineNumber();
        }

        private void LineNumberView_Loaded(object sender, RoutedEventArgs e)
        {
            lineNumberView = (VirtualizingStackPanel)sender;
            lineNumberView.Loaded -= LineNumberView_Loaded;
            string key = "LineNumberTextStyle";
            if (Resources.Contains(key) && Resources[key] is Style style)
            {
                lineNumberTextStyle = style;
            }
            key = "ParagraphMargin";
            if (Resources.Contains(key) && Resources[key] is Thickness thickness)
            {
                lineHeight = ExtentHeight + thickness.Top;
            }
            UpdateLineNumber();
        }

        private void UpdatePageWidth()
        {
            var text = new TextRange(Document.ContentStart, Document.ContentEnd).Text;
            var typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            var formattedText = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, FontSize, Brushes.Black, 96 / 96);
            Document.PageWidth = formattedText.Width + 12;
        }

        private void UpdateLineNumber()
        {
            if (lineNumberView == null) return;
            int lineCount = Document.Blocks.Count;
            if (lineCount > lineNumberView.Children.Count)
            {
                for (int i = lineNumberView.Children.Count + 1; i <= lineCount; ++i)
                {
                    var textBlock = new TextBlock
                    {
                        Text = i.ToString(),
                        Style = lineNumberTextStyle,
                        FontSize = FontSize,
                        FontFamily = FontFamily,
                        LineHeight = 19.56333,
                    };
                    if (lineHeight > 0)
                    {
                        textBlock.LineHeight = lineHeight;
                    }
                    lineNumberView.Children.Add(textBlock);
                }
            }
            else if (lineCount < lineNumberView.Children.Count)
            {
                for (int i = lineNumberView.Children.Count - 1; i >= lineCount && i > 0; --i)
                {
                    lineNumberView.Children.RemoveAt(i);
                }
            }
        }

        private void Content_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (lineNumberScrollViewer == null)
            {
                if (sender is ScrollViewer scrollViewer && scrollViewer.Parent is DockPanel dockPanel)
                {
                    contentScrollViewer = scrollViewer;
                    lineNumberScrollViewer = (ScrollViewer)dockPanel.Children[0];
                }
            }
            if (contentScrollViewer != null && lineNumberScrollViewer != null)
            {
                lineNumberScrollViewer.ScrollToVerticalOffset(contentScrollViewer.VerticalOffset);
            }
        }

        private void RichTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.V)
            {
                if (Clipboard.GetData("Text") != null)
                {
                    Clipboard.SetText((string)Clipboard.GetData("Text"), TextDataFormat.Text);
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        private VirtualizingStackPanel? lineNumberView = null;
        private Style? lineNumberTextStyle = null;
        private double lineHeight = 0;
        private ScrollViewer? contentScrollViewer = null;
        private ScrollViewer? lineNumberScrollViewer = null;
    }
}
