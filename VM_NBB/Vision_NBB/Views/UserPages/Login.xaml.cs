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
using Vision_NBB.Model.DB;
using Vision_NBB.Utility;
using Vision_NBB.Views.Pages;

namespace Vision_NBB.Views.UserPages
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
 
        public List<Model_UserConfog> UserModel = new List<Model_UserConfog>();
        public Login()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_login_out(object sender, RoutedEventArgs e)
        {
            new MessageShow("当前用户已成功登出").ShowDialog();
            CurrentInfo.Config.LoginUserName = "未登录";
            this.Close();
        }



        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_login_Click(object sender, RoutedEventArgs e)
        {
            string currentName = cmb.Text;
            string currentPwd = txb_old.Password;
            if (LoginContrast(currentName, currentPwd))
            {
                //登录成功
                new MessageShow("登录成功").ShowDialog();
                CurrentInfo.Config.LoginUserName = currentName;
                this.Close();
            }
            else
            {
                //登录失败
                new MessageShow("登录失败，请检查密码是否正确后重新输入").ShowDialog();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                UserModel = await Sqlite_ConfigHelper.GetUserConfigValue();
                foreach (var item in UserModel)
                {
                    cmb.Items.Add(item.UserName);
                }

                if ((CurrentInfo.Config.LoginUserName == null) || (CurrentInfo.Config.LoginUserName == "未登录"))
                {
                    cmb.SelectedIndex = 0;
                }
                else
                {
                    cmb.Text = CurrentInfo.Config.LoginUserName;
                }

            }
            catch (Exception ex)
            {
               GetLogHelper.VisionLog.Debug(ex);
            }
        }

        private bool LoginContrast(string userName, string userPwd)
        {
            try
            {
                foreach (var item in UserModel)
                {
                    if ((userName == item.UserName) && (userPwd == item.PassWord))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
               GetLogHelper.VisionLog.Debug(ex);
            }
            return false;
        }
    }
}
