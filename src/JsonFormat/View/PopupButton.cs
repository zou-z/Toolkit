using System;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows;
using System.Runtime.CompilerServices;

namespace JsonFormat.View
{
    internal class PopupButton : FrameworkElement
    {
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(PopupButton), new PropertyMetadata(Brushes.Transparent, new PropertyChangedCallback(BackgroundChanged)));
        public static readonly DependencyProperty SwitchButtonProperty = DependencyProperty.Register("SwitchButton", typeof(ToggleButton), typeof(PopupButton), new PropertyMetadata(null, new PropertyChangedCallback(SwitchButtonChanged)));
        public static readonly DependencyProperty RelatedPopupProperty = DependencyProperty.Register("RelatedPopup", typeof(Popup), typeof(PopupButton), new PropertyMetadata(null, new PropertyChangedCallback(RelatedPopupChanged)));

        public Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        public ToggleButton SwitchButton
        {
            get => (ToggleButton)GetValue(SwitchButtonProperty);
            set => SetValue(SwitchButtonProperty, value);
        }

        public Popup RelatedPopup
        {
            get => (Popup)GetValue(RelatedPopupProperty);
            set => SetValue(RelatedPopupProperty, value);
        }

        public PopupButton()
        {
            DataContextChanged += (sender, e) =>
            {
                if (SwitchButton != null)
                {
                    SwitchButton.DataContext = DataContext;
                }
                if (RelatedPopup != null)
                {
                    RelatedPopup.DataContext = DataContext;
                }
            };
        }

        protected override int VisualChildrenCount => childrenCount;

        protected override Visual GetVisualChild(int index)
        {
            return SwitchButton;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(Background, null, new Rect(0, 0, RenderSize.Width, RenderSize.Height));
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (SwitchButton != null)
            {
                SwitchButton.Measure(availableSize);
                return SwitchButton.DesiredSize;
            }
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            SwitchButton?.Arrange(new Rect(new Point(0, 0), finalSize));
            return finalSize;
        }

        private static void BackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PopupButton self)
            {
                self.InvalidateVisual();
            }
        }

        private static void SwitchButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PopupButton self)
            {
                self.childrenCount = 0;
                if (e.OldValue is ToggleButton oldToggleButton)
                {
                    oldToggleButton.Checked -= self.ToggleButton_Checked;
                    oldToggleButton.DataContext = null;
                    self.AddLogicalChild(oldToggleButton);
                    self.AddVisualChild(oldToggleButton);
                    if (self.RelatedPopup != null)
                    {
                        self.RelatedPopup.PlacementTarget = null;
                    }
                }
                if (e.NewValue is ToggleButton toggleButton)
                {
                    self.AddLogicalChild(toggleButton);
                    self.AddVisualChild(toggleButton);
                    self.childrenCount = 1;
                    toggleButton.Checked += self.ToggleButton_Checked;
                    if (self.RelatedPopup != null)
                    {
                        self.RelatedPopup.PlacementTarget = toggleButton;
                    }
                }
            }
        }

        private static void RelatedPopupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PopupButton self)
            {
                if (e.OldValue is Popup oldPopup)
                {
                    oldPopup.Closed -= self.Popup_Closed;
                    oldPopup.PlacementTarget = null;
                    oldPopup.DataContext = null;
                }
                if (e.NewValue is Popup popup)
                {
                    popup.Closed += self.Popup_Closed;
                    if (self.SwitchButton != null)
                    {
                        popup.PlacementTarget = self.SwitchButton;
                    }
                }
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (RelatedPopup != null)
            {
                RelatedPopup.IsOpen = true;
                SwitchButton.Focusable = true;
            }
        }

        private void Popup_Closed(object? sender, EventArgs e)
        {
            if (SwitchButton != null)
            {
                SwitchButton.IsChecked = false;
                SwitchButton.Focusable = false;
            }
        }

        private int childrenCount = 0;
    }
}
