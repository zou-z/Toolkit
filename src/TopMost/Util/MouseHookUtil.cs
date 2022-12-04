using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Toolkit.Base.Common;

namespace TopMost.Util
{
    internal class MouseHookUtil
    {
        public event Action<double, double>? MouseLeftButtonDownEvent;
        public event Action<double, double>? MouseLeftButtonUpEvent;
        public event Action<double, double>? MouseRightButtonDownEvent;
        public event Action<double, double>? MouseMoveEvent;

        public int SetHook()
        {
            hookProc ??= new Win32Native.HookProc(MouseHookProc); // 回调函数必须是类的字段，否则会被GC回收导致报错
            hHook = Win32Native.SetWindowsHookEx(Win32Native.HookType.WH_MOUSE_LL, hookProc, IntPtr.Zero, 0);
            return hHook;
        }

        public bool UnHook()
        {
            return Win32Native.UnhookWindowsHookEx(hHook);
        }

        private IntPtr MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            object? param = Marshal.PtrToStructure(lParam, typeof(Win32Native.MouseHookStruct));
            if (param == null)
            {
                return Win32Native.CallNextHookEx(hHook, nCode, wParam, lParam);
            }
            Win32Native.MouseHookStruct mouseHookStruct = (Win32Native.MouseHookStruct)param;
            if (nCode < 0)
            {
                return Win32Native.CallNextHookEx(hHook, nCode, wParam, lParam);
            }
            else
            {
                switch (wParam.ToInt32())
                {
                    case Win32Native.WM_LBUTTONDOWN:
                        MouseLeftButtonDownEvent?.Invoke(mouseHookStruct.Point.X, mouseHookStruct.Point.Y);
                        break;
                    case Win32Native.WM_LBUTTONUP:
                        MouseLeftButtonUpEvent?.Invoke(mouseHookStruct.Point.X, mouseHookStruct.Point.Y);
                        break;
                    case Win32Native.WM_RBUTTONDOWN:
                        MouseRightButtonDownEvent?.Invoke(mouseHookStruct.Point.X, mouseHookStruct.Point.Y);
                        break;
                    case Win32Native.WM_MOUSEMOVE:
                        MouseMoveEvent?.Invoke(mouseHookStruct.Point.X, mouseHookStruct.Point.Y);
                        break;
                    default:
                        break;
                }
                return Win32Native.CallNextHookEx(hHook, nCode, wParam, lParam);
            }
        }

        private int hHook = 0;
        private Win32Native.HookProc? hookProc = null;
    }
}
