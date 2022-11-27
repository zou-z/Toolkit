using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Toolkit.Base.Common.Win32Native;

namespace TopMost.Util
{
    internal static class MouseButtonStatusUtil
    {
        public static bool IsMouseLeftButtonDown()
        {
            int state = GetAsyncKeyState((int)VirtualKeyCodes.VK_LBUTTON);
            return (state & 0x8000) > 0;
        }
    }
}
