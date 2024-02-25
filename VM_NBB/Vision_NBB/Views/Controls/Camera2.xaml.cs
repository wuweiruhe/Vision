#define TestDebug
using Apps.Log;
using CalculatorModuleCs;
using IMVSGroupCs;
using PLCDevice;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using Vision_NBB.Log;
using Vision_NBB.Model;
using Vision_NBB.Toolkit;
using Vision_NBB.Utility;
using Vision_NBB.Views.Pages;
using VM.Core;
using VMControls.WPF.Release;
using PLCTagName = Vision_NBB.Model.PLCTagName;

namespace Vision_NBB.Views.Controls
{
    /// <summary>
    /// Camera2.xaml 的交互逻辑
    /// </summary>
    public partial class Camera2 : UserControl
    {
        
        Camera2WindowStatus Status = new Camera2WindowStatus();
        public Dictionary<string, ModelBlock> ModuleInfoDict = new Dictionary<string, ModelBlock>();
        public List<ModelBlock> ItemList = new List<ModelBlock>();
        BasePLC basePLC;
        bool IsCreated = false;
        public string ProcedureName = "";
        private Configuration config;
        VmProcedure process;
        private StringBuilder sb = new StringBuilder();

        Dictionary<string, string> dict = new Dictionary<string, string>();
        public Camera2()
        {
            if (IsCreated) return;

            InitializeComponent();

            this.config = CurrentInfo.Config;

        }

        /// <summary>
        ///  ------------------------------------------ VM 回调处理函数-----------------------------------------------------------------
        /// </summary>

        public void ExecuteAsync()
        {
            var CallBackStopWatch = new Stopwatch();
            CallBackStopWatch.Start();
            try
            {
                var result = new Camera2Result();
                if (null == process) return;
                var moduResult = process.ModuResult;


                try
                {
                    #region 获取VM回调结果

                    result.OutX = moduResult.GetOutputFloat("outX").pFloatVal[0];
                    result.OutY = moduResult.GetOutputFloat("outY").pFloatVal[0];
                    result.OutR = moduResult.GetOutputFloat("outR").pFloatVal[0];
                    result.OutOffset = moduResult.GetOutputFloat("outOffset").pFloatVal[0];
                    //result.NgResultString = moduResult.GetOutputString("NGResultString").astStringVal[0].strValue;
                    result.CameraResult = moduResult.GetOutputInt("CameraResult").pIntVal[0];
                    #endregion

                }
                catch (Exception ex)
                {
                    GetLogHelper.VisionLog.Error("相机2 获取VM 数据异常" + ex.ToString());

                    result.OutX = 0;
                    result.OutY = 0;
                    result.OutR = 0;
                    result.OutOffset = 0;
                    result.CameraResult = -1;
                }


                Status.Ng_color = result.CameraResult > 0 ? MyColor.Ok_Color : MyColor.Ng_color;
                Status.Ng_text = result.CameraResult > 0 ? "OK" : "NG";
          

             

                Status.Result2 = result;

                var dt = DateTime.Now;

                VmHelper.SaveImage(result.CameraResult, CurrentInfo.CameraDir.Camera2_FileDirName, dt.DateNowFormat(), ProcedureName, config);



                #region 保存csv
                var src_path = FileHelper.CreateDirByTime(config.cameraSettings.ImagePath, CurrentInfo.CameraDir.Camera2_FileDirName, "Content");
                AddResultText("时间", dt.DateNowFormat());
                AddResultText("X", result.OutX);
                AddResultText("Y", result.OutY);
                AddResultText("R", result.OutR);
                AddResultText("Offset", result.OutOffset);
                //AddResultText("NG说明", result.NgResultString);

                FileHelper.WriteText(src_path + "\\content.txt", sb.ToString());
                #endregion


                #region Send PLC Data

#if !TestDebug
                float[] outData = new float[10];
                outData[0] = result.OutOffset;// result.OutX;
                outData[1] = result.OutY;
                outData[2] = result.OutR;
                outData[3] = result.CameraResult;
                //outData[4] = result.OutOffset;
                basePLC.WriteValue(PLCTagName.FixtureCCD_Senddata, outData);
                Thread.Sleep(10);
                basePLC.WriteValue(PLCTagName.FixtureDoneLable, 1);


#endif

                #endregion

                //CameraHelper.ShowNgIcon(process.Modules, config.workFlowSorts.camera2_Sequence, ModuleInfoDict);

                CameraHelper.ShowNgIcon2(process.Modules, dict, ModuleInfoDict);


            }
            catch (Exception ex)
            {
                GetLogHelper.VisionLog.Error("相机2执行异常" + ex.ToString());
             

                UILogMangerHelper.Instance.AddLog(LogLevel.Error, "相机2 VM 回调执行异常");
                Thread.Sleep(100);
            }
            finally
            {
                CallBackStopWatch.Stop();
                Status.TimeStatistic = Convert.ToInt32(CallBackStopWatch.ElapsedMilliseconds + process.ProcessTime + 50);//加相机执行时间
            }
        }


#region 控件点击事件



        private void ButtonContinuExecute_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 执行一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonExecuteOnce_Click(object sender, RoutedEventArgs e)
        {

            if (CurrentInfo.Config.cameraSettings.TriggerMode == 0)
            {

                System.Windows.MessageBox.Show("相机处于硬触发模式 无法手动触发", "", MessageBoxButton.OK, MessageBoxImage.Information);


                return;
            }
            //var process = (VmProcedure)VmSolution.Instance[ProcedureName];
            //if (null == process) return;
            //process.Run();
        }


        private void ButtonStopExecute_Click(object sender, RoutedEventArgs e)
        {
        }

        private void TvProperties_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //需要判断是否有操作权限
                var modul = (ModelBlock)this.tvProperties.SelectedItem;
                string name = modul.fullName + "." + modul.RealName;
                VmHelper.OpenVmControlWindow(name).ShowDialog(); ;
            }
            catch (Exception ex)
            {
                GetLogHelper.VisionLog.Error(ex.ToString());
            }
        }



        /// <summary>
        /// NG历史
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_NgHistroy_Click(object sender, RoutedEventArgs e)
        {
            new NgImageHistory(2).ShowDialog();
        }

#endregion


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsCreated) return;
                IsCreated = true;
                //List<VmProcedure> procedures = new List<VmProcedure>();
                //VmSolution.Instance.GetAllProcedureObjects(ref procedures);

                //ProcedureName = procedures[1].FullName;

                //process = VmSolution.Instance[ProcedureName] as VmProcedure;
                //this.vm.ModuleSource = process;
                //this.DataContext = this.Status;

            }
            catch (Exception ex)
            {
                UILogMangerHelper.Instance.AddLog(LogLevel.Error, "相机2窗体加载异常");
               GetLogHelper.VisionLog.Info("相机2窗体加载异常:  " + ex.ToString());
            }
        }



        public void ShowTreeView(IEnumerable<ModuInfo> ModuInfos, WorkFlowSorts WorkFlowSort, BasePLC plc, string processFullName)
        {
            try
            {
#region showView
                this.basePLC = plc;
                var workFlowSort = WorkFlowSort;
                List<ModelBlock> list = new List<ModelBlock>();

                this.dict = Toolkits.CameraSequenceToDict(WorkFlowSort.camera2_Sequence);

                //遍历模块信息
                foreach (var item in ModuInfos)
                {
                    if (item.strDisplayName == null) continue;
                    ModelBlock child = new ModelBlock();

                    child.RealName = item.strDisplayName;
                    child.DisplayName = item.strDisplayName;
                    child.Icon = Toolkits.getFileByName(item.strModuleName);
                    child.id = item.nModuleID.ToString();
                    child.Name = item.strModuleName;
                    child.processID = item.nProcessID.ToString();
                    child.fullName = processFullName;
                    ModuleInfoDict.Add(child.id, child);
                    list.Add(child);
                }

                ModelBlock node = new ModelBlock()
                {
                    DisplayName = "Camera2",
                    Name = "This is the discription of Node1. This is a folder.",
                    Icon = "/ICON/toolbar_logical_分支.png",
                };


                var data = Toolkits.ParseCameraSequence(WorkFlowSort.camera2_Sequence);


                foreach (var IDitem in data[0])
                {
                    var nodeTag = list.Where(p => p.id == IDitem).FirstOrDefault();
                    node.Children.Add(nodeTag);
                }


                ItemList.Add(node);

                this.tvProperties.ItemsSource = ItemList;

             

                Status.VmGlobalData = VmGlobalDataModel.Instance();

#endregion

               

            }
            catch (Exception ex)
            {
               GetLogHelper.VisionLog.Error(ex);
            }
        }


       


        public void AddResultText(string name, object value)
        {
            sb.Append($"{name}: {value.ToString()}" + "\r\n");
        }




    }



    public class Camera2WindowStatus : BaseWindowStatus
    {

        public Camera2WindowStatus()
        {

        }
        private Camera2Result result;

        public Camera2Result Result2
        {
            get { return result; }
            set
            {
                result = value;

                OnPropertyChanged("Result2");
            }
        }



        private VmGlobalDataModel vmGlobalData;

        public VmGlobalDataModel VmGlobalData
        {
            get { return vmGlobalData; }
            set
            {
                vmGlobalData = value;

                OnPropertyChanged("VmGlobalData");
            }
        }




    }

#region 相机2 绑定结果模型

    public class Camera2Result
    {
        private float _outOffset;

        public float OutOffset
        {
            get { return _outOffset.ToF3(); }
            set { _outOffset = value; }
        }



        private float _outX;

        public float OutX
        {
            get { return _outX.ToF3(); }
            set { _outX = value; }
        }

        private float _outY;

        public float OutY
        {
            get { return _outY.ToF3(); }
            set { _outY = value; }
        }
        private float _outR;

        public float OutR
        {
            get { return _outR.ToF3(); }
            set { _outR = value; }
        }


        private int _cameraResult;

        public int CameraResult
        {
            get { return _cameraResult; }
            set { _cameraResult = value; }
        }


        private string _ngResultString;

        public string NgResultString
        {
            get { return _ngResultString; }
            set { _ngResultString = value; }
        }

    }
#endregion
}
