using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using Toolkit.Base.Common;
using Toolkit.Base.Log;
using TopMost.Util;
using TopMost.View;

namespace TopMost.ViewModel
{
    internal class AreaIndicationViewModel : ObservableObject
    {
        public string WindowTitle
        {
            get => windowTitle;
            set => SetProperty(ref windowTitle, value);
        }

        public AreaIndicationViewModel()
        {
        }

        public async void Start()
        {
            if (!MouseButtonStatusUtil.IsMouseLeftButtonDown()) return;
            await Task.Run(() =>
            {
                windowInfos?.Clear();
                try
                {
                    windowInfos = WindowListUtil.GetWindowList(IntPtr.Zero);
                    windowInfos = windowInfos.FindAll(t => !t.IsMinimized);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "获取窗口列表失败", MethodBase.GetCurrentMethod()?.GetMethodName());
                }
            });
            if (!MouseButtonStatusUtil.IsMouseLeftButtonDown()) return;
            InitAreaIndicationView();
            InitMouseHook();
            if (!SetMouseHook()) return;
            if (areaIndicationView != null)
            {
                areaIndicationView.Width = areaIndicationView.Height = areaIndicationView.Left = areaIndicationView.Top = 0;
                areaIndicationView.Show();
                areaIndicationViewHandle = new WindowInteropHelper(areaIndicationView).Handle;
            }
            isRepositionWindow = false;
        }

        private void InitAreaIndicationView()
        {
            areaIndicationView ??= new AreaIndicationView
            {
                Width = 200,
                Height = 200,
                Topmost = true,
                ShowInTaskbar = false,
                DataContext = this,
            };
        }

        private void InitMouseHook()
        {
            if (mouseHookUtil == null)
            {
                mouseHookUtil = new MouseHookUtil();
                mouseHookUtil.MouseLeftButtonUpEvent += MouseHookUtil_MouseLeftButtonUpEvent;
                mouseHookUtil.MouseMoveEvent += MouseHookUtil_MouseMoveEvent;
            }
        }

        private bool SetMouseHook()
        {
            if (mouseHookUtil?.SetHook() == Win32Native.NULL)
            {
                long errorCode = Win32Native.GetLastError();
                Logger.Error(new Exception($"GetLastError:{errorCode}"), "安装鼠标钩子失败", MethodBase.GetCurrentMethod()?.GetMethodName());
                return false;
            }
            return true;
        }

        private void RemoveMouseHook()
        {
            if (mouseHookUtil != null && !mouseHookUtil.UnHook())
            {
                long errorCode = Win32Native.GetLastError();
                Logger.Error(new Exception($"GetLastError:{errorCode}"), "卸载鼠标钩子失败", MethodBase.GetCurrentMethod()?.GetMethodName());
            }
        }

        private void MouseHookUtil_MouseLeftButtonUpEvent(double x, double y)
        {
            RemoveMouseHook();
            areaIndicationView?.Hide();
        }

        private void MouseHookUtil_MouseMoveEvent(double x, double y)
        {
            if (isRepositionWindow) return;
            isRepositionWindow = true;
            if (windowInfos?.Count > 0 && areaIndicationView != null)
            {
                foreach (var windowInfo in windowInfos)
                {
                    if (windowInfo.Left < x && x < windowInfo.Right && windowInfo.Top < y && y < windowInfo.Bottom)
                    {
                        if (windowInfo.Equals(displayedWindowInfo))
                        {
                            isRepositionWindow = false;
                            return;
                        }
                        displayedWindowInfo = windowInfo;
                        _ = areaIndicationView.Dispatcher.BeginInvoke(() =>
                        {
                            Win32Native.MoveWindow(
                                areaIndicationViewHandle,
                                windowInfo.Left,
                                windowInfo.Top,
                                windowInfo.Right - windowInfo.Left,
                                windowInfo.Bottom - windowInfo.Top,
                                true);
                            WindowTitle = windowInfo.Title;
                            // icon
                        });
                        isRepositionWindow = false;
                        return;
                    }
                }
            }
            areaIndicationView?.Hide();
            isRepositionWindow = false;
        }

        private AreaIndicationView? areaIndicationView = null;
        private IntPtr areaIndicationViewHandle = IntPtr.Zero;
        private MouseHookUtil? mouseHookUtil = null;
        private List<WindowListUtil.WindowInfo>? windowInfos = null;
        private WindowListUtil.WindowInfo? displayedWindowInfo = null;
        private bool isRepositionWindow = false;
        private string windowTitle = string.Empty;
    }
}
