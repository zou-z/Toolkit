using CommunityToolkit.Mvvm.ComponentModel;
using JsonFormat.Core;
using JsonFormat.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
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
            if (string.IsNullOrWhiteSpace(text)) return;
            //await Task.Run(() =>
            //{
            try
            {
                var jsonObject = JsonParser.Parse(text);
                jsonRenderer ??= new JsonRenderer(new JsonRenderer.RenderConfig
                {
                });
                JsonDocument = jsonRenderer.Render(jsonObject);
            }
            catch (JsonParseException)
            {
            }
            catch (Exception)
            {
            }
            //});
        }

        private FlowDocument? jsonDocument = null;
        private JsonRenderer? jsonRenderer = null;
    }
}
