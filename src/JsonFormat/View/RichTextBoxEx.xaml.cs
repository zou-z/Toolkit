﻿using CommunityToolkit.Mvvm.Messaging;
using JsonFormat.Model;
using Microsoft.Extensions.DependencyInjection;
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
            ContextMenu = null;
            SnapsToDevicePixels = false;
            PreviewKeyDown += RichTextBox_PreviewKeyDown;
        }

        public void ApplySettingsUpdate(string fontFamily, int fontSize)
        {
            Document.FontFamily = new FontFamily(fontFamily);
            Document.FontSize = fontSize;
            UpdatePageWidth();
            if (lineNumber != null)
            {
                lineNumber.Document.FontFamily = Document.FontFamily;
                lineNumber.Document.FontSize = Document.FontSize;
                lineNumber.UpdatePageWidth();
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            UpdatePageWidth();
            lineNumber?.UpdateLineNumber(Document.Blocks.Count);
        }

        private void RichTextBoxLineNumber_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is RichTextBoxLineNumber _lineNumber)
            {
                lineNumber = _lineNumber;
                lineNumber.Loaded -= RichTextBoxLineNumber_Loaded;
                lineNumber.Document.FontFamily = Document.FontFamily;
                lineNumber.Document.FontSize = Document.FontSize;
                lineNumber.UpdateLineNumber(Document.Blocks.Count);
            }
        }

        private void UpdatePageWidth()
        {
            var text = new TextRange(Document.ContentStart, Document.ContentEnd).Text;
            var typeface = new Typeface(Document.FontFamily, Document.FontStyle, Document.FontWeight, Document.FontStretch);
            var formattedText = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, Document.FontSize, Brushes.Black, 96 / 96);
            Document.PageWidth = formattedText.Width + 12;
        }

        private void Content_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (contentScrollViewer == null)
            {
                if (sender is ScrollViewer scrollViewer)
                {
                    contentScrollViewer = scrollViewer;
                }
            }
            if (contentScrollViewer != null && lineNumber != null)
            {
                lineNumber.UpdateVerticalOffset(contentScrollViewer.VerticalOffset);
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

        private RichTextBoxLineNumber? lineNumber = null;
        private ScrollViewer? contentScrollViewer = null;
    }
}
