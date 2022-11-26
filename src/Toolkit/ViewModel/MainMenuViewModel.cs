using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Toolkit.Base.Log;
using Toolkit.Contract;
using Toolkit.Model;

namespace Toolkit.ViewModel
{
    internal class MainMenuViewModel
    {
        public ObservableCollection<Plugin> Plugins => plugins ??= new ObservableCollection<Plugin>();

        public MainMenuViewModel()
        {
        }

        public async Task LoadPluginsAsync()
        {
            await Task.Run(() =>
            {
                var pluginFiles = GetPluginFiles();
                if (pluginFiles == null) return;
                foreach (string pluginFile in pluginFiles)
                {
                    IPlugin? plugin = LoadAssembly<IPlugin>(pluginFile);
                    if (plugin != null)
                    {
                        Plugins.Add(new Plugin(plugin));
                    }
                }
            });
        }

        private List<string>? GetPluginFiles()
        {
            if (!Directory.Exists(pluginFolder)) return null;
            var directoryInfo = new DirectoryInfo(pluginFolder);
            var files = directoryInfo.GetFiles();
            if (files.Length == 0) return null;
            var pluginFiles = new List<string>(files.Length);
            foreach (var file in files)
            {
                pluginFiles.Add(file.FullName);
            }
            return pluginFiles;
        }

        /// <summary>
        /// 加载程序集
        /// <para>1.C#程序集</para>
        /// <para>2.public class</para>
        /// <para>3.implements T</para>
        /// </summary>
        /// <typeparam name="T">需要创建的类型</typeparam>
        /// <param name="path">程序集的绝对路径</param>
        /// <returns>T t</returns>
        private static T? LoadAssembly<T>(string path) where T : class
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(path);
                foreach (Type type in assembly.GetTypes())
                {
                    if ((type.Attributes & TypeAttributes.Class) == TypeAttributes.Class &&
                        (type.Attributes & TypeAttributes.Public) == TypeAttributes.Public &&
                        typeof(T).IsAssignableFrom(type))
                    {
                        if (Activator.CreateInstance(type) is T plugin)
                        {
                            return plugin;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Info(ex, "加载程序集失败", MethodBase.GetCurrentMethod()?.GetMethodName());
            }
            return null;
        }

        private ObservableCollection<Plugin>? plugins = null;
        private readonly string pluginFolder = "Plugin";
    }
}
