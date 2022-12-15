using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JsonFormat.Model;
using JsonFormat.Service;
using JsonFormat.View;
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

        public TabManager TabManager => tabManager ??= new TabManager();

        public AppSetting AppSetting => appSetting ??= new AppSetting();

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

        private TabManager? tabManager = null;
        private AppSetting? appSetting = null;
        private AsyncRelayCommand? formatCommand = null;
    }
}
