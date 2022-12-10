using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
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

namespace JsonFormatTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Left = Top = 30;
            jsonFormat = new JsonFormat.JsonFormat();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            jsonFormat.MouseLeftUp();
        }

        private void TopMost_Click(object? sender, RoutedEventArgs? e)
        {
            if (Topmost)
            {
                Topmost = false;
                topMostButton.Content = "置顶";
            }
            else
            {
                Topmost = true;
                topMostButton.Content = "取消置顶";
            }
        }

        private readonly JsonFormat.JsonFormat jsonFormat;
    }
}
