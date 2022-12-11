using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JsonFormat.Model;
using JsonFormat.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace JsonFormat.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public ObservableCollection<TabViewItem> Tabs => tabs;

        public TabViewItem? SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }

        public RelayCommand NewTabCommand => newTabCommand ??= new RelayCommand(NewTab);

        public MainViewModel()
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

        private void NewTab()
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
        }

        private static object CreateTabContent()
        {
            return new RichTextBoxEx();
        }

        private readonly ObservableCollection<TabViewItem> tabs;
        private TabViewItem? selectedItem = null;
        private RelayCommand? newTabCommand = null;
        private int newTabIndex = 0;
    }
}
