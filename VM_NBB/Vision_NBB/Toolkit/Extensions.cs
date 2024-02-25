using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Vision_NBB.Toolkit
{
    public static class Extensions
    {

        public static async Task TimeoutAfter(this Task task, TimeSpan timeout)
        {
            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {
                var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    await task;  // Very important in order to propagate exceptions
                }
                else
                {
                    throw new TimeoutException("The operation has timed out.");
                }
            }
        }



        public static void ExpandAll(this System.Windows.Controls.TreeView treeView)
        {
            ExpandAllItems(treeView);
        }

        private static void ExpandAllItems(ItemsControl control)
        {
            if (control == null)
            {
                return;
            }

            foreach (Object item in control.Items)
            {
                System.Windows.Controls.TreeViewItem treeItem = control.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (treeItem == null || !treeItem.HasItems)
                {
                    continue;
                }

                treeItem.IsExpanded = true;
                treeItem.UpdateLayout();
                ExpandAllItems(treeItem as ItemsControl);
            }
        }


        public static string DateNowFormat(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy_MM_dd_HH mm ss");
        }

        public static string CaliFormat(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd_HH-mm-ss");
        }

        /// <summary>
        /// 转换3位有效数子
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static float ToF3(this float Data)
        {

            return (float)Math.Round(Data, 3);

        }




        public static string ArrayTostring(this float[] data,string split="   ")
        {
            return string.Join(split, data);
        }



        public static float[] ArrayToF2(this float[] data)
        {
            float[] roundedNumbers = new float[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                roundedNumbers[i] = (float)Math.Round(data[i], 2);
            }
            return roundedNumbers;
        }


        public static float[] ArrayToF3(this float[] data)
        {
            float[] roundedNumbers = new float[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                roundedNumbers[i] = (float)Math.Round(data[i], 3);
            }
            return roundedNumbers;
        }

        public static string ArrayTostring(this float data, string split = " ")
        {
            return string.Join(split, data);
        }
    }
}
