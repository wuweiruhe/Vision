using Apps.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Vision_NBB.Model;
using VM.Core;
using Vision_NBB.Toolkit;
using Vision_NBB.Utility;
using System.ComponentModel;
using System.Threading.Tasks;
using PLCDevice;
using Vision_NBB.Model;
using System.Threading;
using Vision_NBB.Views.Pages;
using Vision_NBB.Log;
using System.Diagnostics;
using System.Text;
using System.Collections.ObjectModel;
using PLCTagName = Vision_NBB.Model.PLCTagName;

namespace Vision_NBB.Views.Controls
{
    /// <summary>
    /// Camera1.xaml 的交互逻辑
    /// </summary>
    public partial class Camera1 : UserControl
    {
        Camera1WindowStatus Status = new Camera1WindowStatus();
     
        public Dictionary<string, ModelBlock> ModuleInfoDict = new Dictionary<string, ModelBlock>();
        public List<ModelBlock> ItemList = new List<ModelBlock>();
        BasePLC basePLC;
        public bool IsCreated = false;
        public string ProcedureName = "";
        private Configuration config;
        private StringBuilder sb = new StringBuilder();
        VmProcedure process;

       Dictionary<string,string> dict = new Dictionary<string,string>();

      



        /// <summary>
        /// 初始化
        /// </summary>
        public Camera1()
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
            sb.Clear();
            var CallBackStopWatch = new Stopwatch();
            CallBackStopWatch.Start();
            try
            {
                var result = new Camera1Result();
                if (null == process) return;
                var moduResult = process.ModuResult;

                try
                {
                    #region 获取VM回调结果
                    result.OutX = moduResult.GetOutputFloat("outX").pFloatVal[0];
                    result.OutY = moduResult.GetOutputFloat("outY").pFloatVal[0];
                    result.OutR = moduResult.GetOutputFloat("outR").pFloatVal[0];
                    //result.NgResultString = moduResult.GetOutputString("NgResultString").astStringVal[0].strValue;
                    //result.Vdistance = moduResult.GetOutputFloat("Vdistance").pFloatVal[0];
                    //result.Hdistance = moduResult.GetOutputFloat("Hdistance").pFloatVal[0];
                    result.CameraResult = moduResult.GetOutputInt("CameraResult").pIntVal[0];
                    #endregion
                }
                catch ( Exception ex)
                {
                    result.OutX =0;
                    result.OutY = 0;
                    result.OutR = 0;
                 
                    result.Vdistance = 0;
                    result.Hdistance = 0;
                    result.CameraResult = -1;
                    GetLogHelper.VisionLog.Error("相机1 获取VM 数据异常" + ex.ToString());

                    
                }

               
                Status.Ng_color = result.CameraResult > 0 ? MyColor.Ok_Color : MyColor.Ng_color;
                Status.Ng_text = result.CameraResult > 0 ? "OK" : "NG";

             
             
                Status.Result1 = result;

                var dt = DateTime.Now.DateNowFormat();
                VmHelper.SaveImage(result.CameraResult, CurrentInfo.CameraDir.Camera1_FileDirName, dt, ProcedureName, config);

                FileHelper.DeletedirByDay(FileHelper.Camera1_DeleteLock, config.cameraSettings.ImagePath, CurrentInfo.CameraDir.Camera1_FileDirName, config.cameraSettings.MaxSaveDays);

                #region 保存csv

                var src_path = FileHelper.CreateDirByTime(config.cameraSettings.ImagePath, CurrentInfo.CameraDir.Camera1_FileDirName, "Content");


                AddResultText("时间", dt);
                AddResultText("X", result.OutX);
                AddResultText("Y", result.OutY);
                AddResultText("R", result.OutR);

                AddResultText("ResultType", result.CameraResult);

                //AddResultText("NG说明", result.NgResultString);
                AddResultText("竖直间距", result.Vdistance);
                AddResultText("水平间距", result.Hdistance);
                FileHelper.WriteText(src_path + "\\content.txt", sb.ToString());

                #endregion



                #region Send PLC Data
                float[] outData = new float[10];
                outData[0] = result.OutX;
                outData[1] = result.OutY;
                outData[2] = result.OutR;
                outData[3] = result.CameraResult;
                outData[3] = result.Vdistance;
                outData[5] = result.Vdistance;

#if !TestDebug
                basePLC.WriteValue(PLCTagName.Robot_calib_Senddata, outData);
                Thread.Sleep(10);
                basePLC.WriteValue(PLCTagName.CamaraDoneRobotLable, 1);
                basePLC.WriteValue(PLCTagName.Camera1_Status, PLCTagName.Ready);

#endif


                #endregion

                //CameraHelper.ShowNgIcon(process.Modules, config.workFlowSorts.camera1_Sequence, ModuleInfoDict);
                CameraHelper.ShowNgIcon2(process.Modules, dict, ModuleInfoDict);

            }
            catch (Exception ex)
            {
                 GetLogHelper.VisionLog.Error("相机1执行异常" + ex.ToString());
                 UILogMangerHelper.Instance.AddLog(LogLevel.Error, "相机1 VM 回调执行异常");
                 Thread.Sleep(100);
            }
            finally
            {
                CameraState.Camera1_IsRunning = false;
                CallBackStopWatch.Stop();
                Status.TimeStatistic = Convert.ToInt32(CallBackStopWatch.ElapsedMilliseconds + process.ProcessTime + 50);//加相机执行时间
            }
        }




        /// <summary>
        /// 窗体load加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsCreated) return;
                IsCreated = true;
                List<VmProcedure> procedures = new List<VmProcedure>();
                VmSolution.Instance.GetAllProcedureObjects(ref procedures);
                ProcedureName = procedures[0].FullName;
                process = VmSolution.Instance[ProcedureName] as VmProcedure;

                this.vm.ModuleSource = process;

                this.DataContext = this.Status;


             


            }
            catch (Exception ex)
            {
                UILogMangerHelper.Instance.AddLog(LogLevel.Error, "相机1窗体加载异常");
               GetLogHelper.VisionLog.Info("相机1窗体加载异常:  " + ex.ToString());
            }
        }



#region 控件点击事件
        /// <summary>
        /// 双击tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TvProperties_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //需要判断是否有操作权限 
                var modul = (ModelBlock)this.tvProperties.SelectedItem;
                string name = modul.fullName + "." + modul.RealName;
                var state = VmHelper.OpenVmControlWindowByOwner(name, this)?.ShowDialog();

            }
            catch (Exception ex)
            {
                GetLogHelper.VisionLog.Info(ex); //本地VisionLog保存
            }
        }


        /// <summary>
        /// 连续执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonContinuExecute_Click(object sender, RoutedEventArgs e)
        {
            //var process2 = (VmProcedure)VmSolution.Instance[ProcedureName];
            //if (null == process2) return;

            //process2.ContinuousRunEnable = true;
        }



        /// <summary>
        /// 执行一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonExecuteOnce_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentInfo.Config.cameraSettings.TriggerMode == 0)
                {

                    System.Windows.MessageBox.Show("相机处于硬触发模式 无法手动触发", "", MessageBoxButton.OK, MessageBoxImage.Information);
                  

                    return;
                }
                var process = (VmProcedure)VmSolution.Instance[ProcedureName];
                if (null == process) return;
                process.Run();
            }
            catch (Exception ex)
            {
                GetLogHelper.VisionLog.Info(ex); //本地VisionLog保存
            }
        }




        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonStopExecute_Click(object sender, RoutedEventArgs e)
        {
            //var process2 = (VmProcedure)VmSolution.Instance[ProcedureName];
            //if (null == process2) return;
            //process2.ContinuousRunEnable = false;
        }


        /// <summary>
        /// NG历史图片加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_NgHistroy_Click(object sender, RoutedEventArgs e)
        {
            new NgImageHistory(1).ShowDialog();

 
        }



#endregion



        
        public void ShowTreeView(IEnumerable<ModuInfo> ModuInfos, WorkFlowSorts WorkFlowSort, BasePLC plc, string processFullName)
        {
            try
            {

                #region showView
                this.basePLC = plc;
                var workFlowSort = WorkFlowSort;
                List<ModelBlock> list = new List<ModelBlock>();



                this.dict= Toolkits.CameraSequenceToDict(WorkFlowSort.camera1_Sequence);

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
                    DisplayName = "Camera1",
                    Name = "This is the discription of Node1. This is a folder.",
                    Icon = "/ICON/toolbar_logical_分支.png",
                };


                var data = Toolkits.ParseCameraSequence(WorkFlowSort.camera1_Sequence);


                foreach (var IDitem in data[0])
                {
                    var nodeTag = list.Where(p => p.id == IDitem).FirstOrDefault();
                    node.Children.Add(nodeTag);
                }


                ItemList.Add(node);

                this.tvProperties.ItemsSource = ItemList;

                #endregion

                Status.VmGlobalData = VmGlobalDataModel.Instance();

            }
            catch (Exception ex)
            {
               GetLogHelper.VisionLog.Info(ex); //本地VisionLog保存
            }
        }


        public void AddResultText(string name, object value)
        {
            sb.Append($"{name}: {value.ToString()}"+"  ");
        }

 



    }




   public class Camera1WindowStatus: BaseWindowStatus
    {


        private Camera1Result result;

        public Camera1Result Result1
        {
            get { return result; }
            set
            {
                result = value;

                OnPropertyChanged("Result1");
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




#region 相机1 Vm绑定结果模型

    public class Camera1Result
    {
        private string _outMsg;

        public string OutMsg
        {
            get { return _outMsg; }
            set { _outMsg = value; }
        }


        private float _outX;

        public float OutX
        {
            get { return   _outX.ToF3(); }
            set { _outX =  value; }
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

        //private float[] _height;

        //public float[] Height
        //{
        //    get { return _height; }
        //    set { _height = value; }
        //}

        //private float[] _width;

        //public float[] Width
        //{
        //    get { return _width; }
        //    set { _width = value; }
        //}

        /// <summary>
        /// 竖直间距
        /// </summary>
        private float _vdistance;

        public float Vdistance
        {
            get { return _vdistance.ToF3(); }
            set { _vdistance = value; }
        }

        /// <summary>
        /// 水平间距
        /// </summary>
        private float _hdistance;

        public float Hdistance
        {
            get { return _hdistance.ToF3(); }
            set { _hdistance = value; }
        }

    }

#endregion


}
