using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolkit.Model;

namespace Toolkit.ViewModel
{
    internal class MainMenuViewModel
    {
        public ObservableCollection<Plugin> Plugins => plugins ??= new ObservableCollection<Plugin>();

        public MainMenuViewModel()
        {
            for (int i = 0; i < 10; ++i)
            {
                string name = $"Menu Item {i}";
                Plugins.Add(new Plugin()
                {
                    Name = name + "," + name.Substring(0, i),
                });
            }

        }


        private ObservableCollection<Plugin>? plugins = null;
    }
}
