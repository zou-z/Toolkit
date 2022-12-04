using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolkit.Contract;
using TopMost.ViewModel;

namespace TopMost
{
    public class TopMost : IPlugin
    {
        public string GetName()
        {
            return "窗口置顶";
        }

        public object? GetIcon()
        {
            return null;
        }

        public object? GetToolTip()
        {
            return "按住鼠标拖拽到窗口，设置/取消窗口置顶";
        }

        public bool IsCloseMenuEarly()
        {
            return true;
        }

        public void MouseLeftDown()
        {
            areaIndicationViewModel ??= new AreaIndicationViewModel();
            areaIndicationViewModel.Start();
        }

        public void MouseLeftUp()
        {
        }

        private AreaIndicationViewModel? areaIndicationViewModel = null;
    }
}
