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
    /// Authority.xaml 的交互逻辑
    /// </summary>
    public partial class Authority : Window
    {
        public List<Model_UserConfog> UserList = new List<Model_UserConfog>();

        public string authority_str = "";
        public Authority()
        {
            InitializeComponent();
        }

        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private void lst_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }


        /// <summary>
        /// 保存权限设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = cmb.Text;

                if (groupsettings.IsChecked == true)
                {
                    authority_str += Permission.groupsettings;
                }
                if (authority.IsChecked == true)
                {
                    authority_str += Permission.authority;
                }
                if (soluctionSet.IsChecked == true)
                {
                    authority_str += Permission.soluctionSet;
                }
                if (userSet.IsChecked == true)
                {
                    authority_str += Permission.userSet;
                }
                if (openVM.IsChecked == true)
                {
                    authority_str += Permission.openVM;
                }
                if (plcSet.IsChecked == true)
                {
                    authority_str += Permission.plcSet;
                }

                await Sqlite_ConfigHelper.SetUserConfigValue(
                                          new Model_UserConfog()
                                          {
                                              UserName = name,
                                              PassWord = UserList.Where(i => i.UserName == name).ToList()[0].PassWord,
                                              Level = authority_str
                                          });
                new MessageShow("权限修改成功").ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
               GetLogHelper.VisionLog.Debug(ex);
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                UserList = await Sqlite_ConfigHelper.GetUserConfigValue();
                foreach (var item in UserList)
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

        /// <summary>
        /// 切换加载初始权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string level = "";

            foreach (var item in UserList)
            {
                if (item.UserName.Equals(cmb.SelectedItem))
                {
                    level = item.Level;
                    break;
                }
            }
            if (level != null)
            {
                groupsettings.IsChecked = false;
                authority.IsChecked = false;
                soluctionSet.IsChecked = false;
                userSet.IsChecked = false;
                openVM.IsChecked = false;
                plcSet.IsChecked = false;

                //if(Permission.CheckAuthority(level, Permission.executeOnce))
                //{
                //    executeOnce.IsChecked = true;
                //}

                //if (Permission.CheckAuthority(level, Permission.continuExecute))
                //{
                //    continuExecute.IsChecked = true;
                //}

                if (Permission.CheckAuthority(level, Permission.groupsettings))
                {
                    groupsettings.IsChecked = true;
                }

                if (Permission.CheckAuthority(level, Permission.authority))
                {
                    authority.IsChecked = true;
                }

                if (Permission.CheckAuthority(level, Permission.soluctionSet))
                {
                    soluctionSet.IsChecked = true;
                }

                if (Permission.CheckAuthority(level, Permission.userSet))
                {
                    userSet.IsChecked = true;
                }

                if (Permission.CheckAuthority(level, Permission.openVM))
                {
                    openVM.IsChecked = true;
                }

                if (Permission.CheckAuthority(level, Permission.plcSet))
                {
                    plcSet.IsChecked = true;
                }
            }
        }



        private void lst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }



    public class Permission
    {
        public static string executeOnce = "1";
        public static string continuExecute = "2";
        public static string groupsettings = "3";
        public static string authority = "4";
        public static string soluctionSet = "5";
        public static string userSet = "6";
        public static string openVM = "7";
        public static string plcSet = "8";

        public static bool CheckAuthority(string authorityLevel, string authorityId)
        {
            if (authorityLevel.Contains(authorityId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static  string Level = "";

        public static  bool EquipUserAuthority(string currentUser, string authorityId, List<Model_UserConfog> userList)
        {
            try
            {
                foreach (var item in userList)
                {
                    if (item.UserName.Equals(currentUser))
                    {
                        Level = item.Level;
                        if (Level.Contains(authorityId))
                        {
                            return true;
                        }
                        else
                        {
                            new MessageShow("您没有访问权限，请切换用户后重新操作").ShowDialog();
                            return false;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
               GetLogHelper.VisionLog.Debug(ex);
            }
            return false;
        }      
    }
}
