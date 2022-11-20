using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Toolkit.Base.Util
{
    public class TextBlockUtil : DependencyObject
    {
        /// <summary>
        /// 自动文本裁剪（TextBlock的可显示宽度要与父控件相同，TextBlock不要设置Margin，父控件不要设置Padding）
        /// </summary>
        public static readonly DependencyProperty IsAutoTextTrimmingProperty = DependencyProperty.RegisterAttached("IsAutoTextTrimming", typeof(bool), typeof(TextBlockUtil), new PropertyMetadata(false, new PropertyChangedCallback(IsAutoTextTrimmingChanged)));

        public TextBlockUtil()
        {
        }

        public static void SetIsAutoTextTrimming(DependencyObject d, bool value)
        {
            d.SetValue(IsAutoTextTrimmingProperty, value);
        }

        public static bool GetIsAutoTextTrimming(DependencyObject d)
        {
            return (bool)d.GetValue(IsAutoTextTrimmingProperty);
        }

        private static void IsAutoTextTrimmingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlock textBlock && e.NewValue is bool isAutoTextTrimming)
            {
                textBlock.SizeChanged -= TextBlock_SizeChanged;
                if (isAutoTextTrimming)
                {
                    textBlock.SizeChanged += TextBlock_SizeChanged;
                }
                else
                {
                    textBlock.TextTrimming = TextTrimming.None;
                    textBlock.ToolTip = null;
                }
            }
        }

        private static async void TextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                FrameworkElement element = (FrameworkElement)textBlock.Parent;
                if (textBlock.ActualWidth > element.ActualWidth)
                {
                    if (textBlock.TextTrimming == TextTrimming.CharacterEllipsis) return;
                    textBlock.SizeChanged -= TextBlock_SizeChanged;
                    textBlock.TextTrimming = TextTrimming.CharacterEllipsis;
                    textBlock.ToolTip = textBlock.Text;
                }
                else
                {
                    if (textBlock.TextTrimming == TextTrimming.None) return;
                    textBlock.SizeChanged -= TextBlock_SizeChanged;
                    textBlock.TextTrimming = TextTrimming.None;
                    textBlock.ToolTip = null;
                }
                await Task.Delay(100);
                textBlock.SizeChanged += TextBlock_SizeChanged;
            }
        }
    }
}
