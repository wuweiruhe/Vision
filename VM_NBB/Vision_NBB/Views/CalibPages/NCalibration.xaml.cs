using IMVSCaliperCornerModuCs;
using log4net;
using PLCDevice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TranslationCalibModuCs;
using Vision_NBB.Log;
using Vision_NBB.Model;
using Vision_NBB.Toolkit;
using Vision_NBB.Utility;
using Vision_NBB.Views.Pages;
using VM.Core;
using VMControls.WPF.Release;
using MessageBox = System.Windows.MessageBox;
using PLCTagName = Vision_NBB.Model.PLCTagName;

namespace Vision_NBB.Views.CalibPages
{
    /// <summary>
    /// 机械手标定
    /// </summary>
    public partial class NCalibration : Window
    {
        NPointCalibParam nPointCalibParam;
        public LogManager log;

        //取消基于Task建立的线程
        CancellationTokenSource source;

        AutoResetEvent _handle = new AutoResetEvent(false);

        List<double> L_pixel_x = new List<double>();
        List<double> L_pixel_y = new List<double>();

        List<double> L_robot_x = new List<double>();
        List<double> L_robot_y = new List<double>();
        List<double> L_robot_th = new List<double>();

        bool Isrun = false;
        double worldX;
        double worldY;
        double rotateAngle;

        double Point5_x;
        double Point5_y;
        double Point5_th;

        int moveCount;

        static int cts = 0;

        BasePLC PLC;

        static int _Counts = 0;

        static int Calibcts = 0;

        VmProcedure process;
        public NCalibration(BasePLC basePLC)
        {
            InitializeComponent();
            LoadCailFiles();


            log = new LogManager();

            this.lsv_log.ItemsSource = log.LogsAll;

            this.DataContext = CurrentInfo.Config.nPointCalibParam;
            nPointCalibParam = CurrentInfo.Config.nPointCalibParam;
            this.PLC = basePLC;

            //rdo_secend_cail.IsChecked = true;




        }

        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {

            if (source != null)
                source.Cancel();


            this.Close();


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                process = VmSolution.Instance["机械手标定1"] as VmProcedure;

                if(process == null)
                {
                    MessageBox.Show("加载标定流程失败", "", MessageBoxButton.OK, MessageBoxImage.Error);

                }

                //process.OnWorkEndStatusCallBack += Procedure1_OnWorkEndStatusCallBack;

                this.vm.ModuleSource = process;
            }
            catch (Exception ex)
            {
                GetLogHelper.VisionLog.Error(ex);
                
            }
           
        }


        /// <summary>
        /// Vm回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Procedure1_OnWorkEndStatusCallBack(object sender, EventArgs e)
        {
            //_handle.Set();
           
        }

        private void ExecuteOnce()
        {
            //VmProcedure process = VmSolution.Instance["机械手标定1"] as VmProcedure;
            VmProcedure process = VmSolution.Instance[VMTagName.机械手标定相机.机械手标定流程名] as VmProcedure;


            if (process != null) process.Run();

        }

        public void GetNextRobotPos(ref double worldX, ref double worldY, ref double rotateAngle, int count)
        {



        }

        /// <summary>
        /// 快速匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_match_Click(object sender, RoutedEventArgs e)
        {
            var paramForm = new ParamsConfigControl("快速匹配");

            //VmModule FindModule = (VmModule)VmSolution.Instance["机械手标定.快速匹配"];
            VmModule FindModule = (VmModule)VmSolution.Instance[VMTagName.机械手标定相机.机械手标定_快速匹配];

            if (null == FindModule) return;

            paramForm.config.ModuleSource = FindModule;
            paramForm.ShowDialog(); // 显示界面
        }


        /// <summary>
        /// 执行一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_execute_Click(object sender, RoutedEventArgs e)
        {
            
            VmGlobalDataModel.Instance().SetSingleGlobalVars("TrigCts", "0");
            Thread.Sleep(100);
            ExecuteOnce();
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_run_Click(object sender, RoutedEventArgs e)
        {
            UnEnable();

            ClearCaliStatus();

            this.log.AddLog(UILogLevel.Debug, "标定流程开始！");
            Isrun = true;

            //// 获取基准点X
            float baseX = nPointCalibParam.BasePointX;
            float baseY = nPointCalibParam.BasePointY;
            float BaseAngle = nPointCalibParam.BaseAngle;

            ////获取基准点XY偏移X
            float offset_x = nPointCalibParam.MoveAlignX;
            float offset_y = nPointCalibParam.MoveAlignY;
            float offset_th = nPointCalibParam.MoveAngle;

            SetVirable(baseX, baseY, BaseAngle, _Counts, offset_x, offset_y, offset_th);

            Thread.Sleep(100);

            if (_Counts == 999) { _Counts = 0; }

            //_Counts += 1;

            source = new CancellationTokenSource();

            var task = new Task(() =>
            {

                try
                {

                    object[] data8 = new object[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };   //标定结束

                    PLC.WriteValue(PLCTagName.Robot_calib_Senddata, data8);


                    do
                    {
                        //Object RevData = PLC.ReadValue(PLCTagName.Robot_calib_Readdata);  //获得触发变量

                        float []RevData = PLC.ReadValue<float[]>(PLCTagName.Robot_calib_Readdata);  //获得触发变量
                       
                        if (RevData[8] == 11)//触发标志
                        {

                            object[] data9 = new object[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //X ,Y ,R ,1/2,X0,Y0,R0,1,22,0（完成信号)

                            PLC.WriteValue(PLCTagName.Robot_calib_Readdata, data9);//清空触发变量

                            VmGlobalDataModel.Instance().SetSingleGlobalVars("TrigCts", _Counts.ToString());

                            Thread.Sleep(50);

                            _Counts++;

                            ExecuteOnce(); // 拍照一次

                            Thread.Sleep(1000);

                            if (!CallFun())
                            { this.log.AddLog(UILogLevel.Debug, "标定失败流程退出！"); break; }

                            Thread.Sleep(1000);
                        }

                        Thread.Sleep(100);
                    }
                    while (_Counts <= 12);

                    _Counts = 0;

                    Calibcts = 0;

                    Enable();

                    data8 = new object[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };   //标定结束

                    PLC.WriteValue(PLCTagName.Robot_calib_Senddata, data8);

                    this.log.AddLog(UILogLevel.Debug, "标定流程结束！");

                }
                catch (Exception ex)
                {
                    Isrun = false;
                    _Counts = 0;

                    Calibcts = 0;
                    this.log.AddLog(UILogLevel.Debug, ex.ToString());

                }
                Thread.Sleep(100);
            }, source.Token);

            task.Start();

        }


        private bool CallFun()
        {
            bool _bFlag = false;

            object[] data8 = new object[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };   //X ,Y ,R ,1/2,X0,Y0,R0,1,22,0

            //VmProcedure vmprocess = (VmProcedure)VmSolution.Instance["机械手标定1"];
            VmProcedure vmprocess = (VmProcedure)VmSolution.Instance[VMTagName.机械手标定相机.机械手标定流程名];

            var moduResult = vmprocess.ModuResult;

            try
            {
                var Result = moduResult.GetOutputInt("outResult").pIntVal[0];

                if (Result == 1)
                {
                    data8[0] = moduResult.GetOutputFloat("outX0").pFloatVal[0];
                    data8[1] = moduResult.GetOutputFloat("outY0").pFloatVal[0];
                    data8[2] = moduResult.GetOutputFloat("outR0").pFloatVal[0];
                    data8[3] = 1;//处理结果
                    data8[4] = moduResult.GetOutputFloat("outX1").pFloatVal[0];
                    data8[5] = moduResult.GetOutputFloat("outY1").pFloatVal[0];
                    data8[6] = moduResult.GetOutputFloat("outR1").pFloatVal[0];
                    data8[7] = Calibcts;
                    data8[8] = 22;
                    data8[9] = 0;
                    _bFlag = true;
                }
                else
                {

                    data8[3] = 2;//处理结果 1OK 2NG
                    Isrun = false;
                    _bFlag = false;
                }

                PLC.WriteValue(PLCTagName.Robot_calib_Senddata, data8);

                Thread.Sleep(100);

                PLC.WriteValue(PLCTagName.CamaraDoneRobotLable, 1);//处理完成
                Thread.Sleep(100);
                string msg = $"次数: {Calibcts}    取片x:{data8[0]}   取片y:{data8[1]}   取片r:{data8[2]}    result:{data8[3]}     放片x1:{data8[4]}    放片y1:{data8[5]}    放片r1:{data8[6]}";

                this.log.AddLog(UILogLevel.Debug, msg);
                Calibcts++;
            }
            catch (Exception EX)
            {
                this.log.AddLog(UILogLevel.Debug, EX.ToString());
                return false;

            }

            return _bFlag;
        }

        private void SetVirable(float baseX, float baseY, float BaseAngle, int count, float offset_x, float offset_y, float offset_th)
        {
            
            VmGlobalDataModel.Instance().BasePointX = baseX;
            VmGlobalDataModel.Instance().BasePointY = baseY;
            VmGlobalDataModel.Instance().BaseAngle = BaseAngle;
            VmGlobalDataModel.Instance().TrigCts = count;//次数

            VmGlobalDataModel.Instance().XCalibInterval = offset_x;
            VmGlobalDataModel.Instance().YCalibInterval = offset_y;
            VmGlobalDataModel.Instance().RCalibInterval = offset_th;

            VmGlobalDataModel.Instance().SetGlobalVars();

        }

        public void UnEnable()
        {


            this.btn_run.IsEnabled = false;


            L_pixel_x.Clear();
            L_pixel_y.Clear();

            L_robot_x.Clear();
            L_robot_y.Clear();
            L_robot_th.Clear();

            log.ClearLog();


        }

        public void Enable()
        {
            this.Dispatcher.Invoke(() => { this.btn_run.IsEnabled = true; });

        }

        private void Btn_StopRun_Click(object sender, RoutedEventArgs e)
        {
            if (source != null)
            { source.Cancel(); }

            _Counts = 999;

            Isrun = false;

            Thread.Sleep(100);

            Enable();

            ClearCaliStatus();

            this.log.AddLog(UILogLevel.Debug, "中断停止！");

        }

        /// <summary>
        /// 清空日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_clearLog_Click(object sender, RoutedEventArgs e)
        {
            this.log.ClearLog();
        }
        private void Point5_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// 图像源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cam_Click(object sender, RoutedEventArgs e)
        {
            //var paramForm = new ParamsConfigControl("图像源");

            //VmModule FindModule = (VmModule)VmSolution.Instance["机械手标定.图像源"];

            //if (null == FindModule) return;

            //paramForm.config.ModuleSource = FindModule;
            //paramForm.ShowDialog(); // 显示界面


            //VmHelper.OpenVmControlWindow("机械手标定.图像源")?.ShowDialog();
            VmHelper.OpenVmControlWindow(VMTagName.机械手标定相机.机械手标定_图像源)?.ShowDialog();
         
        }

        /// <summary>
        /// 平移旋转标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cail_Click(object sender, RoutedEventArgs e)
        {
            //var paramForm = new ParamsConfigControl("平移旋转标定");

            //VmModule FindModule = (VmModule)VmSolution.Instance["机械手标定1.平移旋转标定1"];

            //if (null == FindModule) return;

            //paramForm.config.ModuleSource = FindModule;
            //paramForm.ShowDialog(); // 显示界面 


            //VmHelper.OpenVmControlWindow("机械手标定1.平移旋转标定1")?.ShowDialog();
            VmHelper.OpenVmControlWindow(VMTagName.机械手标定相机.机械手标定_平移旋转标定)?.ShowDialog();

        }

        /// <summary>
        /// 找交点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cross_Click(object sender, RoutedEventArgs e)
        {
            //var paramForm = new ParamsConfigControl("边缘交点");

            //VmModule FindModule = (VmModule)VmSolution.Instance["机械手标定.边缘交点"];

            //if (null == FindModule) return;

            //paramForm.config.ModuleSource = FindModule;
            //paramForm.ShowDialog(); // 显示界面


            //VmHelper.OpenVmControlWindow("机械手标定.边缘交点")?.ShowDialog();
            VmHelper.OpenVmControlWindow(VMTagName.机械手标定相机.机械手标定_四边形查找)?.ShowDialog();
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }



        /// <summary>
        /// 加载标定文件
        /// </summary>
        public void LoadCailFiles()
        {
            
        }


        ///// <summary>
        ///// 清空标定点
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    ClearCaliStatus();

        //}


        private void ClearCaliStatus()
        {

            //TranslationCalibModuTool CalibTool = VmSolution.Instance["机械手标定1.平移旋转标定1"] as TranslationCalibModuTool;
            TranslationCalibModuTool CalibTool = VmSolution.Instance[VMTagName.机械手标定相机.机械手标定_平移旋转标定] as TranslationCalibModuTool;
            CalibTool.ModuParams.DoClearPoint();

        }

        /// <summary>
        /// 生成标定文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generateCalib_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var path=CurrentInfo.CalibDir.Camera1FirstCailPath + DateTime.Now.CaliFormat() + ".xml";


               
               //var isExportSuccessfull= VmHelper.Export_TranslationCalib_File(VMTagName.机械手标定_平移旋转标定, path);
               var isExportSuccessfull= VmHelper.Export_TranslationCalib_File(VMTagName.机械手标定相机.机械手标定_平移旋转标定, path);
                 
                if(isExportSuccessfull) 
                { 
                    System.Windows.MessageBox.Show("导出成功", "", MessageBoxButton.OK, MessageBoxImage.Information);

                    VmHelper.LoadVmCalibFiles();

                }

                else
                {
                    System.Windows.MessageBox.Show("导出失败", "", MessageBoxButton.OK, MessageBoxImage.Error);

                }
             


            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("导出失败", "", MessageBoxButton.OK, MessageBoxImage.Error);

            }



        }

        private void Btn_checkCaliResult_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //var calibResult = VmHelper.GetCalibParamsResult(VMTagName.机械手标定_平移旋转标定);
                var calibResult = VmHelper.GetCalibParamsResult(VMTagName.机械手标定相机.机械手标定_平移旋转标定);

                if (calibResult.ModuStatus <= 0) {

                    MessageBox.Show("标定未成功");
                    return;

                }


                string msg = $"像素精度 :  {calibResult.PixelPrecision}   X和Y比例   {calibResult.DirectionRatio} \n平移像素平均误差:{calibResult.TransError}  平移平均误差:{calibResult.TransWorldError}";
                System.Windows.MessageBox.Show(msg, "标定结果", MessageBoxButton.OK, MessageBoxImage.Information);



            }
            catch (Exception ex)
            {

                MessageBox.Show("标定未成功");
            }
            
          
        }
    }


    public class LogManager
    {
        #region field

        public ObservableCollection<UILogEntity> logsAll = new ObservableCollection<UILogEntity>();
        public ObservableCollection<UILogEntity> logsDebug = new ObservableCollection<UILogEntity>();
        public ObservableCollection<UILogEntity> logsInfor = new ObservableCollection<UILogEntity>();
        public ObservableCollection<UILogEntity> logsWarn = new ObservableCollection<UILogEntity>();
        public ObservableCollection<UILogEntity> logsError = new ObservableCollection<UILogEntity>();

        //private static readonly UILogManager instance;
        private object _lock = new object();

        #endregion

        #region property

        public int MaxCount { get; set; }

        public bool IsAutoSaveLog { get; set; }

        public string AutoSavedLogPath { get; set; }

        public ObservableCollection<UILogEntity> LogsAll
        {
            get { return logsAll; }
        }

        public ObservableCollection<UILogEntity> LogsDebug
        {
            get { return logsDebug; }
        }

        public ObservableCollection<UILogEntity> LogsInfor
        {
            get { return logsInfor; }
        }

        public ObservableCollection<UILogEntity> LogsWarn
        {
            get { return logsWarn; }
        }

        public ObservableCollection<UILogEntity> LogsError
        {
            get { return logsError; }
        }
        public UInt64 LogsCount
        {
            get
            {
                return (UInt64)logsAll.Count;
            }
        }

        //public static UILogManager Instance
        //{
        //    get
        //    {
        //        return instance;
        //    }
        //}

        /// <summary>
        /// 当前用户
        /// </summary>
        public static string CurUserName { get; set; }

        #endregion
        #region const

        public const int ERR_CODE_OK = 0;
        public const int ERR_CODE_EXCEPTION = -1;

        #endregion

        #region ctor

        public LogManager()
        {
            MaxCount = 300;
        }

        #endregion

        private void DeleteAllLogs()
        {
            lock (_lock)
            {
                LogsAll.Clear();
                LogsDebug.Clear();
                LogsInfor.Clear();
                LogsWarn.Clear();
                LogsError.Clear();
            }
        }

        private void CheckLogsCapacity()
        {
            if (logsAll.Count > MaxCount)
            {
                // DeleteAllLogs( );

                logsAll.RemoveAt(0);
                //SaveLog( AutoSavedLogPath );
            }
        }

        private void AppendLog(UILogLevel level, UILogEntity logEntity)
        {
            if (logEntity == null)
            {
                return;
            }
            lock (_lock)
            {
                LogsAll.Add(logEntity);
                //switch ( level )
                //{
                //    case UILogLevel.Debug:
                //        LogsDebug.Add( logEntity );
                //        break;
                //    case UILogLevel.Infor:
                //        LogsInfor.Add( logEntity );
                //        break;
                //    case UILogLevel.Warn:
                //        LogsWarn.Add( logEntity );
                //        break;
                //    case UILogLevel.Error:
                //        LogsError.Add( logEntity );
                //        break;
                //    default:
                //        break;
                // }
            }
        }

        private void InsertLog(UILogLevel level, UILogEntity logEntity, int index)
        {
            if (index < 0 || logEntity == null)
            {
                return;
            }
            lock (_lock)
            {
                LogsAll.Insert(index, logEntity);
                //switch ( level )
                //{
                //    case UILogLevel.Debug:
                //        LogsDebug.Insert( index, logEntity );
                //        break;
                //    case UILogLevel.Infor:
                //        LogsInfor.Insert( index, logEntity );
                //        break;
                //    case UILogLevel.Warn:
                //        LogsWarn.Insert( index, logEntity );
                //        break;
                //    case UILogLevel.Error:
                //        LogsError.Insert( index, logEntity );
                //        break;
                //    default:
                //        break;
                //}
            }
        }

        public void AddLog(UILogLevel level, string content)
        {

            lock (_lock)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(content))
                        return;


                    CheckLogsCapacity();

                    System.Windows.Application.Current.Dispatcher.Invoke(delegate
                    {
                        logsAll.Add(new UILogEntity(content, level, 0));
                    });
                }
                catch (Exception ex)
                {


                }

            }
        }

        public void AddLog(UILogLevel level, string content, int errorCode, string userName)
        {
            if (string.IsNullOrWhiteSpace(content))
                return;
            UILogEntity logEntity = new UILogEntity(content, level, errorCode, userName, LogsCount);
            AppendLog(level, logEntity);

        }

        public void AddLog(UILogLevel level, string content, int errorCode)
        {
            if (string.IsNullOrWhiteSpace(content))
                return;
            UILogEntity logEntity = new UILogEntity(content, level, errorCode, CurUserName, LogsCount);
            AppendLog(level, logEntity);
        }

        public void ClearLog()
        {
            DeleteAllLogs();
        }

        public bool SaveLog(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                StringBuilder sb = new StringBuilder();
                lock (_lock)
                {
                    foreach (var log in LogsAll)
                    {
                        // sb.Append( log.RowIndex.ToString( ) + "  " + log.Time.ToString( ) + "  " + log.Level.ToString( ) + "  " + log.Content + "\r\n" );
                    }
                }

                string fileName = filePath + "\\" + "RunningLog" + '_' + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".txt";
                if (SaveFile(sb.ToString(), fileName))
                {
                    DeleteAllLogs();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool SaveFile(string fileContent, string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileContent) && !string.IsNullOrWhiteSpace(fileName))
            {
                byte[] tempData = System.Text.UTF8Encoding.UTF8.GetBytes(fileContent);
                try
                {
                    using (FileStream SourceStream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                    {
                        SourceStream.SetLength(0);
                        SourceStream.Write(tempData, 0, tempData.Length);
                        SourceStream.Flush();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }



    }
}
