using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace JsonFormat.View
{
    internal class RichTextBoxLineNumber : FlowDocumentScrollViewer
    {
        public RichTextBoxLineNumber()
        {
            Document = new FlowDocument
            {
                PagePadding = new Thickness(5, 0, 13, 0),
                TextAlignment = TextAlignment.Right,
                Foreground = Brushes.Gray,
            };
            IsSelectionEnabled = false;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitScrollViewer();
        }

        public void UpdatePageWidth()
        {
            var text = new TextRange(Document.ContentStart, Document.ContentEnd).Text;
            var typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            var formattedText = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, FontSize, Brushes.Black, 96 / 96);
            Document.PageWidth = formattedText.Width + Document.PagePadding.Left + Document.PagePadding.Right + 12;
        }

        public void UpdateVerticalOffset(double verticalOffset)
        {
            scrollViewer?.ScrollToVerticalOffset(verticalOffset);
        }

        public void UpdateLineNumber(int targetLineCount)
        {
            if (targetLineCount > Document.Blocks.Count)
            {
                for (int i = Document.Blocks.Count + 1; i <= targetLineCount; ++i)
                {
                    var paragraph = new Paragraph
                    {
                        FontFamily = FontFamily,
                        FontSize = FontSize,
                        Foreground = Brushes.Gray,
                    };
                    paragraph.Inlines.Add(new Run
                    {
                        Text = i.ToString(),
                    });
                    Document.Blocks.Add(paragraph);
                }
            }
            else if (targetLineCount < Document.Blocks.Count)
            {
                var lastBlock = Document.Blocks.LastBlock;
                for (int i = Document.Blocks.Count - 1; i >= targetLineCount && i > 0; --i)
                {
                    var block = lastBlock.PreviousBlock;
                    Document.Blocks.Remove(lastBlock);
                    lastBlock = block;
                }
            }
            UpdatePageWidth();
        }

        private void InitScrollViewer()
        {
            if (Template.FindName("PART_ContentHost", this) is ScrollViewer _scrollViewer)
            {
                scrollViewer = _scrollViewer;
                scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                scrollViewer.CanContentScroll = false;
            }
        }
        
        private ScrollViewer? scrollViewer = null;
    }
}
