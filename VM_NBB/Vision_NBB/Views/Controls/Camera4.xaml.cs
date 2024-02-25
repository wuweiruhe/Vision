#define TestDebug
using Apps.Log;
using CalculatorModuleCs;
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
using System.Xml.Linq;
using Vision_NBB.Log;
using Vision_NBB.Model;
using Vision_NBB.Toolkit;
using Vision_NBB.Utility;
using Vision_NBB.Views.Pages;
using VM.Core;
using PLCTagName = Vision_NBB.Model.PLCTagName;

namespace Vision_NBB.Views.Controls
{
    /// <summary>
    /// Camera4.xaml 的交互逻辑  
    /// </summary>
    public partial class Camera4 : UserControl
    {
        
        Camera4WindowStatus Status = new Camera4WindowStatus();
        public Dictionary<string, ModelBlock> ModuleInfoDict = new Dictionary<string, ModelBlock>();
        public List<ModelBlock> ItemList = new List<ModelBlock>();
        BasePLC basePLC;

        bool IsCreated = false;
        public string ProcedureName = "";
        private Configuration config;
        VmProcedure process;
        private StringBuilder sb = new StringBuilder();
        public Action<Data> SetDataHistory;
        public List<string> dataHistoryItems = new List<string>();
        public List<Data> dataItemList = new List<Data>();
        private CellData cellData = new CellData();
        Dictionary<string, string> dict = new Dictionary<string, string>();

        private List<string> GroupNmae = new List<string>();


        public Camera4()
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
                var result = new Camera4Result();
                if (null == process) return;
                var moduResult = process.ModuResult;


                #region 获取VM回调结果

                try
                {

                    result.OutY1 = moduResult.GetOutputFloat("outY1").pFloatVal;
                    result.OutY2 = moduResult.GetOutputFloat("outY2").pFloatVal;
                    result.OutY3 = moduResult.GetOutputFloat("outY3").pFloatVal;
                    result.OutY4 = moduResult.GetOutputFloat("outY4").pFloatVal;
                    result.OutY5 = moduResult.GetOutputFloat("outY5").pFloatVal;
                    result.OutY6 = moduResult.GetOutputFloat("outY6").pFloatVal;
                    result.OutY7 = moduResult.GetOutputFloat("outY7").pFloatVal;
                    result.OutY8 = moduResult.GetOutputFloat("outY8").pFloatVal;
                    result.OutUpX = moduResult.GetOutputFloat("outUp_X").pFloatVal;
                    result.OutDwX = moduResult.GetOutputFloat("outDw_X").pFloatVal;

                    result.UpResult = moduResult.GetOutputInt("UpResult").pIntVal[0];
                    result.DwResult = moduResult.GetOutputInt("DwResult").pIntVal[0];
                    result.CameraResult = moduResult.GetOutputInt("CameraResult").pIntVal[0];

                    var count = VmGlobalDataModel.Instance().WeldingCount;

                    if (count < 28)
                    {
                        float[] _FillZero = new float[28 - count];


                        result.OutY1 = result.OutY1.Concat(_FillZero).ToArray();
                        result.OutY2 = result.OutY2.Concat(_FillZero).ToArray();
                        result.OutY3 = result.OutY3.Concat(_FillZero).ToArray();
                        result.OutY4 = result.OutY4.Concat(_FillZero).ToArray();
                        result.OutY5 = result.OutY5.Concat(_FillZero).ToArray();
                        result.OutY6 = result.OutY6.Concat(_FillZero).ToArray();
                        result.OutY7 = result.OutY7.Concat(_FillZero).ToArray();
                        result.OutY8 = result.OutY8.Concat(_FillZero).ToArray();

                    }

                }
                catch (Exception ex)
                {

                    GetLogHelper.VisionLog.Error("相机4 接受VM数据异常" + ex.ToString());
                    UILogMangerHelper.Instance.AddLog(LogLevel.Error, "相机4 接受VM数据异常");
                }


                if (result.UpResult < 0) { result.OutUpX[0] = 0; }
                float[] _UpResult = new float[1] { (float)result.UpResult };



                if (result.DwResult < 0) { result.OutDwX[0] = 0; }
                float[] _DwResult = new float[1] { (float)result.DwResult };


                float[] _result = new float[1] { (float)result.CameraResult };

                var len = 28;



                float[] ZeroFill = new float[0];

                var UpGlueData = result.OutY1.Concat(ZeroFill).Concat(result.OutY2).Concat(ZeroFill).Concat(result.OutY3).Concat(ZeroFill).Concat(result.OutY4).Concat(ZeroFill).ToArray();
                var DownGlueData = result.OutY5.Concat(ZeroFill).Concat(result.OutY6).Concat(ZeroFill).Concat(result.OutY7).Concat(ZeroFill).Concat(result.OutY8).Concat(ZeroFill).ToArray();


                float[] Fill = new float[len];

                float[] Fill2 = new float[16];

                var outData = UpGlueData.Concat(ZeroFill).Concat(DownGlueData).Concat(ZeroFill).Concat(Fill).Concat(ZeroFill).Concat(Fill).Concat(ZeroFill).Concat(result.OutUpX).Concat(ZeroFill).Concat(result.OutDwX).Concat(ZeroFill).Concat(_UpResult).Concat(ZeroFill).Concat(_DwResult).Concat(ZeroFill).Concat(Fill2).Concat(ZeroFill).Concat(_result).ToArray();



                #endregion







                Status.Ng_color = result.CameraResult > 0 ? MyColor.Ok_Color : MyColor.Ng_color;
                Status.Ng_text = result.CameraResult > 0 ? "OK" : "NG";
            
           
                Status.Result4 = result;
                var dt = DateTime.Now;

                 cellData.AddData(result.UpResult, result.DwResult,ref UpGlueData,ref DownGlueData,ref result.OutUpX[0],ref result.OutDwX[0]);

                #region Send PLC Data



                //#if !TestDebug
                basePLC.WriteValue(PLCTagName.BackGlueCCD_Senddata, outData);
                Thread.Sleep(10);
                basePLC.WriteValue(PLCTagName.BackGlueCCD_Done, 1);
                basePLC.WriteValue(PLCTagName.Camera4_Status, PLCTagName.Ready);


                //#endif


                #endregion

                CallBackStopWatch.Stop();

                var imagePath = VmHelper.SaveImage2(result.CameraResult, CurrentInfo.CameraDir.Camera4_FileDirName, dt.DateNowFormat(), ProcedureName, config, CurrentInfo.Config.cameraSettings.IsForceSaveOK);

                FileHelper.DeletedirByDay(FileHelper.Camera4_DeleteLock, config.cameraSettings.ImagePath, CurrentInfo.CameraDir.Camera4_FileDirName, config.cameraSettings.MaxSaveDays);

                #region 保存csv
                string a_status = result.UpResult > 0 ? $"OK  {result.UpResult}" : "NG  {result.UpResult}";
                string b_status = result.DwResult > 0 ? "OK  {result.UpResult}" : "NG  {result.UpResult}";
                var src_path = FileHelper.CreateDirByTime(config.cameraSettings.ImagePath, CurrentInfo.CameraDir.Camera4_FileDirName, "Content");

                CallBackStopWatch.Stop();

                AddResultText("时间", dt.DateNowFormat());

                AddResultText("CT", (CallBackStopWatch.ElapsedMilliseconds + process.ProcessTime + 36).ToString());
                AddResultText("A结果", a_status);
                AddResultText("X", result.OutUpX.ArrayTostring());
                AddResultText("Y1", result.OutY1.ArrayTostring());
                AddResultText("Y2", result.OutY2.ArrayTostring());
                AddResultText("Y3", result.OutY3.ArrayTostring());
                AddResultText("Y4", result.OutY4.ArrayTostring());

                AddResultText("B结果", b_status);
                AddResultText("X", result.OutDwX.ArrayTostring());
                AddResultText("Y5", result.OutY5.ArrayTostring());
                AddResultText("Y6", result.OutY6.ArrayTostring());
                AddResultText("Y7", result.OutY7.ArrayTostring());
                AddResultText("Y8", result.OutY8.ArrayTostring());
                sb.Append("----------------------------------------------------------------" + "\r\n");

                FileHelper.WriteText(src_path + "\\content.txt", sb.ToString());

                result.OutMsg = sb.ToString();

                Data data = new Data();
                data.DataImagePath = imagePath;
                data.CreateteTime = dt;

                data.Msg = result.OutMsg;
                Status.Result4 = result;

                #endregion

                dataItemList.Add(data);
                
                SetDataLengthRange(dataItemList);

             
                //CameraHelper.ShowNgIcon2(process.Modules, dict, ModuleInfoDict);

                //CameraHelper.ShowNgIcon(process.Modules, config.workFlowSorts.camera4_Sequence, ModuleInfoDict);



            }
            catch (Exception ex)
            {
                GetLogHelper.VisionLog.Error("相机4执行异常" + ex.ToString());
                UILogMangerHelper.Instance.AddLog(LogLevel.Error, "相机4 VM 回调执行异常");
                Thread.Sleep(100);
            }
            finally
            {
                CameraState.Camera4_IsRunning = false;
                CallBackStopWatch.Stop();
                Status.TimeStatistic = Convert.ToInt32(CallBackStopWatch.ElapsedMilliseconds + process.ProcessTime + 36);//加相机执行时间
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
               
                if (IsCreated) return;
                IsCreated = true;
                List<VmProcedure> procedures = new List<VmProcedure>();
                VmSolution.Instance.GetAllProcedureObjects(ref procedures);

                VmProcedure procedure4 = VmSolution.Instance[VMTagName.背点胶相机.背点胶相机流程名] as VmProcedure;

                ProcedureName = procedure4.FullName;

             
                process = VmSolution.Instance[ProcedureName] as VmProcedure;
                this.vm.ModuleSource = process;
                this.DataContext = this.Status;

                ///////////////////////////////////////////////////////////////////////////


            }
            catch (Exception ex)
            {
                UILogMangerHelper.Instance.AddLog(LogLevel.Error, "相机4窗体加载异常");
                GetLogHelper.VisionLog.Info("相机4窗体加载异常:  " + ex.ToString());
            }
        }


        public void ShowTreeView(IEnumerable<ModuInfo> ModuInfos, WorkFlowSorts WorkFlowSort, BasePLC plc, string processFullName)
        {
            try
            {

                this.basePLC = plc;
                var workFlowSort = WorkFlowSort;
                List<ModelBlock> list = new List<ModelBlock>();

                this.dict = Toolkits.CameraSequenceToDict(WorkFlowSort.camera4_Sequence);

                //遍历模块信息
                foreach (var item in ModuInfos)
                {

                    if (item.nModuleID > 1500)
                    {
                        GroupNmae.Add(item.strDisplayName);  // ID 大于1000 Vm 一般为组合模块
                    }




                    if (item.strDisplayName == null) continue;
                    ModelBlock child = new ModelBlock();

                    child.RealName = item.strDisplayName;
                    child.DisplayName = item.strDisplayName;
                    //    child.Icon = "/ICON/toolbar_logical_分支.png";
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
                    DisplayName = "Camera4 上测",
                    Name = "This is the discription of Node1. This is a folder.",
                    Icon = "/ICON/toolbar_logical_分支.png",
                };

                ModelBlock node2 = new ModelBlock()
                {
                    DisplayName = "Camera4 下测",
                    Name = "This is the discription of Node1. This is a folder.",
                    Icon = "/ICON/toolbar_logical_分支.png",
                };


                var data = Toolkits.ParseCameraSequence(WorkFlowSort.camera4_Sequence);


                foreach (var IDitem in data[0])
                {
                    var nodeTag = list.Where(p => p.id == IDitem).FirstOrDefault();
                    if (nodeTag != null)
                        node.Children.Add(nodeTag);
                }

                foreach (var IDitem in data[1])
                {
                    var nodeTag = list.Where(p => p.id == IDitem).FirstOrDefault();
                    if (nodeTag != null)
                        node2.Children.Add(nodeTag);
                }


                ItemList.Add(node);
                ItemList.Add(node2);

                this.tvProperties.ItemsSource = ItemList;



            }
            catch (Exception ex)
            {
                GetLogHelper.VisionLog.Info(ex); //本地VisionLog保存
            }
        }



        public void AddResultText(string name, object value)
        {
            sb.Append($"{name}: {value.ToString()}" + "\r\n");
        }


        public void SetDataLengthRange(List<Data> itemList, int Maxlength = 80)
        {
            if (itemList.Count >= Maxlength)
            {
                if (itemList.Count > 11)
                    itemList.RemoveRange(0, 10);
            }

        }


        #region 点击事件


        /// <summary>
        /// NG历史
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_NgHistroy_Click(object sender, RoutedEventArgs e)
        {
            new NgImageHistory(4).ShowDialog();
        }

        private void Btn_DataHistroy_Click(object sender, RoutedEventArgs e)
        {

            DataHistoryWindow dataHistory = new DataHistoryWindow(4, dataItemList);
            SetDataHistory = dataHistory.AddData;
            dataHistory.ShowDialog();

            SetDataHistory = null;

        }
        private void TvProperties_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var modul = (ModelBlock)this.tvProperties.SelectedItem;
                if (modul == null) return;
                string name = modul.fullName + "." + modul.RealName;


                var state = VmHelper.OpenVmControlWindowByOwner(name, this)?.ShowDialog();

               

                if (state == null)
                {

                    foreach (var item in GroupNmae)
                    {

                        string G_Name = $"{modul.fullName}.{item}.{modul.RealName}";

                        VmModule FindModule = (VmModule)VmSolution.Instance[G_Name];

                        if (FindModule == null) continue;
                        if (FindModule.ID == Convert.ToUInt16(modul.id))
                        {

                            VmHelper.OpenVmControlWindowByOwner(G_Name, this)?.ShowDialog(); 
                        }


                    }

                }
            }
            catch (Exception ex)
            {
                GetLogHelper.VisionLog.Error(ex.ToString());
            }
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

            var process = (VmProcedure)VmSolution.Instance[ProcedureName];
            if (null == process) return;
            process.Run();

        }


        private void ButtonContinuExecute_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonStopExecute_Click(object sender, RoutedEventArgs e)
        {

        }



        #endregion



    }




    #region 模型



    public class Camera4WindowStatus : BaseWindowStatus
    {


        private Camera4Result result;

        public Camera4Result Result4
        {
            get { return result; }
            set
            {
                result = value;

                OnPropertyChanged("Result4");
            }
        }




    }

    #region MyRegion

    /// <summary>
    /// 背点胶
    /// </summary>
    public class Camera4Result
    {
        private string _outMsg;

        public string OutMsg
        {
            get { return _outMsg; }
            set { _outMsg = value; }
        }

        private float[] _outY1;

        public float[] OutY1
        {
            get { return _outY1; }
            set { _outY1 = value; }
        }

        private float[] _outY2;

        public float[] OutY2
        {
            get { return _outY2; }
            set { _outY2 = value; }
        }

        private float[] _outY3;

        public float[] OutY3
        {
            get { return _outY3; }
            set { _outY3 = value; }
        }

        private float[] _outY4;

        public float[] OutY4
        {
            get { return _outY4; }
            set { _outY4 = value; }
        }

        private float[] _outY5;

        public float[] OutY5
        {
            get { return _outY5; }
            set { _outY5 = value; }
        }

        private float[] _outY6;

        public float[] OutY6
        {
            get { return _outY6; }
            set { _outY6 = value; }
        }

        private float[] _outY7;

        public float[] OutY7
        {
            get { return _outY7; }
            set { _outY7 = value; }
        }

        private float[] _outY8;

        public float[] OutY8
        {
            get { return _outY8; }
            set { _outY8 = value; }
        }

        private float[] _outUpX;

        public float[] OutUpX
        {
            get { return _outUpX; }
            set { _outUpX = value; }
        }


        private float[] _outDwX;

        public float[] OutDwX
        {
            get { return _outDwX; }
            set { _outDwX = value; }
        }

        private int _upResult;

        public int UpResult
        {
            get { return _upResult; }
            set { _upResult = value; }
        }

        private int _dwResult;

        public int DwResult
        {
            get { return _dwResult; }
            set { _dwResult = value; }
        }

        private int _cameraResult;

        public int CameraResult
        {
            get { return _cameraResult; }
            set { _cameraResult = value; }
        }







        /// <summary>
        /// 基准X
        /// </summary>
        private float _datumX;

        public float DatumX
        {
            get { return _datumX; }
            set { _datumX = value; }
        }


        /// <summary>
        /// 基准Y
        /// </summary>
        private float _datumY;

        public float DatumY
        {
            get { return _datumY; }
            set { _datumY = value; }
        }


        /// <summary>
        /// 治具偏移量
        /// </summary>
        private float _fixtureOffset;

        public float FixtureOffset
        {
            get { return _fixtureOffset; }
            set { _fixtureOffset = value; }
        }


        /// <summary>
        /// 基准角度
        /// </summary>
        private float _referenceAngle;

        public float ReferenceAngle
        {
            get { return _referenceAngle; }
            set { _referenceAngle = value; }
        }


        /// <summary>
        /// 实际抓取X
        /// </summary>
        private float _actualGrabX;

        public float ActualGrabX
        {
            get { return _actualGrabX; }
            set { _actualGrabX = value; }
        }


        /// <summary>
        /// 实际抓取Y 
        /// </summary>
        private float _actualGrabY;

        public float ActualGrabY
        {
            get { return _actualGrabY; }
            set { _actualGrabY = value; }
        }


        /// <summary>
        /// 实际抓取角度
        /// </summary>
        private float _actualGrabAngle;

        public float ActualGrabAngle
        {
            get { return _actualGrabAngle; }
            set { _actualGrabAngle = value; }
        }



    }
    #endregion


    #endregion

}
