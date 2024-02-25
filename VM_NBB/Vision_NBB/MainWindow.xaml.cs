using Microsoft.Win32;
using PLCDevice.OMRON_CIP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using TranslationCalibModuCs;
using Vision_NBB.Controls;
using Vision_NBB.Log;
using Vision_NBB.Model;
using Vision_NBB.Model.DB;
using Vision_NBB.Toolkit;
using Vision_NBB.Utility;
using Vision_NBB.Views.CalibPages;
using Vision_NBB.Views.Pages;
using Vision_NBB.Views.UserPages;
using VM.Core;
using VM.PlatformSDKCS;

namespace Vision_NBB
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region  定义变量

        public int ProgressBarVal;

        public OMRON_NJ PLC;
       
        public List<Model_UserConfog> UserList = new List<Model_UserConfog>();
        public string Level = "";
        public Configuration configuration;

        public long dwLast = 0, dwCurrent = 0;


        public static object Camera1_RunLock = new object();
        public static object Camera3_RunLock = new object();
        public static object Camera4_RunLock = new object();

        object HeartRobot = 0;
        #endregion



        public MainWindow()
        {
            Thread trd = new Thread(new ThreadStart(start_splash));
            trd.SetApartmentState(ApartmentState.STA);
            trd.Start();
            ProgressBarVal = 5;


            try
            {


                LoadExternalProcess();

                //设置最大显示个数，超过则覆盖  
                UILogMangerHelper.Instance.MaxCount = 1000;

                #region loadConfig

                if (Configuration.Load(out configuration))
                {
                   GetLogHelper.VisionLog.Error("加载配置成功"); //本地VisionLog保存
                  


                    UILogMangerHelper.Instance.AddLog(LogLevel.Info, "加载配置成功");
                }
                else
                {
                    configuration = new Configuration();

                    GetLogHelper.VisionLog.Error("初始加载配置文件"); //本地VisionLog保存
                    UILogMangerHelper.Instance.AddLog(LogLevel.Info, "初始加载配置文件");
                }

                CurrentInfo.Config = configuration;

             

                #endregion

                #region 初始化PLC
                PLC = new OMRON_NJ();

                if (PLC == null)
                {
                    UILogMangerHelper.Instance.AddLog(LogLevel.Debug, "PLC组件创建失败"); //界面显示
                    GetLogHelper.VisionLog.Error("PLC组件创建失败"); //本地VisionLog保存
                }

                PLC.Connection(configuration.PLC_IP, Convert.ToInt32(configuration.PLC_Port));

                #endregion

                ProgressBarVal = 25;

                InitializeComponent();

                PlcReconnect();

            }
            catch (Exception ex)
            {
                UILogMangerHelper.Instance.AddLog(LogLevel.Error, "初始化异常");
                GetLogHelper.VisionLog.Error("程序初始化异常:  " + ex.ToString());
           
                
            }
        }




        private void LoadExternalProcess()
        {

            var path = AppDomain.CurrentDomain.BaseDirectory + "UpdateProcess.exe";
            var msg= Toolkits.StartExternalProcess(path, "");
            GetLogHelper.VisionLog.Info(msg);

        }





        /// <summary>
        /// 初始加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
              
                //UserListInit();
                InitVm();
                SetUiTreeView();
                JudgeImagePathExist();

               

                VmHelper.LoadVmCalibFiles();
 
            }
            catch (Exception ex)
            {
                GetLogHelper.VisionLog.Error(ex);             
            }
            finally
            {
                ProgressBarVal = 100;
            }
        }

     


        /// <summary>
        /// 设置每界面UI Treeview 视图
        /// </summary>
        public void SetUiTreeView()
        {
            try
            {

                VmProcedure procedure1 = VmSolution.Instance[VMTagName.机械手定位抓取.机械手定位抓取流程名] as VmProcedure;
                var moduleInfo1 = procedure1.GetAllModuleList().astModuleInfo;


                ///////////////////////////////////////////////////////////////
                VmProcedure procedure3 = VmSolution.Instance[VMTagName.上点胶相机.上点胶相机流程名] as VmProcedure;
                var moduleInfo3 = procedure3.GetAllModuleList().astModuleInfo;
              


                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
                VmProcedure procedure4 = VmSolution.Instance[VMTagName.背点胶相机.背点胶相机流程名] as VmProcedure;
                var moduleInfo4 = procedure4.GetAllModuleList().astModuleInfo;

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////

                var WorkFlowSort = configuration.workFlowSorts;
                this.u1.ShowTreeView(moduleInfo1, WorkFlowSort, PLC, procedure1.FullName);
                //this.u2.ShowTreeView(moduleInfo2, WorkFlowSort, PLC, fullName2);
                this.u3.ShowTreeView(moduleInfo3, WorkFlowSort, PLC, procedure3.FullName);
                this.u4.ShowTreeView(moduleInfo4, WorkFlowSort, PLC, procedure4.FullName);
            }
            catch (Exception ex)
            {
                UILogMangerHelper.Instance.AddLog(LogLevel.Error, "VM流程加载出现异常！"); //界面显示
                GetLogHelper.VisionLog.Error("VM流程加载出现异常: " + ex.ToString());
            }
        }


        /// <summary>
        /// 初始加载VM 以及设置
        /// </summary>
        public void InitVm()
        {
            try
            {
                ProgressBarVal = 50;
                string path = configuration.SoluctionPath;
   
                this.lbl_Version.Content= $"Version {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}";

                if (!FileHelper.isExistFile(path))
                {
                    UILogMangerHelper.Instance.AddLog(LogLevel.Error, "方案路径无法找到");

                    GetLogHelper.VisionLog.Info("方案路径无法找到");
                    path = "方案路径无法找到";
                    
                }
                else
                {
                   VmHelper.LoadVmSolution(path);
                }


                SetVmCallBack();   //  VM 回调设置

                ProgressBarVal = 80;


                current_soluction.Content = path;
                loginUser.Text = configuration.LoginUserName;

             

                VmGlobalDataModel.Instance().GetGloabalVars<VmGlobalDataModel>(); // 获得VM全局变量




            }
            catch (Exception ex)
            {
                #region 加密狗异常
                VM.PlatformSDKCS.VmException vmEx = VM.Core.VmSolution.GetVmException(ex);

                if ((null != vmEx) || (vmEx.errorMessage.Contains("Dongle not detected")))
                {
                    GetLogHelper.VisionLog.Info("加密狗异常");
                    this.DongleMessage.Content = "加密狗异常";


                }
               
                #endregion
               GetLogHelper.VisionLog.Error(ex.ToString());
            }
        }



        private void JudgeImagePathExist()
        {

            Task.Run(() =>
            {

                Thread.Sleep(2500);
                var path = System.IO.Path.Combine(CurrentInfo.Config.cameraSettings.ImagePath, CurrentInfo.CameraDir.Camera1_FileDirName);



                if (!Directory.Exists(path))
                {
                    this.Dispatcher.Invoke(() => { new MessageShow("保存图片路径不存在，请重新设置图片路径").ShowDialog(); });


                };




            });

        }

        /// <summary>
        /// 对应四个流程回调 自动执行
        /// </summary>
        public void SetVmCallBack()
        {
            try
            {
                ProcessInfoList processInfoList = VmSolution.Instance.GetAllProcedureList();//获取所有流程


                VmProcedure procedure1 = VmSolution.Instance[VMTagName.机械手定位抓取.机械手定位抓取流程名] as VmProcedure;
                VmProcedure procedure3 = VmSolution.Instance[VMTagName.上点胶相机.上点胶相机流程名] as VmProcedure;

                VmProcedure procedure4 = VmSolution.Instance[VMTagName.背点胶相机.背点胶相机流程名] as VmProcedure;


                if (procedure1 == null)
                {
                    //流程为空
                   GetLogHelper.VisionLog.Info("流程1 获取异常 ");
                }
                else
                {
                    procedure1.OnWorkEndStatusCallBack += Procedure1_OnWorkEndStatusCallBack;


                    procedure1.OnWorkBeginStatusCallBack += (s, e) =>{

                        try
                        {   

                            lock(Camera1_RunLock)
                            {
                                CameraState.Camera1_IsRunning = true;
                                PLC.WriteValue(PLCTagName.Camera1_Status, PLCTagName.Busy);
                            }
                          
                        }
                        catch (Exception ex)
                        {

                            GetLogHelper.VisionLog.Error("写相机 1  状态异常");
                        }
                      
                    
                    
                    };
                }
                //if (procedure2 == null)
                //{
                //   GetLogHelper.VisionLog.Info("流程2 获取异常 ");
                //}
                //else
                //{
                //    procedure2.OnWorkEndStatusCallBack += Procedure2_OnWorkEndStatusCallBack;
                //}
                if (procedure3 == null)
                {
                   GetLogHelper.VisionLog.Info("流程3 获取异常 ");
                }
                else
                {
                    procedure3.OnWorkEndStatusCallBack += Procedure3_OnWorkEndStatusCallBack;

                    procedure3.OnWorkBeginStatusCallBack += (s, e) => {


                        try
                        {

                            lock (Camera3_RunLock)
                            {
                                CameraState.Camera3_IsRunning = true;

                                PLC.WriteValue(PLCTagName.Camera3_Status, PLCTagName.Busy);

                            }
                               
                        }
                        catch (Exception ex)
                        {
                            GetLogHelper.VisionLog.Error("写相机3  状态异常");
                           
                        }
                      
                    
                    
                    };
                }
                if (procedure4 == null)
                {
                   GetLogHelper.VisionLog.Info("流程4 获取异常 ");
                }
                else
                {
                    procedure4.OnWorkEndStatusCallBack += Procedure4_OnWorkEndStatusCallBack;

                    procedure4.OnWorkBeginStatusCallBack += (s, e) => {


                        try
                        {
                            lock (Camera4_RunLock)
                            {
                                CameraState.Camera4_IsRunning = true;
                                PLC.WriteValue(PLCTagName.Camera4_Status, PLCTagName.Busy);


                            }
                                
                        }
                        catch (Exception ex)
                        {
                            GetLogHelper.VisionLog.Error("写相机 4  状态异常");

                        }
                       
                    
                    
                    };
                }
            }
            catch (Exception ex)
            {
                UILogMangerHelper.Instance.AddLog(LogLevel.Debug, "获取所有流程列表出现异常！"); //界面显示
               GetLogHelper.VisionLog.Error("获取所有流程列表出现异常 " + ex.ToString());
            }
        }



        /// <summary>
        /// 初始化相机状态
        /// </summary>
        private void InitCameraState()
        {
            try
            {     
                if (PLC.IsConnected() == true)
                {
                    if (CameraState.Camera1_IsRunning == false)
                    {
                         
                        lock(Camera1_RunLock)
                        {
                            if(CameraState.Camera1_IsRunning == false)
                            PLC.WriteValue(PLCTagName.Camera1_Status, PLCTagName.Ready);
                        }

                    }


                    if (CameraState.Camera3_IsRunning == false)
                    {

                        lock (Camera3_RunLock)
                        {
                            if (CameraState.Camera3_IsRunning == false)
                                PLC.WriteValue(PLCTagName.Camera3_Status, PLCTagName.Ready);
                        }

                    }



                    if (CameraState.Camera4_IsRunning == false)
                    {

                        lock (Camera4_RunLock)
                        {
                            if (CameraState.Camera4_IsRunning == false)
                                PLC.WriteValue(PLCTagName.Camera4_Status, PLCTagName.Ready);
                        }

                    }


                    
                }
            }
            catch (Exception ex)
            {
                UILogMangerHelper.Instance.AddLog(LogLevel.Error, "相机状态初始化异常"+ex);

                GetLogHelper.VisionLog.Error("相机状态初始化异常");

            }
        }



        #region 各界面流程执行完回调

        private void Procedure4_OnWorkEndStatusCallBack(object sender, EventArgs e)
        {
            this.u4.ExecuteAsync();
        }

        private void Procedure3_OnWorkEndStatusCallBack(object sender, EventArgs e)
        {
            this.u3.ExecuteAsync();
        }

        private void Procedure2_OnWorkEndStatusCallBack(object sender, EventArgs e)
        {
            this.u2.ExecuteAsync();
        }

        private void Procedure1_OnWorkEndStatusCallBack(object sender, EventArgs e)
        {
            this.u1.ExecuteAsync();
        }

        #endregion



        #region 用户

        /// <summary>
        /// 用户实体类初始化加载
        /// </summary>
        public async void UserListInit()
        {
            try
            {
                UserList = await Sqlite_ConfigHelper.GetUserConfigValue();
            }
            catch (Exception ex)
            {
               GetLogHelper.VisionLog.Error(ex);
            }
        }


        #endregion




        /// <summary>
        /// 循环判断 PLC连接
        /// </summary>
        public void PlcReconnect()
        {
            Task.Run(() =>
            {
                Thread.Sleep(100);
                while (true)
                {
                    InitCameraState();
                    Thread.Sleep(1000);
                    try
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            if (PLC.IsConnected())
                            {
                                img_plcstatus.Source = new BitmapImage(new Uri(@"pack://application:,,,/Vision_NBB;component/image/filled_circle_GREEN_20px.png", UriKind.Absolute));
                                lbl_plcstatus.Content = "PLC已经建立连接";
                                HeartService();
                            }
                            else
                            {
                                img_plcstatus.Source = new BitmapImage(new Uri(@"pack://application:,,,/Vision_NBB;component/image/filled_circle_28px.png", UriKind.Absolute));
                                lbl_plcstatus.Content = "PLC连接失败";
                                // PLC.Connection(SolutionAndPLCModel.PLC_IP, Convert.ToInt32(SolutionAndPLCModel.PLC_Port));
                                PLC.Connection(configuration.PLC_IP, Convert.ToInt32(configuration.PLC_Port));

                               
                                if (PLC.IsConnected())
                                {

                                    


                                }
                            }
                        });



#if !TestDebug


                        ////PLC心跳   1
                        //PLC.WriteValue(PLCTagName.PLC_HeartBeat, 1);

                        //Thread.Sleep(1000);
                        //PLC.WriteValue(PLCTagName.PLC_HeartBeat, 0);
                        //PLC心跳   1
                        //PLC.WriteValue(PLCTagName.RobotToCcdHeart, 1);
#endif




                    }
                    catch (Exception ex)
                    {
                       GetLogHelper.VisionLog.Info("PLC循环连接出现异常:  " + ex.ToString());
                    }
                }
            });
        }



        /// <summary>
        /// 重启cip服务
        /// </summary>
        private void RestartCIP()
        {
            Task.Run(() =>
            {
                Thread.Sleep(500);
                RestartCipService();
            });
        }


        /// <summary>
        /// 重启CIP服务
        /// </summary>
        public void RestartCipService()
        {
            try
            {
                ServiceController service = new ServiceController("CIPCore");
                if (service.Status == ServiceControllerStatus.Running)
                {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);
                }
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running);

            }
            catch (Exception ex)
            {
               GetLogHelper.VisionLog.Error(ex);
            }
        }

        public void HeartService()
        {

            dwCurrent = System.Environment.TickCount;

            if ((dwCurrent - dwLast) > 1000)
            {
                if (Convert.ToInt32(HeartRobot) == 0)
                {
                    HeartRobot = 1;

                    ////System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    //label2.BeginInvoke(new Action(() =>
                    //{
                    //    label2.BackColor = Color.Green;
                    //}));

                }
                else
                {
                    HeartRobot = 0;

                    //label2.BeginInvoke(new Action(() =>
                    //{
                    //    label2.BackColor = Color.Red;
                    //}));
                }
                PLC.WriteValue(PLCTagName.PLC_HeartBeat, HeartRobot);
                dwLast = System.Environment.TickCount;
            }

        }


        #region 进度条相关方法

        private void start_splash()
        {

            var frm = new Splash();
            frm.getMessage += getProgressBarVal;
            frm.ShowDialog();

        }


        public int getProgressBarVal(bool flag)
        {
            return ProgressBarVal;
        }



#endregion


#region 按钮事件

        /// <summary>
        /// 打开VM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_openvM_Click(object sender, RoutedEventArgs e)
        {
            UserListInit();
            //if (!Permission.EquipUserAuthority(configuration.LoginUserName, Permission.openVM, UserList)) return;

            this.Visibility = Visibility.Hidden;
            try
            {
                var vm_window = new Views.Pages.VM();
                vm_window.showMainWin = showMainWindow;
                vm_window.ShowDialog();

            }
            catch (Exception ex)
            {
               UILogMangerHelper.Instance.AddLog(LogLevel.Debug, "Open Vm 出现异常");
               GetLogHelper.VisionLog.Error(ex);
            }
        }


        /// <summary>
        /// 窗体是否可见
        /// </summary>
        /// <param name="flag"></param>
        public void showMainWindow(bool flag)
        {
            this.Visibility = Visibility.Visible;
        }


        /// <summary>
        /// 保存方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_saveSolutiojn_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    //    VmSolution.SaveAs(SolutionAndPLCModel.SoluctionPath, "");
                    VmSolution.SaveAs(configuration.SoluctionPath, "");
                    new MessageShow("保存成功").ShowDialog();

                }
                catch (Exception ex)
                {
                    new MessageShow("保存失败").ShowDialog();
                   GetLogHelper.VisionLog.Debug(ex);
                }
            }));
        }


        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_SveeAsSolutiojn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "VM Sol  (*.sol)|*.sol";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            try
            {
                if (saveFileDialog1.ShowDialog() == true)
                {
                    var fileName = saveFileDialog1.FileName;
                    VmSolution.SaveAs(fileName, "");
                    new MessageShow("保存成功").ShowDialog();
                }
            }
            catch (Exception ex)
            {
                new MessageShow("保存失败").ShowDialog();
               GetLogHelper.VisionLog.Debug(ex);
            }
        }



        /// <summary>
        /// PLC设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_plcSet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //UserListInit();
                //if (!Permission.EquipUserAuthority(configuration.LoginUserName, Permission.plcSet, UserList)) return;
                PLCSetting s = new PLCSetting();
                s.ShowDialog();

            }
            catch (Exception ex)
            {
               GetLogHelper.VisionLog.Debug(ex);

            }
        }


        /// <summary>
        /// 相机设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CameraSetting_Click(object sender, RoutedEventArgs e)
        {
            var w = new CameraSetting();
            Thread.Sleep(200);
            w.ShowDialog();
        }



        /// <summary>
        /// 权限设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_PermissionSet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserListInit();
                if (!Permission.EquipUserAuthority(configuration.LoginUserName, Permission.authority, UserList)) return;
                Authority authority = new Authority();
                authority.ShowDialog();
                UserListInit();

            }
            catch (Exception ex)
            {
               GetLogHelper.VisionLog.Debug(ex);
            }
        }


        /// <summary>
        /// 用户管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_userSet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserListInit();
                if (!Permission.EquipUserAuthority(configuration.LoginUserName, Permission.userSet, UserList)) return;
                //权限判断  只有管理员才可以修改全部pwd
                if (configuration.LoginUserName.Equals("Admin"))
                {
                    EditPasswordAdmin frm = new EditPasswordAdmin();
                    frm.ShowDialog();
                }
                else
                {
                    EditPassword editPassword = new EditPassword();
                    editPassword.ShowDialog();
                }

            }
            catch (Exception ex)
            {
               GetLogHelper.VisionLog.Debug(ex);
            }
        }


        /// <summary>
        /// 机械手标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_cail_Click(object sender, RoutedEventArgs e)
        {
            NCalibration nCalibration = new NCalibration(PLC);
            nCalibration.ShowDialog();
            //Calibration calibration = new Calibration();
            //calibration.ShowDialog();

        }


        /// <summary>
        /// 流程分组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_group_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserListInit();
                if (!Permission.EquipUserAuthority(configuration.LoginUserName, Permission.groupsettings, UserList)) return;
                CameraGroup cameraGroup = new CameraGroup();
                cameraGroup.ShowDialog();

            }
            catch (Exception ex)
            {
               GetLogHelper.VisionLog.Debug(ex);
            }
        }


        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_log_Click(object sender, RoutedEventArgs e)
        {
            LogWindow log = new LogWindow();
            log.ShowDialog();
        }



        /// <summary>
        /// 帮助
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_help_Click(object sender, RoutedEventArgs e)
        {
            Helps help = new Helps();
            help.ShowDialog();
        }


        /// <summary>
        /// 数据查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataView_Click(object sender, RoutedEventArgs e)
        {
            DataViewWindow dataViewWindow = new DataViewWindow();
            dataViewWindow.ShowDialog();
        }

        /// <summary>
        /// 主页面直接切换方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_UpdateSoluction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserListInit();
                //if (!Permission.EquipUserAuthority(configuration.LoginUserName, Permission.soluctionSet, UserList)) return;
                new UpdateSoluction().ShowDialog();

            }
            catch (Exception ex)
            {
               GetLogHelper.VisionLog.Debug(ex);

            }


        }
#endregion


#region 窗体操作 （可移动 最小化 关闭 用户登录等）

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Configuration.Save(configuration);

                MessageBoxResult result = MessageBox.Show("请确认是否退出?", "提示", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No) return;

                var closeLoading = new Loading("视觉软件正在关闭中 请耐心等待......");

                try
                {
                    Task.Run(() =>
                    {


                        if (!string.IsNullOrEmpty(VmSolution.Instance.ModuleFilePath))
                        {
                            VmSolution.Instance?.Dispose();

                        }

                        System.Environment.Exit(0);







                    });
                }
                catch (Exception ex)
                {

                    GetLogHelper.VisionLog.Debug(ex.ToString());

                }

                closeLoading.ShowDialog();
                this.Dispatcher.Invoke(() => this.Close());

            }
            catch (Exception ex)
            {
                GetLogHelper.VisionLog.Debug(ex.ToString());

            }

        }





        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 用户切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Login login = new Login();
                login.ShowDialog();
                this.Dispatcher.Invoke(() =>
                {
                    //       loginUser.Text = UIGlobal.LoginUserName;
                    loginUser.Text = configuration.LoginUserName;
                });
            }
            catch (Exception ex)
            {
               GetLogHelper.VisionLog.Debug(ex);
            }
        }




        /// <summary>
        /// 左键可移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

#endregion


        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
           
        }

     


        private void Camera2GlassCailb_Click_1(object sender, RoutedEventArgs e)
        {
              new Views.CalibPages.GlassBoardCalib().ShowDialog();
        }

        private void UpGlueDistortionCalib_Click(object sender, RoutedEventArgs e)
        { 
             new UpGlueDistortionCalib().ShowDialog();




        }

        private void DownGlueDistortionCalib_Click(object sender, RoutedEventArgs e)
        {   
            new DownGlueDistortionCalib().ShowDialog();

        }

        private void Menu_UpGlueGlassBoardCalib_Click(object sender, RoutedEventArgs e)
        {
            new UpGlueGlassBoardCalib() .ShowDialog();
        }

        private void Menu_DownGlueGlassBoardCalib_Click(object sender, RoutedEventArgs e)
        {
            new DownGlueGlassBoardCalib() .ShowDialog();    
        }
    }
}
