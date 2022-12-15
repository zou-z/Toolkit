using JsonFormat.Model;
using JsonFormat.View;
using Microsoft.Extensions.DependencyInjection;
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
                mainWindow.Closed += (sender, e) =>
                {
                    mainWindow = null;
                };
            }
            mainWindow.Show();
            mainWindow.Activate();
        }

        internal static IServiceProvider GetServices()
        {
            lock (lockObj)
            {
                if (services == null)
                {
                    var serviceCollection = new ServiceCollection();
                    serviceCollection.AddTransient<AppSetting>();
                    services = serviceCollection.BuildServiceProvider();
                }
                return services;
            }
        }

        private MainWindow? mainWindow = null;
        private readonly static object lockObj = new();
        private static IServiceProvider? services = null;
    }
}
