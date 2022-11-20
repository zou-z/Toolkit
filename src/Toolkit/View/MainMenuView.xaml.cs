using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Toolkit.View
{
    internal partial class MainMenuView : Window
    {
        public MainMenuView(Rect notifyIconPosition, Action exit)
        {
            InitializeComponent();
            // don't show in switcher
            Size size = GetContentSize();
            Left = notifyIconPosition.Left - BorderThickness.Left;
            Top = notifyIconPosition.Top - size.Height - BorderThickness.Top - BorderThickness.Bottom;
            this.exit = exit;
        }

        private Size GetContentSize()
        {
            UIElement uiElement = (UIElement)Content;
            uiElement.Measure(new Size
            {
                Width = SystemParameters.PrimaryScreenWidth,
                Height = SystemParameters.PrimaryScreenHeight,
            });
            return uiElement.DesiredSize;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            exit?.Invoke();
        }

        private readonly Action exit;
    }
}
