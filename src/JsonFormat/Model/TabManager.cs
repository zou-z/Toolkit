using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JsonFormat.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JsonFormat.Model
{
    internal class TabManager : ObservableObject
    {
        public AsyncRelayCommand NewTabCommand => newTabCommand ??= new AsyncRelayCommand(NewTab);

        public ObservableCollection<TabViewItem> Tabs => tabs;

        public TabViewItem? SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }

        public TabManager()
        {
            tabs = new ObservableCollection<TabViewItem>();
            for (int i = 0; i < 3; ++i)
            {
                tabs.Add(new TabViewItem
                {
                    Header = $"header {i + 1}",
                    Content = CreateTabContent(),
                });
            }
        }

        private async Task NewTab()
        {
            await Application.Current.Dispatcher.BeginInvoke(() =>
            {
                var item = new TabViewItem
                {
                    Header = $"new header {++newTabIndex}",
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
            });
        }

        private static object CreateTabContent()
        {
            return new JsonFormatView();
        }

        private readonly ObservableCollection<TabViewItem> tabs;
        private TabViewItem? selectedItem = null;
        private AsyncRelayCommand? newTabCommand = null;
        private int newTabIndex = 0;
    }
}
