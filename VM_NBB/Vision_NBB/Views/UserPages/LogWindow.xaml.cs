using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Vision_NBB.Log;
using Vision_NBB.Utility;

namespace Vision_NBB.Views.UserPages
{
    /// <summary>
    /// Log.xaml 的交互逻辑
    /// </summary>
    public partial class LogWindow : Window
    {
        public LogWindow()
        {
            InitializeComponent();
            //设置最大显示个数，超过则覆盖  
            UILogMangerHelper.Instance.MaxCount = 1000;
            this.lsv_log.ItemsSource = UILogMangerHelper.Instance.LogsAll;
        }

        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
          
        }
    }
}
