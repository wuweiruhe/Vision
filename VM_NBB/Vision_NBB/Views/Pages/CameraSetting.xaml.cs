using System;
using System.Collections.Generic;
using System.IO;
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
using Vision_NBB.Toolkit;
using Vision_NBB.Utility;
using Vision_NBB.Views.UserPages;

namespace Vision_NBB.Views.Pages
{
    /// <summary>
    /// CameraSetting.xaml 的交互逻辑
    /// </summary>
    public partial class CameraSetting : Window
    {
     
        public CameraSetting()
        {
            InitializeComponent();

           
        }



       

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                VmGlobalDataModel.Instance().GetGloabalVars<VmGlobalDataModel>();
                //VmHelper.GetCamerasTriggerMode();  //  获得全局相机触发模式
            }
            catch (Exception ex)
            {
                GetLogHelper.VisionLog.Error(ex);

            }
         
            this.DataContext = new
            {
                vmGlobalDataModel = VmGlobalDataModel.Instance(),
                cameraSettings = CurrentInfo.Config.cameraSettings
            };

           

        }


        /// <summary>
        /// 路径保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              

             
                var path = System.IO.Path.Combine(CurrentInfo.Config.cameraSettings.ImagePath, CurrentInfo.CameraDir.Camera1_FileDirName);

              
                if (!Directory.Exists(path))
                {
                    var basePath = CurrentInfo.Config.cameraSettings.ImagePath;
                    Directory.CreateDirectory(System.IO.Path.Combine(basePath, CurrentInfo.CameraDir.Camera1_FileDirName));
                    Directory.CreateDirectory(System.IO.Path.Combine(basePath, CurrentInfo.CameraDir.Camera2_FileDirName));
                    Directory.CreateDirectory(System.IO.Path.Combine(basePath, CurrentInfo.CameraDir.Camera3_FileDirName));
                    Directory.CreateDirectory(System.IO.Path.Combine(basePath, CurrentInfo.CameraDir.Camera4_FileDirName));
                }
                VmGlobalDataModel.Instance().SetGlobalVars();
                new MessageShow("保存成功").ShowDialog();
            }
            catch (Exception ex)
            {

                if(ex.Message== "获得Vm全局变量出错")
                {
                    new MessageShow("获得Vm全局变量出错！").ShowDialog();

                    return;

                }
                new MessageShow("路径保存出现异常！").ShowDialog();
                GetLogHelper.VisionLog.Error(ex);
             

               
            }
        }



        
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }


        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rdo_Auto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              
                VmHelper.SetCameras_Line0Trigger();
            }
            catch (Exception EX)
            {
                MessageBox.Show("无法设置触发模式");
               
            }
        }

        private void Rdo_Manual_Click(object sender, RoutedEventArgs e)
        {

            try
            {
          
                VmHelper.SetCameras_SoftwareTrigger();
            }
            catch (Exception)
            {
                MessageBox.Show("无法设置触发模式");
               
            }
           
        }

        private void Rdo_Auto_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_SetRobotCalibParams_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string name = VMTagName.机械手定位抓取.单点抓取;
                VmHelper.OpenVmControlWindow(name).ShowDialog(); ;
            }
            catch (Exception ex)
            {

                MessageBox.Show("打开失败");



            }


        }
    }



    public class CameraView
    {
        public CameraSettings cameraSettings { get; set; }
        public VmGlobalDataModel vmGlobalDataModel { get; set; }
        public CameraView()
        {
        }
    }
}
