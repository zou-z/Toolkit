using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Toolkit.Base.Log;
using Toolkit.View;
using Toolkit.ViewModel;

namespace Toolkit
{
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;

        public IServiceProvider Services { get; }

        public App()
        {
            Services = ConfigureServices();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<MainMenuViewModel>();
            return services.BuildServiceProvider();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            CatchUnhandledException();
            var window = new MainWindow();
            window.Show();
            window.Hide();
        }

        private void CatchUnhandledException()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Error(e.Exception, "UI线程未捕获的异常", MethodBase.GetCurrentMethod()?.GetMethodName());
            e.Handled = true;
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            Logger.Error(e.Exception, "Task线程未捕获的异常", MethodBase.GetCurrentMethod()?.GetMethodName());
            e.SetObserved();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = e.ExceptionObject is Exception ex ? ex : new Exception($"非UI线程未捕获的异常，{e.ExceptionObject}");
            if (e.IsTerminating)
            {
                Logger.Fatal(exception, "非UI线程未捕获的异常", MethodBase.GetCurrentMethod()?.GetMethodName());
            }
            else
            {
                Logger.Error(exception, "非UI线程未捕获的异常", MethodBase.GetCurrentMethod()?.GetMethodName());
            }
        }
    }
}
