using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkit.Util
{
    internal class IconUtil
    {
        public static IntPtr GetIconFromFile(string filePath)
        {
            using var image = System.Drawing.Image.FromFile(filePath);
            using var bitmap = new System.Drawing.Bitmap(image);
            return bitmap.GetHicon();
        }
    }
}
