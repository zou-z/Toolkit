using JsonFormat.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JsonFormat.Core
{
    internal static class JsonParser
    {
        public static object Parse(string json)
        {
            json = StringUtil.StringCompression(json);
            CheckNullOrEmptyString(json);
            string result = GetJson(json, out object obj);
            if (result.Length > 0)
            {
                throw new JsonParseException($"含有多余的字符串 {result}", json);
            }
            return obj;
        }

        private static string GetJson(string text, out object obj)
        {
            string result = string.Empty;
            obj = string.Empty;
            if (GetValueType(text) == ValueType.Object)
            {
                result = GetJsonObject(text, out Dictionary<string, object> dict);
                obj = dict;
            }
            else if (GetValueType(text) == ValueType.Array)
            {
                result = GetJsonArray(text, out List<object> list);
                obj = list;
            }
            else if (GetValueType(text) == ValueType.String)
            {
                result = GetJsonString(text, out string str);
                obj = $"\"{str}\"";
            }
            else if (GetValueType(text) == ValueType.Boolean)
            {
                result = GetJsonBoolean(text, out bool b);
                obj = b;
            }
            else if (GetValueType(text) == ValueType.Null)
            {
                result = GetJsonNull(text, out string str);
                obj = str;
            }
            else if (GetValueType(text) == ValueType.Number)
            {
                result = GetJsonNumber(text, out double number);
                obj = number;
            }
            return result;
        }

        private static string GetJsonObject(string text, out Dictionary<string, object> dict)
        {
            dict = new Dictionary<string, object>();
            string str = text;
            try
            {
                str = str[1..];
                while (true)
                {
                    if (str.Length == 0)
                    {
                        throw new JsonParseException("无效的Object，末尾缺少字符}", text);
                    }
                    if (str[0] == '}')
                    {
                        return str.Length == 1 ? "" : str[1..];
                    }

                    // 解析key
                    if (GetValueType(str) != ValueType.String)
                    {
                        throw new JsonParseException("键值对需要以字符\"开头", str);
                    }
                    string result = GetJsonString(str, out string key);
                    if (key.Length == 0)
                    {
                        throw new JsonParseException("键值对的Key不能为空", str);
                    }
                    if (dict.ContainsKey(key))
                    {
                        throw new JsonParseException($"Object中含有相同的Key：{key}", text);
                    }

                    if (result.Length == 0 || result[0] != ':')
                    {
                        throw new JsonParseException($"字符串：{result}，缺少字符:", str);
                    }
                    str = result[1..];

                    // 解析value
                    str = GetJson(str, out object obj);
                    dict.Add(key, obj);

                    if (str.Length > 0)
                    {
                        if (str[0] == ',')
                        {
                            str = str[1..];
                        }
                        else if (str[0] != '}')
                        {
                            throw new JsonParseException($"字符串：{str}，缺少字符,或}}", str);
                        }
                    }
                }
            }
            catch (JsonParseException) { throw; }
            catch (Exception) { throw; }
        }

        private static string GetJsonArray(string text, out List<object> list)
        {
            list = new List<object>();
            string str = text;
            try
            {
                str = str[1..];
                while (true)
                {
                    if (str.Length == 0)
                    {
                        throw new JsonParseException("无效的Array，末尾缺少字符]", text);
                    }
                    if (str[0] == ']')
                    {
                        return str.Length == 1 ? "" : str[1..];
                    }

                    // 解析value
                    str = GetJson(str, out object obj);
                    list.Add(obj);

                    if (str.Length > 0)
                    {
                        if (str[0] == ',')
                        {
                            str = str[1..];
                        }
                        else if (str[0] != ']')
                        {
                            throw new JsonParseException($"字符串：{str}，缺少字符,或]", str);
                        }
                    }
                }
            }
            catch (JsonParseException) { throw; }
            catch (Exception) { throw; }
        }

        private static string GetJsonString(string text, out string strString)
        {
            strString = string.Empty;
            string str = text;
            str = str[1..];
            if (str.Length > 0)
            {
                for (int i = 0; i < str.Length; ++i)
                {
                    if (str[i] == '"')
                    {
                        strString = str[..i];
                        return str[(i + 1)..];
                    }
                }
            }
            throw new JsonParseException("无效的String，末尾缺少字符\"", text);
        }

        private static string GetJsonBoolean(string text, out bool b)
        {
            if (text.StartsWith("true"))
            {
                b = true;
                return text[4..];
            }
            if (text.StartsWith("false"))
            {
                b = false;
                return text[5..];
            }
            throw new JsonParseException("无效的Boolean值", text);
        }

        private static string GetJsonNull(string text, out string nullString)
        {
            if (text.StartsWith("null"))
            {
                nullString = "null";
                return text[4..];
            }
            throw new JsonParseException("无效的Null值", text);
        }

        private static string GetJsonNumber(string text, out double number)
        {
            int i = 0;
            for (; i < text.Length; ++i)
            {
                if (text[i] == ',' || text[i] == ']' || text[i] == '}')
                {
                    break;
                }
            }
            string str = text[..i];
            if (double.TryParse(str, out double _number))
            {
                number = _number;
                return text[i..];
            }
            throw new JsonParseException($"无效的数字 {str}", text);
        }

        private static ValueType GetValueType(string text)
        {
            CheckNullOrEmptyString(text);
            return text[0] switch
            {
                '{' => ValueType.Object,
                '[' => ValueType.Array,
                '"' => ValueType.String,
                't' or 'f' => ValueType.Boolean,
                'n' => ValueType.Null,
                _ => ValueType.Number,
            };
        }

        private static void CheckNullOrEmptyString(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new Exception("空字符串无法判断类型");
            }
        }

        private enum ValueType
        {
            Object,   // 以{}开头和结尾
            Array,    // 以[]开头和结尾
            String,   // 以"开头和结尾
            Boolean,  // true或者false
            Null,     // null
            Number,   // 除此之外皆为数字
        }
    }
}
