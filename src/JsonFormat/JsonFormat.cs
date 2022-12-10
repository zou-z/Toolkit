using JsonFormat.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Toolkit.Contract;

namespace JsonFormat
{
    public class JsonFormat : IPlugin
    {
        public object? GetIcon()
        {
            return null;
        }

        public string GetName()
        {
            return "JsonFormat";
        }

        public object? GetToolTip()
        {
            return "Json格式化工具";
        }

        public bool IsCloseMenuEarly()
        {
            return false;
        }

        public void MouseLeftDown()
        {
        }

        public void MouseLeftUp()
        {
            if (mainWindow == null)
            {
                mainWindow = new MainWindow
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                };
            }
            mainWindow.Show();
            mainWindow.Activate();
        }

        private MainWindow? mainWindow = null;
    }
}
