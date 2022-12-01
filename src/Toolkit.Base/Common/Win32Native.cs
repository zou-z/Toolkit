using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace Toolkit.Base.Common
{
    public static class Win32Native
    {
#pragma warning disable SYSLIB1054
#pragma warning disable CA1401

        #region Common Data Type
        public const int NULL = 0;

        [StructLayout(LayoutKind.Sequential)]
        public struct Win32Point
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Win32Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        #endregion

        #region GetLastError
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern long GetLastError();
        #endregion

        #region Shell_NotifyIcon
        [StructLayout(LayoutKind.Sequential)]
        public struct NotifyIconData
        {
            public int CbSize; // DWORD
            public IntPtr Hwnd; // HWND
            public int UID; // UINT
            public uint UFlags; // UINT
            public uint UCallbackMessage; // UINT
            public IntPtr HIcon; // HICON
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string SzTip; // char[128]
            public int DwState; // DWORD
            public int DwStateMask; // DWORD
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string SzInfo; // char[256]
            public int UTimeoutOrVersion; // UINT
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string SzInfoTitle; // char[64]
            public int DwInfoFlags; // DWORD
            //public Guid GuidItem;   //GUID guidItem; > IE 6
        }

        public enum NotifyIconMessage : uint
        {
            NIM_ADD = 0x00000000,
            NIM_MODIFY = 0x00000001,
            NIM_DELETE = 0x00000002,
            NIM_SETFOCUS = 0x00000003,
            NIM_SETVERSION = 0x00000004,
        }

        public const uint NIF_MESSAGE = 0x00000001;
        public const uint NIF_ICON = 0x00000002;
        public const uint NIF_TIP = 0x00000004;
        public const uint NIF_SHOWTIP = 0x00000080;
        public const uint NIF_GUID = 0x00000020;

        public const int WM_APP = 0x8000;

        [DllImport("shell32.dll")]
        public static extern bool Shell_NotifyIcon(uint dwMessage, [In] ref NotifyIconData pnid);
        #endregion

        #region Shell_NotifyIconGetRect
        [StructLayout(LayoutKind.Sequential)]
        public struct NotifyIconIdentifier
        {
            public int CbSize;
            public IntPtr Hwnd;
            public uint UID;
            public Guid GuidItem;
        }

        [DllImport("shell32.dll", SetLastError = true)]
        public static extern int Shell_NotifyIconGetRect([In] ref NotifyIconIdentifier identifier, [Out] out Win32Rect iconLocation);
        #endregion

        #region SetParent
        public const int HWND_MESSAGE = -3;

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hwnd, IntPtr hwndNewParent);
        #endregion

        #region GetAsyncKeyState
        public enum VirtualKeyCodes : int
        {
            VK_LBUTTON = 0x01,
            VK_ESCAPE = 0x1B,
        }

        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(int vKeys);
        #endregion

        #region MouseHook
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_LBUTTONDBLCLK = 0x203;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_RBUTTONDBLCLK = 0x206;
        public const int WM_MBUTTONDOWN = 0x207;
        public const int WM_MBUTTONUP = 0x208;
        public const int WM_MBUTTONDBLCLK = 0x209;

        public enum HookType : int
        {
            WH_MOUSE = 7,
            WH_MOUSE_LL = 14,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MouseHookStruct
        {
            public Win32Point Point;
            public IntPtr Hwnd;
            public int WHitTestCode;
            public int DwExtraInfo;
        }

        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(HookType hookType, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);
        #endregion

        #region GetWindowTextLength
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetWindowTextLength(IntPtr hwnd);
        #endregion

        #region GetWindowText
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hwnd, StringBuilder lpString, int cch);
        #endregion

        #region GetWindow
        public const uint GW_HWNDNEXT = 2;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindow(IntPtr hwnd, uint windowType);
        #endregion

        #region GetWindowRect
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out Win32Rect lpRect);
        #endregion

        #region GetDesktopWindow
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        #endregion

        #region GetTopWindow
        [DllImport("user32.dll")]
        public static extern IntPtr GetTopWindow(IntPtr hWnd);
        #endregion

        #region GetClassName
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int nMaxCount);
        #endregion

        #region GetShellWindow
        [DllImport("user32.dll")]
        public static extern IntPtr GetShellWindow();
        #endregion

        #region IsWindowVisible
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hwnd);
        #endregion

        #region GetAncestor
        public const uint GA_ROOT = 2;

        [DllImport("user32.dll")]
        public static extern IntPtr GetAncestor(IntPtr hwnd, uint gaFlags);
        #endregion

        #region GetWindowLong
        public const int GWL_STYLE = -16;
        public const int GWL_EXSTYLE = -20;
        public const long WS_DISABLED = 0x08000000L;
        public const long WS_EX_TOOLWINDOW = 0x00000080L;
        public const long WS_EX_TOPMOST = 0x00000008L;

        [DllImport("user32.dll")]
        public static extern long GetWindowLong(IntPtr hwnd, int nIndex);
        #endregion

        #region DwmGetWindowAttribute
        public const int DWM_CLOAKED_SHELL = 0x0000002;
        
        public enum DWMWINDOWATTRIBUTE
        {
            DWMWA_NCRENDERING_ENABLED = 1,
            DWMWA_NCRENDERING_POLICY,
            DWMWA_TRANSITIONS_FORCEDISABLED,
            DWMWA_ALLOW_NCPAINT,
            DWMWA_CAPTION_BUTTON_BOUNDS,
            DWMWA_NONCLIENT_RTL_LAYOUT,
            DWMWA_FORCE_ICONIC_REPRESENTATION,
            DWMWA_FLIP3D_POLICY,
            DWMWA_EXTENDED_FRAME_BOUNDS,
            DWMWA_HAS_ICONIC_BITMAP,
            DWMWA_DISALLOW_PEEK,
            DWMWA_EXCLUDED_FROM_PEEK,
            DWMWA_CLOAK,
            DWMWA_CLOAKED,
            DWMWA_FREEZE_REPRESENTATION,
            DWMWA_LAST
        };

        [DllImport("dwmapi.dll")]
        public static extern uint DwmGetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, out int pvAttribute, int cbAttribute);
        #endregion

#pragma warning restore CA1401
#pragma warning restore SYSLIB1054
    }
}
