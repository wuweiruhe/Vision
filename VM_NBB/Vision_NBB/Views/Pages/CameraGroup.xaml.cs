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
using Vision_NBB.Model;
using Vision_NBB.Utility;
using Vision_NBB.Views.UserPages;

namespace Vision_NBB.Views.Pages
{
    /// <summary>
    /// CameraGroup.xaml 的交互逻辑
    /// </summary>
    public partial class CameraGroup : Window
    {
        private WorkFlowSorts sortList = new WorkFlowSorts();
        
        public CameraGroup()
        {
            InitializeComponent();
            this.sortList = CurrentInfo.Config.workFlowSorts;
            camera1_NO.Text = sortList.camera1_NO;
            camera1_Sequence.Text = sortList.camera1_Sequence;

            camera2_NO.Text = sortList.camera2_NO;
            camera2_Sequence.Text = sortList.camera2_Sequence;

            camera3_NO.Text = sortList.camera3_NO;
            camera3_Sequence.Text = sortList.camera3_Sequence;

            camera4_NO.Text = sortList.camera4_NO;
            camera4_Sequence.Text = sortList.camera4_Sequence;
         

        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 配置保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {           
            CurrentInfo.Config.workFlowSorts.camera1_NO = camera1_NO.Text;
            CurrentInfo.Config.workFlowSorts.camera1_Sequence = camera1_Sequence.Text;

            CurrentInfo.Config.workFlowSorts.camera2_NO = camera2_NO.Text;
            CurrentInfo.Config.workFlowSorts.camera2_Sequence = camera2_Sequence.Text;

            CurrentInfo.Config.workFlowSorts.camera3_NO = camera3_NO.Text;
            CurrentInfo.Config.workFlowSorts.camera3_Sequence = camera3_Sequence.Text;

            CurrentInfo.Config.workFlowSorts.camera4_NO = camera4_NO.Text;
            CurrentInfo.Config.workFlowSorts.camera4_Sequence = camera4_Sequence.Text;

            new MessageShow("当前配置已成功保存，请重启软件后生效！").ShowDialog();

        }
    }
}
