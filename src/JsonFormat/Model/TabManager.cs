﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JsonFormat.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace JsonFormat.Model
{
    internal class TabManager : ObservableObject
    {
        public AsyncRelayCommand NewTabCommand => newTabCommand ??= new AsyncRelayCommand(NewTabAsync);

        public ObservableCollection<TabViewItem> Tabs => tabs;

        public TabViewItem? SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }

        public TabManager()
        {
            tabs = new ObservableCollection<TabViewItem>();
            _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, () => { NewTab(); });
        }

        private async Task NewTabAsync()
        {
            await Application.Current.Dispatcher.BeginInvoke(() => { NewTab(); });
        }

        private void NewTab()
        {
            var item = new TabViewItem
            {
                Header = $"{defaultTabName} {++newTabIndex}",
                Content = CreateTabContent(),
            };
            if (SelectedItem != null)
            {
                int index = Tabs.IndexOf(SelectedItem);
                Tabs.Insert(index + 1, item);
            }
            else
            {
                Tabs.Add(item);
            }
            SelectedItem = item;
        }

        private static object CreateTabContent()
        {
            return new JsonFormatView();
        }

        private readonly ObservableCollection<TabViewItem> tabs;
        private TabViewItem? selectedItem = null;
        private AsyncRelayCommand? newTabCommand = null;
        private readonly string defaultTabName = "untitled";
        private int newTabIndex = 0;
    }
}
