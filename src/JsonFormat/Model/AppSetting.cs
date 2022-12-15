﻿using CommunityToolkit.Mvvm.Input;
using JsonFormat.Model.Setting;
using JsonFormat.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Toolkit.Base.Util;

namespace JsonFormat.Model
{
    internal class AppSetting
    {
        public RelayCommand OpenSettingCommand => openSettingCommand ??= new RelayCommand(OpenSetting);

        public Settings Settings { get; private set; }

        public AppSetting()
        {
            if (Assembly.GetExecutingAssembly().GetName().Name is string name)
            {
                currentAssemblyName = name;
            }
            else
            {
                currentAssemblyName = "JsonFormat";
            }
            if (SettingUtil.LoadSetting<Settings>(currentAssemblyName) is Settings _settings)
            {
                Settings = _settings;
            }
            else
            {
                Settings = new Settings();
            }
            //SettingUtil.SaveSetting(currentAssemblyName, Settings);
        }

        private void OpenSetting()
        {
            if (settingWindow == null)
            {
                settingWindow = new SettingWindow { DataContext = this, };
                foreach (var window in Application.Current.Windows)
                {
                    if (window is MainWindow mainWindow)
                    {
                        settingWindow.Owner = mainWindow;
                        settingWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        break;
                    }
                }
            }
            settingWindow.Show();
        }

        private RelayCommand? openSettingCommand = null;
        private SettingWindow? settingWindow = null;
        private readonly string currentAssemblyName;
    }
}
