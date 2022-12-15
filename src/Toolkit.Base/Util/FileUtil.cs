using System;
using System.IO;

namespace Toolkit.Base.Util
{
    public static class FileUtil
    {
        public static void SaveBytesToFile(string path, byte[] array)
        {
            using var fs = new FileStream(path, FileMode.Create);
            fs.Write(array, 0, array.Length);
        }

        public static byte[] ReadBytesFromFile(string path)
        {
            if (!File.Exists(path))
            {
                return Array.Empty<byte>();
            }
            using var fs = new FileStream(path, FileMode.Open);
            byte[] array = new byte[fs.Length];
            fs.Read(array, 0, array.Length);
            return array;
        }
    }
}
