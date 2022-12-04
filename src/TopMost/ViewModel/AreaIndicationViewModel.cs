using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media;
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

        public ImageSource? WindowIcon
        {
            get => windowIcon;
            set => SetProperty(ref windowIcon, value);
        }

        public bool WindowTopMost
        {
            get => windowTopMost;
            set => SetProperty(ref windowTopMost, value);
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
                areaIndicationView.Hide();
            }
            displayedWindowInfo = null;
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
                mouseHookUtil.MouseRightButtonDownEvent += MouseHookUtil_MouseRightButtonDownEvent;
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
            MouseHookUtil_MouseRightButtonDownEvent(x, y);
            if (displayedWindowInfo != null)
            {
                displayedWindowInfo.IsTopmost = !displayedWindowInfo.IsTopmost;
                if (!TopMostUtil.SetIsTopMost(displayedWindowInfo.Handle, displayedWindowInfo.IsTopmost))
                {
                    long errorCode = Win32Native.GetLastError();
                    Logger.Error(
                        new Exception($"GetLastError:{errorCode}"),
                        $"{(displayedWindowInfo.IsTopmost ? "设置" : "取消")}窗口置顶失败，窗口标题：{displayedWindowInfo.Title}，窗口句柄：{displayedWindowInfo.Handle}",
                        MethodBase.GetCurrentMethod()?.GetMethodName());
                }
            }
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
                        int left = windowInfo.Left, top = windowInfo.Top;
                        int width = windowInfo.Right - windowInfo.Left, height = windowInfo.Bottom - windowInfo.Top;
                        if (windowInfo.IsMaximized)
                        {
                            left += 8;
                            top += 8;
                            width -= 16;
                            height -= 16;
                        }
                        
                        _ = areaIndicationView.Dispatcher.BeginInvoke(() =>
                        {
                            Win32Native.MoveWindow(areaIndicationViewHandle, left, top, width, height, true);
                            WindowTitle = windowInfo.Title;
                            WindowIcon = IconCache.GetIcon(windowInfo.Handle);
                            WindowTopMost = windowInfo.IsTopmost;
                            areaIndicationView.Show();
                        });
                        isRepositionWindow = false;
                        return;
                    }
                }
            }
            areaIndicationView?.Hide();
            displayedWindowInfo = null;
            isRepositionWindow = false;
        }

        private void MouseHookUtil_MouseRightButtonDownEvent(double arg1, double arg2)
        {
            RemoveMouseHook();
            areaIndicationView?.Hide();
        }

        private AreaIndicationView? areaIndicationView = null;
        private IntPtr areaIndicationViewHandle = IntPtr.Zero;
        private MouseHookUtil? mouseHookUtil = null;
        private List<WindowListUtil.WindowInfo>? windowInfos = null;
        private WindowListUtil.WindowInfo? displayedWindowInfo = null;
        private bool isRepositionWindow = false;
        private string windowTitle = string.Empty;
        private ImageSource? windowIcon = null;
        private bool windowTopMost = false;
    }
}
