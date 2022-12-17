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

        public RelayCommand RestoreSettingsCommand => restoreSettingsCommand ??= new RelayCommand(RestoreSettings);

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

        public string KeyColor
        {
            get => keyColor;
            set => SetProperty(ref keyColor, value);
        }

        public string StringColor
        {
            get => stringColor;
            set => SetProperty(ref stringColor, value);
        }

        public string NumberColor
        {
            get => numberColor; 
            set => SetProperty(ref numberColor, value);
        }

        public string BooleanNullColor
        {
            get => booleanNullColor;
            set => SetProperty(ref booleanNullColor, value);
        }

        public AppSetting()
        {
            currentAssemblyName = Assembly.GetExecutingAssembly().GetName().Name is string assemblyName ? assemblyName : "JsonFormat";
            Settings = SettingUtil.LoadSetting<Settings>(currentAssemblyName) is Settings settings ? settings : new Settings();
            fontFamilies = new List<string>();
            fontSizes = new List<int>(RenderSetting.GetMaxFontSize() - RenderSetting.GetMinFontSize() + 1);
            indentSpaceCounts = new List<int>(RenderSetting.GetMaxIndentSpaceCount() - RenderSetting.GetMinIndentSpaceCount() + 1);

            InitDataCollections();    // 初始化只读数据集合
            CheckSettings();          // 检查读取的设置的正确性
            InitSettings();           // 初始化设置相关的属性
        }

        private void InitDataCollections()
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

        private void CheckSettings()
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
                Settings.RenderSetting.CharColor = RenderSetting.GetDefaultCharColor();
            }
            if (!ColorUtil.IsValidStringColor(Settings.RenderSetting.KeyColor))
            {
                Settings.RenderSetting.KeyColor = RenderSetting.GetDefaultKeyColor();
            }
            if (!ColorUtil.IsValidStringColor(Settings.RenderSetting.StringColor))
            {
                Settings.RenderSetting.StringColor = RenderSetting.GetDefaultStringColor();
            }
            if (!ColorUtil.IsValidStringColor(Settings.RenderSetting.NumberColor))
            {
                Settings.RenderSetting.NumberColor = RenderSetting.GetDefaultNumberColor();
            }
            if (!ColorUtil.IsValidStringColor(Settings.RenderSetting.BooleanNullColor))
            {
                Settings.RenderSetting.BooleanNullColor = RenderSetting.GetDefaultBooleanNullColor();
            }
            SettingUtil.SaveSetting(currentAssemblyName, Settings);
        }

        private void InitSettings()
        {
            SelectedFontFamily = Settings.RenderSetting.FontFamily;
            SelectedFontSize = Settings.RenderSetting.FontSize;
            SelectedIndentSpaceCount = Settings.RenderSetting.IndentSpaceCount;
            CharColor = Settings.RenderSetting.CharColor;
            KeyColor = Settings.RenderSetting.KeyColor;
            StringColor = Settings.RenderSetting.StringColor;
            NumberColor = Settings.RenderSetting.NumberColor;
            BooleanNullColor = Settings.RenderSetting.BooleanNullColor;
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

        private void RestoreSettings()
        {
            var fontFamily = RenderSetting.GetDefaultFontFamily();
            SelectedFontFamily = FontFamilies.Contains(fontFamily) ? fontFamily : FontFamilies[0];
            SelectedFontSize = RenderSetting.GetDefaultFontSize();
            SelectedIndentSpaceCount = RenderSetting.GetDefaultIndentSpaceCount();
            CharColor = RenderSetting.GetDefaultCharColor();
            KeyColor = RenderSetting.GetDefaultKeyColor();
            StringColor = RenderSetting.GetDefaultStringColor();
            NumberColor = RenderSetting.GetDefaultNumberColor();
            BooleanNullColor = RenderSetting.GetDefaultBooleanNullColor();
        }

        private void RestoreSavedColor(string? name)
        {
            OnPropertyChanged(name);
        }

        private RelayCommand? openSettingCommand = null;
        private AsyncRelayCommand? applySettingsCommand = null;
        private RelayCommand? restoreSettingsCommand = null;
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
        private string keyColor = string.Empty;
        private string stringColor = string.Empty;
        private string numberColor = string.Empty;
        private string booleanNullColor = string.Empty;
    }
}
