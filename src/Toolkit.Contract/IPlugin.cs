using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkit.Contract
{
    public interface IPlugin
    {
        /// <summary>
        /// 获取插件的名称
        /// </summary>
        /// <returns>插件的名称</returns>
        string GetName();

        /// <summary>
        /// 获取插件的图标
        /// </summary>
        /// <returns>插件的图标</returns>
        object? GetIcon();

        /// <summary>
        /// 获取插件的提示
        /// </summary>
        /// <returns>插件的提示</returns>
        object? GetToolTip();

        /// <summary>
        /// 是否提前关闭菜单
        /// </summary>
        /// <returns>
        /// <para>true 鼠标左键按下时关闭菜单</para>
        /// <para>false 鼠标左键松开时关闭菜单</para>
        /// </returns>
        bool IsCloseMenuEarly();

        /// <summary>
        /// 鼠标左键按下时执行的动作
        /// </summary>
        void MouseLeftDown();

        /// <summary>
        /// 鼠标左键松开时执行的动作
        /// </summary>
        void MouseLeftUp();
    }
}
