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
                            mainMenu = new MainMenuView(notifyIconPosition, () =>
                            {
                                Close();
                            })
                            {
                                Owner = this,
                            };
                            mainMenu.Closed += (sender, e) =>
                            {
                                mainMenu = null;
                            };
                            mainMenu.Deactivated += (sender, e) =>
                            {
                                mainMenu.Close();
                            };
                            mainMenu.Show();
                            mainMenu.Activate();
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
            notifyIconUtil.AddNotifyIcon(new WindowInteropHelper(this).Handle, iconPath);
            notifyIconUtil.GetNotifyIconPosition(out notifyIconPosition);
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            notifyIconUtil.DeleteNotifyIcon();
        }

        private readonly NotifyIconUtil notifyIconUtil;
        private Rect notifyIconPosition;
        private MainMenuView? mainMenu = null;
    }
}
