#define TestDebug
using Apps.Log;
using CalculatorModuleCs;
using PLCDevice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static SQLite.SQLite3;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using PLCTagName = Vision_NBB.Model.PLCTagName;


namespace Vision_NBB.Views.Controls
{

    public partial class Camera3 : UserControl
    {
        
        Camera3WindowStatus Status = new Camera3WindowStatus();
        public Dictionary<string, ModelBlock> ModuleInfoDict = new Dictionary<string, ModelBlock>();
        public List<ModelBlock> ItemList = new List<ModelBlock>();
        bool IsCreated = false;
        public string ProcedureName = "";
        private Configuration config;
        VmProcedure process;
        BasePLC basePLC;
        private StringBuilder sb = new StringBuilder();
        public Action<Data> SetDataHistory;
        public List<Data> dataItemList = new List<Data>();
        private bool isUp = true;
        private bool isDown = true;
        private List<float> temp_UpGlueData1 =new List<float>();
        private List<float> temp_UpGlueData2 = new List<float>();
        private List<float> temp_DownGlueData1 = new List<float>();
        private List<float> temp_DownGlueData2 = new List<float>();
        private float[] temp_OutUpX1;
        private float[] temp_OutUpX2;
        private float[] temp_OutDwX1 ;
        private float[] temp_OutDwX2;
        private bool init_status = true;
        private CellData cellData = new CellData();
        private List<string> GroupNmae = new List<string>();
        Dictionary<string, string> dict = new Dictionary<string, string>();


        public Camera3()
        {
            if (IsCreated) return;
            InitializeComponent();
            this.config = CurrentInfo.Config;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (IsCreated) return;
                IsCreated = true;
                List<VmProcedure> procedures = new List<VmProcedure>();
                VmSolution.Instance.GetAllProcedureObjects(ref procedures);

                VmProcedure procedure3 = VmSolution.Instance[VMTagName.上点胶相机.上点胶相机流程名] as VmProcedure;
                                         
                ProcedureName = procedure3.FullName;

           
                process = VmSolution.Instance[ProcedureName] as VmProcedure;
                this.vm.ModuleSource = process;
                this.DataContext = this.Status;

                ////////////////////////////////////////////////////////////////

            }
            catch (Exception ex)
            {
                UILogMangerHelper.Instance.AddLog(LogLevel.Error, "相机3窗体加载异常");
                GetLogHelper.VisionLog.Info("相机3窗体加载异常:  " + ex.ToString());
            }
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
                var result = new Camera3Result();
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
                    result.NgResultString = moduResult.GetOutputString("NgResultString").astStringVal[0].strValue;

                    var count = VmGlobalDataModel.Instance().WeldingCount;

     
                    if (count < 28)
                    {
                        float[] _FillZero= new float[28- count];


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
                    GetLogHelper.VisionLog.Error("相机3 接受VM数据异常" + ex.ToString());
                    UILogMangerHelper.Instance.AddLog(LogLevel.Error, "相机3 接受VM数据异常");

                }

                float[] _result = new float[1] { (float)result.CameraResult };


                //var len = VmGlobalDataModel.Instance().WeldingCount;
                var len = 28;
                float[] ZeroFill = new float[0];
                //A面
                var UpGlueData = result.OutY1.Concat(ZeroFill).Concat(result.OutY2).Concat(ZeroFill).Concat(result.OutY3).Concat(ZeroFill).Concat(result.OutY4).Concat(ZeroFill).ToArray();
               //B面
                var DownGlueData = result.OutY5.Concat(ZeroFill).Concat(result.OutY6).Concat(ZeroFill).Concat(result.OutY7).Concat(ZeroFill).Concat(result.OutY8).Concat(ZeroFill).ToArray();

                float[] Fill = new float[len];
                float[] Fill2 = new float[16];

   
                cellData.AddData(result.UpResult, result.DwResult,ref UpGlueData,ref DownGlueData,ref result.OutUpX[0],ref result.OutDwX[0]);

                float[] _UpResult = new float[1] { (float)result.UpResult };
                float[] _DwResult = new float[1] { (float)result.DwResult };

                var outData = UpGlueData.Concat(ZeroFill).Concat(DownGlueData).Concat(ZeroFill).Concat(Fill).Concat(ZeroFill).Concat(Fill).Concat(ZeroFill).Concat(result.OutUpX).Concat(ZeroFill).Concat(result.OutDwX).Concat(ZeroFill).Concat(_UpResult).Concat(ZeroFill).Concat(_DwResult).Concat(ZeroFill).Concat(Fill2).Concat(ZeroFill).Concat(_result).ToArray();
             
               
                

                #endregion

                Status.Ng_color = result.CameraResult > 0 ? MyColor.Ok_Color : MyColor.Ng_color;
                Status.Ng_text = result.CameraResult > 0 ? "OK" : "NG";

                var dt = DateTime.Now;



                #region Send PLC Data



                #if !TestDebug
                basePLC.WriteValue(PLCTagName.UpGlueCCD_Senddata, outData);
                Thread.Sleep(10);
                basePLC.WriteValue(PLCTagName.UpGlueCCD_Done, 1);
                basePLC.WriteValue(PLCTagName.Camera3_Status, PLCTagName.Ready);


                #endif

                #endregion


                CallBackStopWatch.Stop();

                var imagePath = VmHelper.SaveImage2(result.CameraResult, CurrentInfo.CameraDir.Camera3_FileDirName, dt.DateNowFormat(), ProcedureName, config, CurrentInfo.Config.cameraSettings.IsForceSaveOK);
                FileHelper.DeletedirByDay(FileHelper.Camera3_DeleteLock, config.cameraSettings.ImagePath, CurrentInfo.CameraDir.Camera3_FileDirName, config.cameraSettings.MaxSaveDays);


                #region 保存csv

                string a_status = result.UpResult > 0 ? $"OK   {result.UpResult}" : $"NG   {result.UpResult}";
                string b_status = result.DwResult > 0 ? $"OK   {result.DwResult}" : $"NG   {result.DwResult}";
                var src_path = FileHelper.CreateDirByTime(config.cameraSettings.ImagePath, CurrentInfo.CameraDir.Camera3_FileDirName, "Content");

                #region 赋值后的值（现在不用）
                //int length = UpGlueData.Length / 4;
                //result.OutY1 = UpGlueData.Skip(0 * length).Take(length).ToArray();
                //result.OutY2 = UpGlueData.Skip(1 * length).Take(length).ToArray();
                //result.OutY3 = UpGlueData.Skip(2 * length).Take(length).ToArray();
                //result.OutY4 = UpGlueData.Skip(3 * length).Take(length).ToArray();
                //result.OutY5 = DownGlueData.Skip(0 * length).Take(length).ToArray();
                //result.OutY6 = DownGlueData.Skip(1 * length).Take(length).ToArray();
                //result.OutY7 = DownGlueData.Skip(2 * length).Take(length).ToArray();
                //result.OutY8 = DownGlueData.Skip(3 * length).Take(length).ToArray();
                #endregion


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
                sb.Append("----------------------------------------------------------------------------" + "\r\n");

                FileHelper.WriteText(src_path + "\\content.txt", sb.ToString());

                #endregion


                result.OutMsg = sb.ToString();
                Data data = new Data();
                data.DataImagePath = imagePath;
                data.CreateteTime = dt;
                data.Msg = result.OutMsg;

                Status.Result3 = result;

                dataItemList.Add(data);
                SetDataLengthRange(dataItemList);


                //CameraHelper.ShowNgIcon2(process.Modules, dict, ModuleInfoDict);

                

            }
            catch (Exception ex)
            {
                GetLogHelper.VisionLog.Error("相机3执行异常" + ex.ToString());
                UILogMangerHelper.Instance.AddLog(LogLevel.Error, "相机3 VM 回调执行异常");
                Thread.Sleep(100);
            }
            finally
            {
                CameraState.Camera3_IsRunning = false;
                CallBackStopWatch.Stop();
                Status.TimeStatistic = Convert.ToInt32(CallBackStopWatch.ElapsedMilliseconds + process.ProcessTime + 36);//加相机执行时间
            }
        }



        public void ShowTreeView(IEnumerable<ModuInfo> ModuInfos, WorkFlowSorts WorkFlowSort, BasePLC plc, string processFullName)
        {
            try
            {

                this.basePLC = plc;
                var workFlowSort = WorkFlowSort;
                List<ModelBlock> list = new List<ModelBlock>();

                this.dict = Toolkits.CameraSequenceToDict(WorkFlowSort.camera3_Sequence);

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
                    child.Icon = Toolkits.getFileByName(item.strModuleName);
                    //  child.Icon = "/ICON/toolbar_logical_分支.png";
                    child.id = item.nModuleID.ToString();
                    child.Name = item.strModuleName;
                    child.processID = item.nProcessID.ToString();
                    child.fullName = processFullName;
                    ModuleInfoDict.Add(child.id, child);
                    list.Add(child);
                }


                ModelBlock node = new ModelBlock()
                {
                    DisplayName = "Camera3 上测",
                    Name = "This is the discription of Node1. This is a folder.",
                    Icon = "/ICON/toolbar_logical_分支.png",
                };

                ModelBlock node2 = new ModelBlock()
                {
                    DisplayName = "Camera3 下测",
                    Name = "This is the discription of Node1. This is a folder.",
                    Icon = "/ICON/toolbar_logical_分支.png",
                };


                var data = Toolkits.ParseCameraSequence(WorkFlowSort.camera3_Sequence);


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
                GetLogHelper.VisionLog.Error(ex);
            }
        }




        public void SetDataLengthRange(List<Data> itemList, int Maxlength = 80)
        {
            if (itemList.Count >= Maxlength)
            {
                if (itemList.Count > 11)
                    itemList.RemoveRange(0, 10);
            }

        }



        public void AddResultText(string name, string value)
        {
            sb.Append($"{name}: {value}" + "\r\n");

        }



        #region 点击事件

        /// <summary>
        /// NG历史
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_NgHistroy_Click(object sender, RoutedEventArgs e)
        {
            new NgImageHistory(3).ShowDialog();
        }

        private void Btn_DataHistroy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataHistoryWindow dataHistory = new DataHistoryWindow(3, dataItemList);
                SetDataHistory = dataHistory.AddData;
                dataHistory.ShowDialog();

                SetDataHistory = null;
            }
            catch (Exception ex)
            {


                MessageBox.Show("打开失败");


            }





        }

        private void TvProperties_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                
                var modul = (ModelBlock)this.tvProperties.SelectedItem;
                if (modul == null) return;
                string name = modul.fullName + "." + modul.RealName;


                //var state = VmHelper.OpenVmControlWindow(name)?.ShowDialog(); 
                var state = VmHelper.OpenVmControlWindowByOwner(name,this)?.ShowDialog(); 

                if (state == null)
                {

                    foreach (var item in GroupNmae)
                    {

                        string G_Name = $"{modul.fullName}.{item}.{modul.RealName}";

                        VmModule FindModule = (VmModule)VmSolution.Instance[G_Name];

                        if (FindModule == null) continue;
                        if (FindModule.ID == Convert.ToUInt16(modul.id))
                        {

                            VmHelper.OpenVmControlWindowByOwner(G_Name,this)?.ShowDialog(); 
                        }


                    }

                }
            }
            catch (Exception ex)
            {
                GetLogHelper.VisionLog.Error(ex.ToString());
            }
        }



        private void ButtonContinuExecute_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonStopExecute_Click(object sender, RoutedEventArgs e)
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
            var process = (VmProcedure)VmSolution.Instance[ProcedureName];
            if (null == process) return;
            process.Run();


        }




        #endregion







    }


    #region 模型



    public class Camera3WindowStatus : BaseWindowStatus
    {


        private Camera3Result result;

        public Camera3Result Result3
        {
            get { return result; }
            set
            {
                result = value;

                OnPropertyChanged("Result3");
            }
        }




    }



    /// <summary>
    /// 上点胶
    /// </summary>
    public class Camera3Result
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

        private string _ngResultString;

        public string NgResultString
        {
            get { return _ngResultString; }
            set { _ngResultString = value; }
        }


    }

    #endregion


   


}