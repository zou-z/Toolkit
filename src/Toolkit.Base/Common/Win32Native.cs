using System;
using System.Runtime.InteropServices;

namespace Toolkit.Base.Common
{
    public static class Win32Native
    {
#pragma warning disable SYSLIB1054
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
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NotifyIconIdentifier
        {
            public int CbSize;
            public IntPtr Hwnd;
            public uint UID;
            public Guid GuidItem;
        }


        [DllImport("shell32.dll", SetLastError = true)]
        public static extern int Shell_NotifyIconGetRect([In] ref NotifyIconIdentifier identifier, [Out] out Rect iconLocation);
        #endregion

        #region SetParent
        public const int HWND_MESSAGE = -3;

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hwnd, IntPtr hwndNewParent);
        #endregion
#pragma warning restore SYSLIB1054
    }
}
