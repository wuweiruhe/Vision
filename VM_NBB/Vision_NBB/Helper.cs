using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;


namespace Vision_NBB
{
    public class Helper
    {
        //从Handle中获取Window对象
        static Window GetWindowFromHwnd(IntPtr hwnd)
        {
            return (Window)HwndSource.FromHwnd(hwnd).RootVisual;
        }

        //GetForegroundWindow API
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        //调用GetForegroundWindow然后调用GetWindowFromHwnd
        static Window GetTopWindow()
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == null)
                return null;

            return GetWindowFromHwnd(hwnd);
        }

        //显示对话框并自动设置Owner
        public static bool? ShowDialog(Window win)
        {
            win.Owner = GetTopWindow();
            win.ShowInTaskbar = false;
            return win.ShowDialog();
        }

        public static List<string> GetCheckedTags(DependencyObject obj)
        {
            var list = new List<string>();
            int childCount = VisualTreeHelper.GetChildrenCount(obj);

            for (int i = 0; i < childCount; i++)
            {
                var item = VisualTreeHelper.GetChild(obj, i);
                if (item == null) continue;
                if (item.GetType() == typeof(CheckBox))
                {
                    var ckb = item as CheckBox;
                    if (ckb != null && ckb.IsChecked == true && ckb.Tag != null)
                        list.Add(ckb.Tag.ToString());
                }
                else
                {
                    var _list = GetCheckedTags(item);
                    list.AddRange(_list);
                }
            }
            return list;
        }
    }
}
