using CommunityToolkit.Mvvm.ComponentModel;
using JsonFormat.Core;
using JsonFormat.Model;
using JsonFormat.Service;
using Microsoft.Extensions.DependencyInjection;
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
                renderConfig ??= new JsonRenderer.RenderConfig();
                if (JsonFormat.GetServices().GetService<AppSetting>() is AppSetting appSetting)
                {
                    renderConfig.FontFamily = new FontFamily(appSetting.Settings.RenderSetting.FontFamily);
                    renderConfig.FontSize = appSetting.Settings.RenderSetting.FontSize;
                    renderConfig.IndentSpaceCount = appSetting.Settings.RenderSetting.IndentSpaceCount;
                    renderConfig.CharColor = (Color)ColorConverter.ConvertFromString(appSetting.Settings.RenderSetting.CharColor);
                    renderConfig.KeyColor = (Color)ColorConverter.ConvertFromString(appSetting.Settings.RenderSetting.KeyColor);
                    renderConfig.StringColor = (Color)ColorConverter.ConvertFromString(appSetting.Settings.RenderSetting.StringColor);
                    renderConfig.NumberColor = (Color)ColorConverter.ConvertFromString(appSetting.Settings.RenderSetting.NumberColor);
                    renderConfig.BooleanNullColor = (Color)ColorConverter.ConvertFromString(appSetting.Settings.RenderSetting.BooleanNullColor);
                }
                jsonRenderer ??= new JsonRenderer();
                JsonDocument = jsonRenderer.Render(jsonObject, renderConfig);
            }
        }

        private FlowDocument? jsonDocument = null;
        private JsonRenderer? jsonRenderer = null;
        private JsonRenderer.RenderConfig? renderConfig = null;
    }
}
