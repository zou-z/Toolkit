using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JsonFormat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
                    //Content = new System.Windows.Controls.TextBlock { Text = $"content {i + 1}", Foreground = System.Windows.Media.Brushes.White },
                    Content = CreateTabContent(),
                });
            }
        }

        private void NewTab()
        {
            var item = new TabViewItem
            {
                Header = $"new header {++newTabIndex}",
                //Content = new System.Windows.Controls.TextBlock { Text = $"new content {newTabIndex}", Foreground = System.Windows.Media.Brushes.White },
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

        private object CreateTabContent()
        {
            return new RichTextBox
            {
            };
        }

        private readonly ObservableCollection<TabViewItem> tabs;
        private TabViewItem? selectedItem = null;
        private RelayCommand? newTabCommand = null;
        private int newTabIndex = 0;
    }
}
