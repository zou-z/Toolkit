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
using Toolkit.Base.Log;
using Toolkit.Contract;
using Toolkit.Message;

namespace Toolkit.Model
{
    internal class Plugin
    {
        public string Name { get; set; }

        public RelayCommand MouseLeftButtonDownCommand => mouseLeftButtonDownCommand ??= new RelayCommand(MouseLeftButtonDown);

        public RelayCommand MouseLeftButtonUpCommand => mouseLeftButtonUpCommand ??= new RelayCommand(MouseLeftButtonUp);

        public Plugin(IPlugin plugin)
        {
            this.plugin = plugin;
            Name = this.plugin.GetName();
        }

        private void MouseLeftButtonDown()
        {
            Logger.Trace("Main Menu Mouse Left Button Down");
            if (plugin.IsCloseMenuEarly())
            {
                WeakReferenceMessenger.Default.Send(new MainMenuHideMessage());
            }
            plugin.MouseLeftDown();
        }

        private void MouseLeftButtonUp()
        {
            Logger.Trace("Main Menu Mouse Left Button Up");
            plugin.MouseLeftUp();
        }

        private RelayCommand? mouseLeftButtonDownCommand = null;
        private RelayCommand? mouseLeftButtonUpCommand = null;
        private readonly IPlugin plugin;
    }
}
