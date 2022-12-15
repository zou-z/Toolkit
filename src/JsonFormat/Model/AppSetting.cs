using CommunityToolkit.Mvvm.Input;
using JsonFormat.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JsonFormat.Model
{
    internal class AppSetting
    {
        public RelayCommand OpenSettingCommand => openSettingCommand ??= new RelayCommand(OpenSetting);

        public AppSetting()
        {
            //LoadSetting();
        }

        private void LoadSetting()
        {
            System.Threading.Thread.Sleep(3000);
            MessageBox.Show("模拟加载设置完成");
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
    }
}
