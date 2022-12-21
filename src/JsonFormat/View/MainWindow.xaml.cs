using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JsonFormat.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    internal partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Close_Popup(object sender, RoutedEventArgs e)
        {
            if (popup == null)
            {
                FrameworkElement element = (FrameworkElement)sender;
                while (element.Parent != null)
                {
                    if (element.Parent is Popup _popup)
                    {
                        popup = _popup;
                        break;
                    }
                    element = (FrameworkElement)element.Parent;
                }
            }
            if (popup != null)
            {
                popup.IsOpen = false;
            }
        }

        private Popup? popup = null;
    }
}
