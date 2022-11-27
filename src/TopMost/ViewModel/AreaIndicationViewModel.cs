using System;
using System.Collections.Generic;
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

        public void Start()
        {
            if (!MouseButtonStatusUtil.IsMouseLeftButtonDown()) return;
            if (mouseHookUtil == null)
            {
                mouseHookUtil = new MouseHookUtil();
                mouseHookUtil.MouseLeftButtonUpEvent += MouseHookUtil_MouseLeftButtonUpEvent;
                mouseHookUtil.MouseMoveEvent += MouseHookUtil_MouseMoveEvent;
            }
            areaIndicationView ??= new AreaIndicationView
            {
                Width = 200,
                Height = 200,
                Topmost = true,
                ShowInTaskbar = false,
            };
            if (mouseHookUtil.SetHook() == Win32Native.NULL)
            {
                long errorCode = Win32Native.GetLastError();
                Logger.Error(new Exception($"GetLastError:{errorCode}"), "安装鼠标钩子失败", MethodBase.GetCurrentMethod()?.GetMethodName());
                return;
            }
            areaIndicationView.Show();
        }

        private void MouseHookUtil_MouseLeftButtonUpEvent(double x, double y)
        {
            if (mouseHookUtil != null && !mouseHookUtil.UnHook())
            {
                long errorCode = Win32Native.GetLastError();
                Logger.Error(new Exception($"GetLastError:{errorCode}"), "卸载鼠标钩子失败", MethodBase.GetCurrentMethod()?.GetMethodName());
            }
            areaIndicationView?.Hide();
        }

        private void MouseHookUtil_MouseMoveEvent(double x, double y)
        {
            if (areaIndicationView != null)
            {
                areaIndicationView.Left = x + 10;
                areaIndicationView.Top = y + 10;
            }
        }

        private AreaIndicationView? areaIndicationView = null;
        private MouseHookUtil? mouseHookUtil = null;
    }
}
