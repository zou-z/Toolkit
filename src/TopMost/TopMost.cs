using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
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
            Path? path = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                path = new Path
                {
                    Data = Geometry.Parse("M320.32 704.18c14.058 14.06 14.058 36.854 0 50.913L125.864 949.547c-14.06 14.059-36.853 14.059-50.912 0-14.059-14.059-14.059-36.853 0-50.912l194.454-194.454c14.06-14.059 36.853-14.059 50.912 0z m629.906-396.904c14.818 14.818 13.897 39.112-2 52.766L717.082 558.556l0.667 2.734c27.425 114.91-4.257 237.542-87.885 325.02l-2.745 2.84-2.725 2.759c-14.036 14.195-36.941 14.26-51.057 0.144L126.447 445.162c-14.06-14.059-14.06-36.853 0-50.912 90.247-90.248 220.23-123.274 340.164-91.125l2.991 0.82 195.22-227.306c13.517-15.74 37.463-16.8 52.322-2.445z m-78.374 23.45L694.138 153.011 508.752 368.87a36 36 0 0 1-38.937 10.616l-3.106-1.086c-88.26-30.386-185.781-14.902-260.063 41.255l-2.2 1.682 392.717 392.718 0.874-1.132c54.977-71.991 71.575-166.167 44.804-252.737l-1.07-3.393a36 36 0 0 1 10.96-37.877l219.12-188.19z"),
                    Fill = Brushes.RoyalBlue,
                    Stretch = Stretch.Uniform,
                };
            });
            return path;
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
