using CommunityToolkit.Mvvm.ComponentModel;
using JsonFormat.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace JsonFormat.ViewModel
{
    internal class JsonFormatViewModel : ObservableObject, IJsonFormat
    {
        public FlowDocument? JsonDocument
        {
            get => jsonDocument;
            set => SetProperty(ref jsonDocument, value);
        }

        public JsonFormatViewModel()
        {
        }

        public async Task Format(string text)
        {
            //if (string.IsNullOrWhiteSpace(text)) return;

            await Task.Delay(3000);
            Debug.WriteLine("Format()");
            Debug.WriteLine(text);

            var paragraph1 = new Paragraph();
            paragraph1.Inlines.Add(new Run
            {
                Text = "this is first paragraph",
                Foreground = Brushes.Red,
            });
            var paragraph2 = new Paragraph();
            paragraph2.Inlines.Add(new Run
            {
                Text = "this is second paragraph this is second paragraph this is second paragraph this is second paragraph this is second paragraph this is second paragraph this is second paragraph",
                Foreground = Brushes.Green,
            });
            var paragraph3 = new Paragraph();
            paragraph3.Inlines.Add(new Run
            {
                Text = "this is third paragraph",
                Foreground = Brushes.Blue,
            });
            var flowDocument = new FlowDocument();
            flowDocument.Blocks.Add(paragraph1);
            flowDocument.Blocks.Add(paragraph2);
            flowDocument.Blocks.Add(paragraph3);
            JsonDocument = flowDocument;
        }

        private FlowDocument? jsonDocument = null;
    }
}
