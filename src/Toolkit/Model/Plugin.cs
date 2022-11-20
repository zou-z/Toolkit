using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NLog.Targets.Wrappers;
using Toolkit.Message;

namespace Toolkit.Model
{
    internal class Plugin
    {
        public string Name { get; set; } = string.Empty;

        public RelayCommand MouseLeftButtonDownCommand => mouseLeftButtonDownCommand ??= new RelayCommand(MouseLeftButtonDown);

        public RelayCommand MouseLeftButtonUpCommand => mouseLeftButtonUpCommand ??= new RelayCommand(MouseLeftButtonUp);

        public Plugin()
        {
        }

        private void MouseLeftButtonDown()
        {
            Debug.WriteLine("触发了左键按下事件");
            Mouse.Capture(Application.Current.MainWindow);


            // if callback is not null, close main menu
            WeakReferenceMessenger.Default.Send(new MainMenuHideMessage());
        }

        private void MouseLeftButtonUp()
        {
            Mouse.Capture(null);
            Debug.WriteLine("触发了左键松开事件");
        }

        private RelayCommand? mouseLeftButtonDownCommand = null;
        private RelayCommand? mouseLeftButtonUpCommand = null;
    }
}
