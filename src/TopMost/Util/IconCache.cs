using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TopMost.Util
{
    internal static class IconCache
    {
        public static BitmapSource? GetIcon(IntPtr hwnd)
        {
            lock (lockObj)
            {
                cache ??= new Dictionary<IntPtr, BitmapSource>();
                if (cache.TryGetValue(hwnd, out BitmapSource? value))
                {
                    return value;
                }

                BitmapSource? bitmap = IconUtil.GetIcon(hwnd);
                if (bitmap != null)
                {
                    cache[hwnd] = bitmap;
                }
                return bitmap;
            }
        }

        private static Dictionary<IntPtr, BitmapSource>? cache = null;
        private readonly static object lockObj = new();
    }
}
