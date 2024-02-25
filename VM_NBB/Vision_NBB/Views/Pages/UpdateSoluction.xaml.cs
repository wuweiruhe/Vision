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
using Vision_NBB.Model;
using Vision_NBB.Utility;
using Vision_NBB.Views.UserPages;

namespace Vision_NBB.Views.Pages
{
    /// <summary>
    /// UpdateSoluction.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateSoluction : Window
    {
 
        private Configuration config;

        public UpdateSoluction()
        {
            InitializeComponent();
            config = CurrentInfo.Config;
            solutonPath.Folder = config.SoluctionPath;

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrentInfo.Config.SoluctionPath = solutonPath.Folder;
                new MessageShow("当前方案已成功保存，请重启软件后生效！").ShowDialog();
            }
            catch
            {
                //日志提示配置保存出现异常
                UILogMangerHelper.Instance.AddLog(LogLevel.Debug, "PLC保存出现异常");
            }
        }

        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
