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

namespace Vision_NBB.Views.UserPages
{
    /// <summary>
    /// MessageShow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageShow : Window
    {
        bool flag = false;
        string message;
        public MessageShow()
        {
            InitializeComponent();
        }


        public MessageShow(string message)
        {
            InitializeComponent();
            this.message = message;
            this.lbl_message.Text = message;
        }
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
