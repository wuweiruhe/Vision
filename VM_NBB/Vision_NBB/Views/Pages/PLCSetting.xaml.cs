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
    /// SolutionSetting.xaml 的交互逻辑
    /// </summary>
    public partial class PLCSetting : Window
    {

        private Configuration config;

        public PLCSetting()
        {
            InitializeComponent();

            config = CurrentInfo.Config;
            txb_IpPlc.Text = config.PLC_IP;
            txb_PortPlc.Text = config.PLC_Port;
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrentInfo.Config.PLC_IP = txb_IpPlc.Text;
                CurrentInfo.Config.PLC_Port = txb_PortPlc.Text;

                new MessageShow("当前方案已成功保存，请重启软件后生效！！！").ShowDialog();
  
            }
            catch(Exception ex)
            {
                //日志提示配置保存出现异常
                UILogMangerHelper.Instance.AddLog(LogLevel.Debug, "PLC和路径方案保存出现异常");
                GetLogHelper.VisionLog.Error(ex);
            }
        }
    }
}
