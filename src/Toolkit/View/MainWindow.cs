using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;
using Toolkit.Base.Common;
using Toolkit.Util;
using Toolkit.Base.Log;
using Microsoft.Extensions.DependencyInjection;
using Toolkit.ViewModel;

namespace Toolkit.View
{
    internal class MainWindow : Window
    {
        public MainWindow()
        {
            Title = "MainWindow";
            notifyIconUtil = new NotifyIconUtil();
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
            Logger.Trace("Main Window Initialized");
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (PresentationSource.FromVisual(this) is HwndSource hwndSource)
            {
                Win32Native.SetParent(hwndSource.Handle, Win32Native.HWND_MESSAGE);
                Visibility = Visibility.Collapsed;
                hwndSource.AddHook(WndProc);
            }
            Logger.Trace("Main Window Source Initialized");
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case NotifyIconUtil.NotifyIconMessage:
                    switch (lParam.ToInt32())
                    {
                        case 513: // 左键按下
                            break;
                        case 514: // 左键松开
                            if (mainMenu == null)
                            {
                                InitMainMenu();
                            }
                            mainMenu?.Show();
                            mainMenu?.Activate();
                            Logger.Trace("Main Menu Showed");
                            break;
                        case 516: // 右键按下
                            break;
                        case 517: // 右键松开
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return IntPtr.Zero;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var iconPath = @$"{Environment.CurrentDirectory}\Image\application.ico";
            if (!File.Exists(iconPath))
            {
                Logger.Fatal(new IOException($"未找到图标文件:{iconPath}"), "图标丢失", MethodBase.GetCurrentMethod()?.GetMethodName());
                Close();
                return;
            }
            if (!notifyIconUtil.AddNotifyIcon(new WindowInteropHelper(this).Handle, iconPath))
            {
                Logger.Fatal(new Exception("显示状态栏图标失败"), "显示状态栏图标失败", MethodBase.GetCurrentMethod()?.GetMethodName());
                Exit();
            }
            int code = notifyIconUtil.GetNotifyIconPosition(out notifyIconPosition);
            if (code != 0)
            {
                Logger.Fatal(new Exception($"Error Code {code}"), "获取状态栏图标位置失败", MethodBase.GetCurrentMethod()?.GetMethodName());
                Exit();
            }
            App.Current.Services.GetService<MainMenuViewModel>()?.LoadPluginsAsync();
            Logger.Trace("Main Window Loaded");
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            notifyIconUtil.DeleteNotifyIcon();
        }

        private void InitMainMenu()
        {
            mainMenu = new MainMenuView(notifyIconPosition, Exit)
            {
                Owner = this,
                DataContext = App.Current.Services.GetService<MainMenuViewModel>()
            };
            mainMenu.Deactivated += (sender, e) =>
            {
                mainMenu?.Hide();
            };
        }

        private void Exit()
        {
            Close();
            Environment.Exit(0);
        }

        private readonly NotifyIconUtil notifyIconUtil;
        private Rect notifyIconPosition;
        private MainMenuView? mainMenu = null;
    }
}
