using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Toolkit.Base.Common;
using Toolkit.Base.Log;

namespace TopMost.Util
{
    internal static class WindowListUtil
    {
        public class WindowInfo
        {
            public IntPtr Handle { get; private set; }

            public string Title { get; private set; }

            public object? Icon { get; set; }

            public int Left { get; set; }

            public int Top { get; set; }

            public int Right { get; set; }

            public int Bottom { get; set; }

            public bool IsTopmost { get; set; }

            public bool IsMaximized { get; set; }

            public bool IsMinimized { get; set; }

            public WindowInfo(IntPtr handle)
            {
                Handle = handle;
                int length = Win32Native.GetWindowTextLength(Handle) * 2;
                var stringBuilder = new StringBuilder(length);
                _ = Win32Native.GetWindowText(Handle, stringBuilder, length);
                Title = stringBuilder.ToString();
                Icon = null;
                Left = Top = Right = Bottom = 0;
                IsTopmost = false;
                IsMaximized = IsMinimized = false;
            }
        }

        public static List<WindowInfo> GetWindowList(IntPtr ignoredWindow)
        {
            var list = new List<WindowInfo>();
            IntPtr hwnd = GetTopMostWindowHandle();
            while ((hwnd = Win32Native.GetWindow(hwnd, Win32Native.GW_HWNDNEXT)) != IntPtr.Zero)
            {
                if (hwnd != ignoredWindow)
                {
                    var windowInfo = new WindowInfo(hwnd);
                    if (IsCapturableWindow(windowInfo))
                    {
                        if (Win32Native.GetWindowRect(hwnd, out Win32Native.Win32Rect rect))
                        {
                            windowInfo.Left = rect.Left;
                            windowInfo.Top = rect.Top;
                            windowInfo.Right = rect.Right;
                            windowInfo.Bottom = rect.Bottom;
                        }
                        windowInfo.IsMaximized = Win32Native.IsZoomed(hwnd) != 0;
                        if (!windowInfo.IsMaximized)
                        {
                            windowInfo.IsMinimized = Win32Native.IsIconic(hwnd) != 0;
                        }
                        list.Add(windowInfo);
                    }
                }
            }
            return list;
        }

        private static IntPtr GetTopMostWindowHandle()
        {
            IntPtr desktopHandle = Win32Native.GetDesktopWindow();
            IntPtr topMostWindowHandle = Win32Native.GetTopWindow(desktopHandle);
            if (topMostWindowHandle == IntPtr.Zero)
            {
                long errorCode = Win32Native.GetLastError();
                Logger.Error(new Exception($"GetLastError:{errorCode}"), "获取顶部窗口失败", MethodBase.GetCurrentMethod()?.GetMethodName());
                return IntPtr.Zero;
            }
            return topMostWindowHandle;
        }

        private static string GetClassName(IntPtr hwnd)
        {
            var className = new StringBuilder(256);
            if (Win32Native.GetClassName(hwnd, className, 256) == 0)
            {
                long errorCode = Win32Native.GetLastError();
                Logger.Error(new Exception($"GetLastError:{errorCode}"), "获取窗口所属的类的名称失败", MethodBase.GetCurrentMethod()?.GetMethodName());
                return string.Empty;
            }
            return className.ToString();
        }

        private static bool IsKnownBlockedWindow(WindowInfo windowInfo, string className)
        {
            return windowInfo.Title == "Task View" && className == "Windows.UI.Core.CoreWindow" ||
                windowInfo.Title == "DesktopWindowXamlSource" && className == "Windows.UI.Core.CoreWindow" ||
                windowInfo.Title == "PopupHost" && className == "Xaml_WindowedPopupClass";
        }

        private static bool IsCapturableWindow(WindowInfo windowInfo)
        {
            if (string.IsNullOrEmpty(windowInfo.Title) ||
                windowInfo.Handle == Win32Native.GetShellWindow() ||
                !Win32Native.IsWindowVisible(windowInfo.Handle) ||
                Win32Native.GetAncestor(windowInfo.Handle, Win32Native.GA_ROOT) != windowInfo.Handle)
            {
                return false;
            }

            long style = Win32Native.GetWindowLong(windowInfo.Handle, Win32Native.GWL_STYLE);
            if ((style & Win32Native.WS_DISABLED) == Win32Native.WS_DISABLED)
            {
                return false;
            }

            style = Win32Native.GetWindowLong(windowInfo.Handle, Win32Native.GWL_EXSTYLE);
            if ((style & Win32Native.WS_EX_TOOLWINDOW) == Win32Native.WS_EX_TOOLWINDOW)
            {
                return false;
            }

            string className = GetClassName(windowInfo.Handle);
            if (className == "Windows.UI.Core.CoreWindow" || className == "ApplicationFrameWindow")
            {
                if (Win32Native.DwmGetWindowAttribute(windowInfo.Handle, Win32Native.DWMWINDOWATTRIBUTE.DWMWA_CLOAKED, out int cloaked, sizeof(int)) == 0 &&
                    cloaked == Win32Native.DWM_CLOAKED_SHELL)
                {
                    return false;
                }
            }

            if (IsKnownBlockedWindow(windowInfo, className))
            {
                return false;
            }

            windowInfo.IsTopmost = (style & Win32Native.WS_EX_TOPMOST) == Win32Native.WS_EX_TOPMOST;
            return true;
        }
    }
}
