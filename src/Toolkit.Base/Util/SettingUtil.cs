using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using Toolkit.Base.Log;

namespace Toolkit.Base.Util
{
    public class SettingUtil
    {
        public static void SaveSetting<T>(string name, T t) where T : class
        {
            try
            {
                string path = GetSettingPath(name, true);
                var bytes = JsonSerializer.SerializeToUtf8Bytes(t, new JsonSerializerOptions
                {
                    WriteIndented = false
                });
                FileUtil.SaveBytesToFile(path, bytes);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"保存设置失败，设置名称：{name}", MethodBase.GetCurrentMethod()?.GetMethodName());
            }
        }

        public static T? LoadSetting<T>(string name) where T : class
        {
            try
            {
                string path = GetSettingPath(name, false);
                if (!File.Exists(path)) return null;
                var bytes = FileUtil.ReadBytesFromFile(path);
                var utf8Reader = new Utf8JsonReader(bytes);
                return JsonSerializer.Deserialize<T>(ref utf8Reader);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"读取设置失败，设置名称：{name}", MethodBase.GetCurrentMethod()?.GetMethodName());
            }
            return null;
        }

        private static string GetSettingPath(string name, bool isSaveSetting)
        {
            if (path == string.Empty)
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings");
            }
            if (isSaveSetting && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return Path.Combine(path, $"{name}.Settings.json");
        }

        private static string path = string.Empty;
    }
}
