using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Toolkit.Base.Common;
using Toolkit.Base.Log;
using TopMost.Util;
using TopMost.View;

namespace TopMost.ViewModel
{
    internal class AreaIndicationViewModel
    {
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
            }
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
            if (windowInfos?.Count > 0 && areaIndicationView != null)
            {
                foreach (var windowInfo in windowInfos)
                {
                    if (windowInfo.Left < x && x < windowInfo.Right && windowInfo.Top < y && y < windowInfo.Bottom)
                    {
                        areaIndicationView.Left = windowInfo.Left;
                        areaIndicationView.Top = windowInfo.Top;
                        areaIndicationView.Width = windowInfo.Right - windowInfo.Left;
                        areaIndicationView.Height = windowInfo.Bottom - windowInfo.Top;
                        // icon title
                        return;
                    }
                }
            }
            areaIndicationView?.Hide();
        }

        private AreaIndicationView? areaIndicationView = null;
        private MouseHookUtil? mouseHookUtil = null;
        private List<WindowListUtil.WindowInfo>? windowInfos = null;
    }
}
