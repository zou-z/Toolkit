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
            bool result = false;
            var jsonObject = await Task.Run(() =>
            {
                try
                {
                    var jsonObject = JsonParser.Parse(text);
                    result = true;
                    return jsonObject;
                }
                catch (JsonParseException)
                {
                }
                catch (Exception)
                {
                }
                result = false;
                return false;
            });
            if (result)
            {
                jsonRenderer ??= new JsonRenderer(new JsonRenderer.RenderConfig
                {
                });
                JsonDocument = jsonRenderer.Render(jsonObject);
            }
        }

        private FlowDocument? jsonDocument = null;
        private JsonRenderer? jsonRenderer = null;
    }
}
