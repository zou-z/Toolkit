using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JsonFormat.Model.Setting;
using JsonFormat.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Toolkit.Base.Util;

namespace JsonFormat.Model
{
    internal class AppSetting : ObservableObject
    {
        public RelayCommand OpenSettingCommand => openSettingCommand ??= new RelayCommand(OpenSetting);

        public AsyncRelayCommand ApplySettingsCommand => applySettingsCommand ??= new AsyncRelayCommand(ApplySettings);

        public AsyncRelayCommand RestoreSettingsCommand => restoreSettingsCommand ??= new AsyncRelayCommand(RestoreSettings);

        public RelayCommand<string> RestoreSavedColorCommand => restoreSavedColorCommand ??= new RelayCommand<string>(RestoreSavedColor);

        public Settings Settings { get; private set; }

        public List<string> FontFamilies => fontFamilies;

        public List<int> FontSizes => fontSizes;

        public List<int> IndentSpaceCounts => indentSpaceCounts;

        public string SelectedFontFamily
        {
            get => selectedFontFamily;
            set => SetProperty(ref selectedFontFamily, value);
        }

        public int SelectedFontSize
        {
            get => selectedFontSize;
            set => SetProperty(ref selectedFontSize, value);
        }

        public int SelectedIndentSpaceCount
        {
            get => selectedIndentSpaceCount;
            set => SetProperty(ref selectedIndentSpaceCount, value);
        }

        public string CharColor
        {
            get => charColor;
            set => SetProperty(ref charColor, value);
        }

        public AppSetting()
        {
            if (Assembly.GetExecutingAssembly().GetName().Name is string name)
            {
                currentAssemblyName = name;
            }
            else
            {
                currentAssemblyName = "JsonFormat";
            }

            if (SettingUtil.LoadSetting<Settings>(currentAssemblyName) is Settings _settings)
            {
                Settings = _settings;
            }
            else
            {
                Settings = new Settings();
            }

            fontFamilies = new List<string>();
            fontSizes = new List<int>(RenderSetting.GetMaxFontSize() - RenderSetting.GetMinFontSize() + 1);
            indentSpaceCounts = new List<int>(RenderSetting.GetMaxIndentSpaceCount() - RenderSetting.GetMinIndentSpaceCount() + 1);
            LoadFontDataCollections();
            InitSettings();
        }
        //SettingUtil.SaveSetting(currentAssemblyName, Settings);

        private void LoadFontDataCollections()
        {
            foreach (FontFamily font in Fonts.SystemFontFamilies)
            {
                if (font.Source == "HoloLens MDL2 Assets" ||
                    font.Source == "Marlett" ||
                    font.Source == "OpenSymbol" ||
                    font.Source == "Segoe MDL2 Assets" ||
                    font.Source == "Symbol" ||
                    font.Source == "Webdings" ||
                    font.Source == "Wingdings")
                {
                    continue;
                }
                fontFamilies.Add(font.Source);
            }
            fontFamilies.Sort();

            for (int i = RenderSetting.GetMinFontSize(); i <= RenderSetting.GetMaxFontSize(); ++i)
            {
                fontSizes.Add(i);
            }

            for (int i = RenderSetting.GetMinIndentSpaceCount(); i <= RenderSetting.GetMaxIndentSpaceCount(); ++i)
            {
                indentSpaceCounts.Add(i);
            }
        }

        private void InitSettings()
        {
            if (!FontFamilies.Contains(Settings.RenderSetting.FontFamily))
            {
                Settings.RenderSetting.FontFamily = RenderSetting.GetDefaultFontFamily();
            }
            if (!FontSizes.Contains(Settings.RenderSetting.FontSize))
            {
                Settings.RenderSetting.FontSize = RenderSetting.GetDefaultFontSize();
            }
            if (!IndentSpaceCounts.Contains(Settings.RenderSetting.IndentSpaceCount))
            {
                Settings.RenderSetting.IndentSpaceCount = RenderSetting.GetDefaultIndentSpaceCount();
            }
            if (!ColorUtil.IsValidStringColor(Settings.RenderSetting.CharColor))
            {
                Settings.RenderSetting.CharColor = RenderSetting.GetDefaultTextColor();
            }
            SelectedFontFamily = Settings.RenderSetting.FontFamily;
            SelectedFontSize = Settings.RenderSetting.FontSize;
            SelectedIndentSpaceCount = Settings.RenderSetting.IndentSpaceCount;
            CharColor = Settings.RenderSetting.CharColor;
        }

        private void OpenSetting()
        {
            if (settingWindow == null)
            {
                settingWindow = new SettingWindow { DataContext = this, };
                foreach (var window in Application.Current.Windows)
                {
                    if (window is MainWindow mainWindow)
                    {
                        settingWindow.Owner = mainWindow;
                        settingWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        break;
                    }
                }
            }
            else
            {
                InitSettings();
            }
            settingWindow.Show();
        }

        private async Task ApplySettings()
        {
            await Task.Delay(100);
        }

        private async Task RestoreSettings()
        {
            await Task.Delay(100);
        }

        private void RestoreSavedColor(string? name)
        {
            switch (name)
            {
                case nameof(CharColor): OnPropertyChanged(name); break;
                default: break;
            }
        }

        private RelayCommand? openSettingCommand = null;
        private AsyncRelayCommand? applySettingsCommand = null;
        private AsyncRelayCommand? restoreSettingsCommand = null;
        private RelayCommand<string>? restoreSavedColorCommand = null;
        private SettingWindow? settingWindow = null;
        private readonly string currentAssemblyName;
        private readonly List<string> fontFamilies;
        private readonly List<int> fontSizes;
        private readonly List<int> indentSpaceCounts;
        private string selectedFontFamily = string.Empty;
        private int selectedFontSize = 0;
        private int selectedIndentSpaceCount = 0;
        private string charColor = string.Empty;
    }
}
