using System;
using System.Runtime.InteropServices;

namespace Toolkit.Base.Common
{
    public static class Win32Native
    {
#pragma warning disable SYSLIB1054
#pragma warning disable CA1401

        #region Common Data Type
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

#pragma warning restore CA1401
#pragma warning restore SYSLIB1054
    }
}
