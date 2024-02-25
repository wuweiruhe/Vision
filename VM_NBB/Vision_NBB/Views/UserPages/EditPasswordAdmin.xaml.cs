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
    /// EditPasswordAdmin.xaml 的交互逻辑
    /// </summary>
    public partial class EditPasswordAdmin : Window
    {
        public List<Model_UserConfog> ResultList = new List<Model_UserConfog>();
        string CurrentLevel = "";

        public EditPasswordAdmin()
        {
            InitializeComponent();
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
        /// 确认修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = cmb.Text;

                //       level = UserList.Where(i => i.UserName == cmb.Text).ToList()[0].Level;

                foreach (var item in ResultList)
                {
                    if (item.UserName.Equals(name))
                    {
                        CurrentLevel = item.Level;
                    }
                }

                if (txb_old.Password != "")
                {
                    await Sqlite_ConfigHelper.SetUserConfigValue(
                                          new Model_UserConfog()
                                          {
                                              UserName = name,
                                              PassWord = txb_old.Password,          
                                              Level= CurrentLevel
                                          });
                    new MessageShow("密码修改成功").ShowDialog();
                    this.Close();
                }
                else
                {
                    new MessageShow("密码不可为空").ShowDialog();
                }
            }
            catch(Exception ex)
            {
                new MessageShow("出现异常，密码修改失败").ShowDialog();
               GetLogHelper.VisionLog.Debug(ex);
            }
        }


        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ResultList = await Sqlite_ConfigHelper.GetUserConfigValue();
                foreach (var item in ResultList)
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
    }
}


