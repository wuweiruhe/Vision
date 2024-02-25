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
    /// EditPassword.xaml 的交互逻辑  
    /// </summary>
    public partial class EditPassword : Window
    {
        public List<Model_UserConfog> ResultList = new List<Model_UserConfog>();
        string CurrentLevel = "";
        public EditPassword()
        {
            InitializeComponent(); 
            lbl_user.Content = CurrentInfo.Config.LoginUserName;
       
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //初始密码是否正确（去掉）---
                //新密码和确认密码是否 一致
                if ((txb_new.Password!="")&& (txb_comfirm.Password != ""))
                {
                    if (txb_new.Password.Equals(txb_comfirm.Password))
                    {
                        foreach (var item in ResultList)
                        {
                            if (item.UserName.Equals(lbl_user.Content))
                            {
                                CurrentLevel = item.Level;
                            }
                        }


                        await Sqlite_ConfigHelper.SetUserConfigValue(
                                              new Model_UserConfog()
                                              {
                                                  UserName = lbl_user.Content.ToString(),
                                                  PassWord = txb_new.Password,
                                                  Level=CurrentLevel
                                              });
                        new MessageShow("密码修改成功").ShowDialog();
                        this.Close();

                    }
                    else
                    {
                        new MessageShow("请确认两次输入密码一致！").ShowDialog();
                    }
                }
                else
                {
                    new MessageShow("请分别输入密码和确认密码！").ShowDialog();
                }
               
            }
            catch(Exception ex)
            {
               GetLogHelper.VisionLog.Debug(ex);
                new MessageShow("出现异常，密码修改失败！").ShowDialog();
            }
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

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ResultList = await Sqlite_ConfigHelper.GetUserConfigValue();
        }
    }
}
