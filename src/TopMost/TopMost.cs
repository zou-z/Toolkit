using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolkit.Contract;

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
            return null;
        }

        public bool IsCloseMenuEarly()
        {
            return true;
        }

        public void MouseLeftDown()
        {
            System.Windows.MessageBox.Show("鼠标左键点击");
        }

        public void MouseLeftUp()
        {
        }
    }
}
