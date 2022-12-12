using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonFormat.Util
{
    internal static class StringUtil
    {
        // 字符串压缩
        public static string StringCompression(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;
            // 去除换行，回车和水平制表符
            str = str.Replace("\r", "").Replace("\n", "").Replace("\r\n", "").Replace("\t", "");
            // 只去除双引号之外的空格
            bool flag = true;
            for (int i = 0; i < str.Length;)
            {
                if (str[i] == '\"')
                {
                    flag = !flag;
                }
                else if (str[i] == ' ' && flag)
                {
                    str = str.Remove(i, 1);
                    continue;
                }
                ++i;
            }
            return str;
        }

        // 字符串添加转义(两个双引号之间的字符也会被执行)
        public static string StringAddEscape(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;
            return str.Replace("\"", "\\\"");
        }

        // 字符串去除转义(两个双引号之间的字符也会被执行)
        public static string StringRemoveEscape(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;
            return str.Replace("\\\"", "\"");
        }
    }
}
