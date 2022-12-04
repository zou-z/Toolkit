using System;
using Toolkit.Base.Common;

namespace TopMost.Util
{
    internal static class TopMostUtil
    {
        public static bool SetIsTopMost(IntPtr hwnd, bool isTopmost)
        {
            return Win32Native.SetWindowPos(
                hwnd,
                isTopmost ? Win32Native.HWND_TOPMOST : Win32Native.HWND_NOTOPMOST,
                0, 0, 0, 0,
                Win32Native.SWP_NOMOVE | Win32Native.SWP_NOSIZE);
        }
    }
}
