using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Toolkit.Base.Common;

namespace Toolkit.Util
{
    internal class NotifyIconUtil
    {
        public const int NotifyIconMessage = Win32Native.WM_APP + 1;

        public bool AddNotifyIcon(IntPtr windowHandle, string iconPath)
        {
            this.windowHandle = windowHandle;
            notifyIconData = new Win32Native.NotifyIconData
            {
                Hwnd = windowHandle,
                UFlags = Win32Native.NIF_MESSAGE | Win32Native.NIF_ICON | Win32Native.NIF_TIP | Win32Native.NIF_SHOWTIP | Win32Native.NIF_GUID,
                UCallbackMessage = NotifyIconMessage,
                HIcon = IconUtil.GetIconFromFile(iconPath),
                SzTip = "this is tip"
            };
            if (!Win32Native.Shell_NotifyIcon((uint)Win32Native.NotifyIconMessage.NIM_ADD, ref notifyIconData)) return false;
            notifyIconData.UTimeoutOrVersion = 4;
            if (!Win32Native.Shell_NotifyIcon((uint)Win32Native.NotifyIconMessage.NIM_SETVERSION, ref notifyIconData)) return false;
            return true;
        }

        public bool DeleteNotifyIcon()
        {
            return Win32Native.Shell_NotifyIcon((uint)Win32Native.NotifyIconMessage.NIM_DELETE, ref notifyIconData);
        }

        public bool GetNotifyIconPosition(out Rect position)
        {
            var notifyIconIdentifier = new Win32Native.NotifyIconIdentifier()
            {
                CbSize = Marshal.SizeOf(typeof(Win32Native.NotifyIconIdentifier)),
                Hwnd = windowHandle,
            };
            if (Win32Native.Shell_NotifyIconGetRect(ref notifyIconIdentifier, out Win32Native.Win32Rect rect) != 0) return false;
            position = new Rect(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            return true;
        }

        private IntPtr windowHandle = IntPtr.Zero;
        private Win32Native.NotifyIconData notifyIconData;
    }
}
