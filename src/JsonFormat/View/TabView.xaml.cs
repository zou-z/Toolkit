using CommunityToolkit.Mvvm.ComponentModel;
using JsonFormat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JsonFormat.View
{
    internal partial class TabView : FrameworkElement
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<TabViewItem>), typeof(TabView), new PropertyMetadata(null, ItemsSourceChanged));
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(TabViewItem), typeof(TabView), new PropertyMetadata(null, new PropertyChangedCallback(SelectedItemChanged)));
        public static readonly DependencyProperty TabViewFooterProperty = DependencyProperty.Register("TabViewFooter", typeof(object), typeof(TabView), new PropertyMetadata(null));
        public static readonly DependencyProperty NewTabCommandProperty = DependencyProperty.Register("NewTabCommand", typeof(ICommand), typeof(TabView), new PropertyMetadata(null));

        public ObservableCollection<TabViewItem> ItemsSource
        {
            get => (ObservableCollection<TabViewItem>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public TabViewItem SelectedItem
        {
            get => (TabViewItem)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public object TabViewFooter
        {
            get => GetValue(TabViewFooterProperty);
            set => SetValue(TabViewFooterProperty, value);
        }

        public ICommand NewTabCommand
        {
            get => (ICommand)GetValue(NewTabCommandProperty);
            set => SetValue(NewTabCommandProperty, value);
        }

        public TabView()
        {
            InitializeComponent();
            headerListBox = new ListBox();
            contentControl = new ContentControl();
            AddLogicalChild(headerListBox);
            AddVisualChild(headerListBox);
            AddLogicalChild(contentControl);
            AddVisualChild(contentControl);
            SnapsToDevicePixels = true;
        }

        protected override int VisualChildrenCount => 2;

        protected override Visual GetVisualChild(int index)
        {
            return index == 0 ? headerListBox : contentControl;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            headerListBox.Measure(availableSize);
            contentControl.Measure(availableSize);
            return new Size
            {
                Width = contentControl.DesiredSize.Width,
                Height = headerListBox.DesiredSize.Height + contentControl.DesiredSize.Height,
            };
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            headerListBox.Arrange(new Rect(0, 0, finalSize.Width, headerListBox.DesiredSize.Height));
            contentControl.Arrange(new Rect(0, headerListBox.DesiredSize.Height, finalSize.Width, contentControl.DesiredSize.Height));
            return finalSize;
        }

        private static async void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabView self && e.NewValue is ObservableCollection<TabViewItem> source)
            {
                self.headerListBox.ItemsSource = source;
                if (source.Count > 0)
                {
                    await self.headerListBox.Dispatcher.BeginInvoke(() =>
                    {
                        self.SelectedItem = source[0];
                    });
                }
            }
        }

        private static void SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabView self && e.NewValue is TabViewItem tabViewItem)
            {
                self.contentControl.Content = tabViewItem.Content;
            }
        }

        #region Drag
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isPressed = true;
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isPressed = false;
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPressed)
            {
                index = ItemsSource.IndexOf(SelectedItem);
                if (index >= 0)
                {
                    DragDrop.DoDragDrop((FrameworkElement)sender, index, DragDropEffects.Move);
                    isPressed = false;
                }
            }
        }

        private void Border_DragEnter(object sender, DragEventArgs e)
        {
            if (sender is FrameworkElement item && e.Effects == DragDropEffects.Move)
            {
                int targetIndex = ItemsSource.IndexOf((TabViewItem)item.DataContext);
                (ItemsSource[targetIndex], ItemsSource[index]) = (ItemsSource[index], ItemsSource[targetIndex]);
                index = targetIndex;

                headerListBox.SelectedIndex = targetIndex;
                headerListBox.Focus();
            }
        }

        private void Border_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (e.Effects == DragDropEffects.Move)
            {
                e.UseDefaultCursors = false;
                Mouse.SetCursor(Cursors.Arrow);
            }
            else
            {
                e.UseDefaultCursors = true;
            }
            e.Handled = true;
        }
        #endregion

        #region Close Or Add Tab
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement item && item.Tag is TabViewItem tabViewItem)
            {
                int index = ItemsSource.IndexOf(tabViewItem);
                if (index >= 0)
                {
                    ItemsSource.RemoveAt(index);
                    if (ItemsSource.Count == 0)
                    {
                        contentControl.Content = null;
                    }
                    else
                    {
                        SelectedItem ??= ItemsSource[index < ItemsSource.Count ? index : index - 1];
                    }
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NewTabCommand?.Execute(null);
            headerListBox.ScrollIntoView(SelectedItem);
        }
        #endregion

        private readonly ListBox headerListBox;
        private readonly ContentControl contentControl;
        private int index = 0;
        private bool isPressed = false;
    }
}
