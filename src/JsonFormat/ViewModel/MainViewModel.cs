using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JsonFormat.Model;
using JsonFormat.Service;
using JsonFormat.View;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace JsonFormat.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public AsyncRelayCommand FormatCommand => formatCommand ??= new AsyncRelayCommand(Format);

        public AsyncRelayCommand CopyCommand => copyCommand ??= new AsyncRelayCommand(Copy);

        public RelayCommand ClearContentCommand => clearContentCommand ??= new RelayCommand(ClearContent);

        public RelayCommand CompressedContentCommand => compressedContentCommand ??= new RelayCommand(CompressedContent);

        public RelayCommand AddEscapeCommand => addEscapeCommand ??= new RelayCommand(AddEscape);

        public RelayCommand RemoveEscapeCommand => removeEscapeCommand ??= new RelayCommand(RemoveEscape);

        public RelayCommand OpenAboutCommand => openAboutCommand ??= new RelayCommand(About);

        public TabManager TabManager => tabManager ??= new TabManager();

        public AppSetting? AppSetting
        {
            get
            {
                if (appSetting == null)
                {
                    if (JsonFormat.GetServices().GetService<AppSetting>() is AppSetting setting)
                    {
                        appSetting = setting;
                    }
                }
                return appSetting;
            }
        }

        public string CopyButtonText
        {
            get => copyButtonText;
            set => SetProperty(ref copyButtonText, value);
        }

        public MainViewModel()
        {
        }

        private async Task Format()
        {
            if (TabManager.SelectedItem == null) return;
            if (TabManager.SelectedItem.Content is IJsonFormatView jsonFormatView)
            {
                var jsonFormat = jsonFormatView.GetDataContext();
                if (jsonFormat != null)
                {
                    await jsonFormat.Format(jsonFormatView.GetText());
                }
            }
        }

        private async Task Copy()
        {
            if (TabManager.SelectedItem != null && TabManager.SelectedItem.Content is IJsonFormatView jsonFormatView)
            {
                var text = jsonFormatView.GetText();
                Clipboard.SetText(text);

                text = CopyButtonText;
                CopyButtonText = "\xE081";
                await Task.Delay(1000);
                CopyButtonText = text;
            }
        }

        private void ClearContent()
        {
            if (TabManager.SelectedItem != null && TabManager.SelectedItem.Content is IJsonFormatView jsonFormatView)
            {
                jsonFormatView.ClearContent();
            }
        }

        private void CompressedContent()
        {
            if (TabManager.SelectedItem != null && TabManager.SelectedItem.Content is IJsonFormatView jsonFormatView)
            {
                jsonFormatView.CompressedContent();
            }
        }

        private void AddEscape()
        {
            if (TabManager.SelectedItem != null && TabManager.SelectedItem.Content is IJsonFormatView jsonFormatView)
            {
                jsonFormatView.AddEscape();
            }
        }

        private void RemoveEscape()
        {
            if (TabManager.SelectedItem != null && TabManager.SelectedItem.Content is IJsonFormatView jsonFormatView)
            {
                jsonFormatView.RemoveEscape();
            }
        }

        private void About()
        {
            var aboutWindow = new Window
            {
                Title = "关于",
                ResizeMode = ResizeMode.NoResize,
                SizeToContent = SizeToContent.WidthAndHeight,
                SnapsToDevicePixels = true,
                Content = new TextBox
                {
                    IsReadOnly = true,
                    BorderThickness = new Thickness(0),
                    Background = Brushes.Transparent,
                    Padding = new Thickness(15, 5, 15, 15),
                    Text = "JsonFormat\r\n©zzh\r\nhttps://github.com/zou-z/Toolkit",
                },
            };
            foreach (var window in Application.Current.Windows)
            {
                if (window is MainWindow mainWindow)
                {
                    aboutWindow.Owner = mainWindow;
                    aboutWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    break;
                }
            }
            aboutWindow.ShowDialog();
        }

        private TabManager? tabManager = null;
        private AppSetting? appSetting = null;
        private AsyncRelayCommand? formatCommand = null;
        private AsyncRelayCommand? copyCommand = null;
        private RelayCommand? clearContentCommand = null;
        private RelayCommand? compressedContentCommand = null;
        private RelayCommand? addEscapeCommand = null;
        private RelayCommand? removeEscapeCommand = null;
        private RelayCommand? openAboutCommand = null;
        private string copyButtonText = "\xE16F";
    }
}
