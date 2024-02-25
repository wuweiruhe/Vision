using CoordinateTransformCs;
using GlobalCameraModuleCs;
using HikSaveImageTool;
using IMVSCalibBoardCalibModuCs;
using IMVSCalibTransformModuCs;
using IMVSImageCalibModuCs;
using IMVSImageCorrectCalibModuCs;
using SinglePointGrabModuCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Media;
using TranslationCalibModuCs;
using Vision_NBB.Log;
using Vision_NBB.Model;
using Vision_NBB.Toolkit;
using Vision_NBB.Utility;
using Vision_NBB.Views.Pages;
using VM.Controls.Primitives;
using VM.Core;
using VM.PlatformSDKCS;
using static SQLite.SQLite3;
using static Vision_NBB.Model.CurrentInfo;

namespace Vision_NBB.Toolkit
{


    public enum Trigger_Source
    {
        Line0 = 0,
        Line1 = 1,
        Line2 = 2,
        Line3 = 3,
        SOFTWARE = 7,
    }





    public class VmHelper
    {


        public static void LoadVmSolution(string path)
        {
            //VmSolution.Load(path, null);
            VmSolution.Load(path);
        }


        /// <summary>
        ///  打开vm 参数配置窗口
        /// </summary>
        /// <param name="VmTagName"></param>
        /// <returns></returns>
        public static ParamsConfigControl OpenVmControlWindow(string VmTagName)
        {


            try
            {
                VmModule FindModule = (VmModule)VmSolution.Instance[VmTagName];

                if (null == FindModule) return null;
                var paramForm = new ParamsConfigControl(VmTagName);


                paramForm.config.ModuleSource = FindModule;

                return paramForm;
            }
            catch (Exception ex)
            {


            }


            return null;


        }

        public static ParamsConfigControl OpenVmControlWindowByOwner(string VmTagName, System.Windows.DependencyObject userControl)
        {

            try
            {
                VmModule FindModule = (VmModule)VmSolution.Instance[VmTagName];

                if (null == FindModule) return null;
                var paramForm = new ParamsConfigControl(VmTagName);

                var parentWindown = FindParent<Window>(userControl);
                paramForm.Owner = parentWindown;
                paramForm.config.ModuleSource = FindModule;

                return paramForm;
            }
            catch (Exception ex)
            {


            }


            return null;


        }

        public static T FindParent<T>(System.Windows.DependencyObject child) where T : System.Windows.DependencyObject
        {
            System.Windows.DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            if (parentObject is T parent)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

        public static void LoadVmCalibFiles()
        {
            try
            {
                ////////////////////// 相机1////////////////////////////////////////////////
                var Camera1_Calib_Path = Get_Camera1_CalibFileName();
                if (!string.IsNullOrEmpty(Camera1_Calib_Path))
                    SetSinglePointGrabModulCaliPath(VMTagName.机械手定位抓取.单点抓取, Camera1_Calib_Path);
            





                ///////////////////////////相机2////////////////////////////////////////////
                var Camera2_Calib_Path = Get_Camera2_CalibFileName();
                if (!string.IsNullOrEmpty(Camera2_Calib_Path))
                    SetCalibTransformCaliPath(VMTagName.治具纠偏相机.标定转换1, Camera2_Calib_Path);







                //////////////////////////////相机3/////////////////////////////////////////
                var Camera3_Distortion_Path = Get_Camera3_Distortion_FileName();
                if (!string.IsNullOrEmpty(Camera3_Distortion_Path))
                    SetImageCorrectCalibPath(VMTagName.上点胶相机.畸变校正1, Camera3_Distortion_Path);






                /////////////////////////////////相机4//////////////////////////////////////
                var Camera4_Distortion_Path = Get_Camera4_Distortion_FileName();
                if (!string.IsNullOrEmpty(Camera4_Distortion_Path))
                    SetImageCorrectCalibPath(VMTagName.背点胶相机.畸变校正1, Camera4_Distortion_Path);






            }
            catch (Exception ex)
            {

                GetLogHelper.VisionLog.Error(ex);
                UILogMangerHelper.Instance.AddLog(LogLevel.Error, "加载标定文件失败");
                MessageBox.Show("加载标定文件失败");

            }

        }



        #region 畸变相关反法


        /// <summary>
        /// 设置畸变标定路径
        /// </summary>
        /// <param name="VmTagName"></param>
        /// <param name="CaliPath"></param>

        public static void SetImageCalibPath(string VmTagName, string CaliPath)
        {
            try
            {

                if (string.IsNullOrEmpty(VmTagName)) { MessageBox.Show("绑定名称不能为空"); return; }
                IMVSImageCalibModuTool CalibTool = VmSolution.Instance[VmTagName] as IMVSImageCalibModuTool;
                CalibTool.ModuParams.CalibPathName = CaliPath;
            }
            catch (Exception ex)
            {

                throw new Exception($"设置畸变标定路径 来自  {VmTagName} ：{CaliPath}");
            }






        }

        /// <summary>
        /// 导出畸变标定路径
        /// </summary>
        /// <param name="VmTagName"></param>
        /// <param name="CaliPath"></param>

        public static void ExportImageCalibFile(string VmTagName, string CaliPath)
        {
            try
            {
                if (string.IsNullOrEmpty(VmTagName)) { MessageBox.Show("绑定名称不能为空"); return; }

                IMVSImageCalibModuTool CalibTool = VmSolution.Instance[VmTagName] as IMVSImageCalibModuTool;

                CalibTool.ModuParams.DoSaveFile(CaliPath);

                CalibTool.ModuParams.CalibPathName=CaliPath;

                //CalibTool.SaveAs(CaliPath);
            }
            catch (Exception ex)
            {

                throw ex;
            }




        }


        /// <summary>
        /// 设置畸变修正路径
        /// </summary>
        /// <param name="VmTagName"></param>
        /// <param name="CaliPath"></param>

        public static void SetImageCorrectCalibPath(string VmTagName, string CaliPath)
        {

            try
            {
                if (string.IsNullOrEmpty(VmTagName)) { MessageBox.Show("绑定名称不能为空"); return; }
                IMVSImageCorrectCalibModuTool CalibTool = VmSolution.Instance[VmTagName] as IMVSImageCorrectCalibModuTool;
                CalibTool.ModuParams.CalibPath = CaliPath;
            }
            catch (Exception ex)
            {
                throw new Exception($"设置畸变修正路径 来自  {VmTagName} ：{CaliPath}");

            }


        }



        #endregion


        #region 相机触发相关方法


        /// <summary>
        ///   设置所有全局相机 硬触发
        /// </summary>
        public static void SetCameras_Line0Trigger()
        {
            try
            {
                List<GlobalCameraModuleTool> glCameralist = new List<GlobalCameraModuleTool>();
                List<VmModule> vmModules = new List<VmModule>();
                VmSolution.Instance.GetAllModule(vmModules);
                foreach (VmModule module in vmModules)
                {
                    if (module.GetType() == typeof(GlobalCameraModuleTool))
                    {
                        glCameralist.Add((GlobalCameraModuleTool)module);
                    }
                }


                foreach (GlobalCameraModuleTool item in glCameralist)
                {
                    item.ModuParams.TriggerSource = 0;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("无法设置所有全局相机触发");

            }



        }



        /// <summary>
        ///  设置所有全局相机 软触发
        /// </summary>

        public static void SetCameras_SoftwareTrigger()
        {

            try
            {
                List<GlobalCameraModuleTool> glCameralist = new List<GlobalCameraModuleTool>();
                List<VmModule> vmModules = new List<VmModule>();
                VmSolution.Instance.GetAllModule(vmModules);
                foreach (VmModule module in vmModules)
                {
                    if (module.GetType() == typeof(GlobalCameraModuleTool))
                    {
                        glCameralist.Add((GlobalCameraModuleTool)module);
                    }
                }

                foreach (GlobalCameraModuleTool item in glCameralist)
                {
                    item.ModuParams.TriggerSource = 7;
                }



            }
            catch (Exception ex)
            {
                throw new Exception("无法设置所有全局相机触发");

            }


        }

        public static int GetCamerasTriggerMode()
        {
            try
            {
                List<GlobalCameraModuleTool> glCameralist = new List<GlobalCameraModuleTool>();
                List<VmModule> vmModules = new List<VmModule>();
                VmSolution.Instance.GetAllModule(vmModules);
                foreach (VmModule module in vmModules)
                {
                    if (module.GetType() == typeof(GlobalCameraModuleTool))
                    {
                        glCameralist.Add((GlobalCameraModuleTool)module);
                    }
                }
                var mode = glCameralist[0].ModuParams.TriggerSource;
                if (mode >= 0) CurrentInfo.Config.cameraSettings.TriggerMode = mode;   // 
                return mode;

            }
            catch (Exception ex)
            {


                return -1;


            }

        }


        public static void SetGlobalCameraTrigger(string VmTagName, Trigger_Source mode)
        {
            try
            {
                GlobalCameraModuleTool cameraModuleTool = VmSolution.Instance[VmTagName] as GlobalCameraModuleTool;
                GlobalCameraParam globalCameraParam = cameraModuleTool.ModuParams;
                //var sss = cameraModuleTool.ModuParams.TriggerSource;
                cameraModuleTool.ModuParams.TriggerSource = (int)mode;

            }
            catch (Exception ex)
            {

                MessageBox.Show("设置全局相机触发失败");
            }


        }

        #endregion


        #region 标定相关方法



        /// <summary>
        /// 导出标定板标定文件
        /// </summary>
        /// <param name="boardCalibFile"></param>
        public static bool ExportBoardCalibFile(string VmTagName, string CaliPath)
        {
            try
            {
                if (string.IsNullOrEmpty(VmTagName)) { MessageBox.Show("绑定名称不能为空"); return false; }
                bool isExportSucessfull = false;


                IMVSCalibBoardCalibModuTool CalibTool = VmSolution.Instance[VmTagName] as IMVSCalibBoardCalibModuTool;
                int result = CalibTool.ModuParams.DoSaveFile(CaliPath);



                if (result == 0)
                {
                    CalibTool.ModuParams.CalibPathName = CaliPath;
                    isExportSucessfull = true;


                }




                return isExportSucessfull;


            }
            catch (Exception ex)
            {

                throw ex;
            }



        }




        public static bool ExportBoardCalibFile(string VmTagName, string CaliPath,ref float PixelScale)
        {
            try
            {
                if (string.IsNullOrEmpty(VmTagName)) { MessageBox.Show("绑定名称不能为空"); return false; }
                bool isExportSucessfull = false;


                IMVSCalibBoardCalibModuTool CalibTool = VmSolution.Instance[VmTagName] as IMVSCalibBoardCalibModuTool;
                int result = CalibTool.ModuParams.DoSaveFile(CaliPath);



                if (result == 0)
                {
                    CalibTool.ModuParams.CalibPathName = CaliPath;
                    isExportSucessfull = true;


                    PixelScale = CalibTool.ModuResult.PixelPrecision;

                }




                return isExportSucessfull;


            }
            catch (Exception ex)
            {

                throw ex;
            }



        }



        /// <summary>
        /// 清空平移旋转 标定状态
        /// </summary>
        /// <param name="VmTagName"></param>
        public static void ClearTranslationCalibStatus(string VmTagName)
        {
            TranslationCalibModuTool CalibTool = VmSolution.Instance[VmTagName] as TranslationCalibModuTool;
            CalibTool.ModuParams.DoClearPoint();

        }

        /// <summary>
        ///   设置平移旋转模块路径
        /// </summary>

        public static void SetTranslationCaliPath(string VmTagName, string CaliPath)
        {
            try
            {
                TranslationCalibModuTool CalibTool = VmSolution.Instance[VmTagName] as TranslationCalibModuTool;
                CalibTool.ModuParams.CalibPathName = CaliPath;

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }



        /// <summary>
        ///   设置标定转换标定文件路径
        /// </summary>
        /// <param name="VmTagName"></param>
        /// <param name="CaliPath"></param>
        public static void SetCalibTransformCaliPath(string VmTagName, string CaliPath)
        {

            try
            {
                IMVSCalibTransformModuTool CalibTool = VmSolution.Instance[VmTagName] as IMVSCalibTransformModuTool;

               // CalibTool.ModuParams.LoadCalibPath = CaliPath;


            }
            catch (Exception ex)
            {
                throw new Exception($"设置标定转换标定文件路径 来自  {VmTagName} ：{CaliPath}");

            }
        }

        /// <summary>
        /// 得到标定参数结果
        /// </summary>
        /// <param name="VmTagName"></param>
        /// <returns></returns>

        public static TranslationCalibResult GetCalibParamsResult(string VmTagName)
        {



            try
            {
                TranslationCalibModuTool CalibTool = VmSolution.Instance[VmTagName] as TranslationCalibModuTool;

                CalibTool.Run();

                Thread.Sleep(30);




                return CalibTool.ModuResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }





        }


        /// <summary>
        /// 导出平移旋转标定文件
        /// </summary>

        public static bool Export_TranslationCalib_File(string VmTagName, string PathName)
        {

            try
            {
                if (string.IsNullOrEmpty(VmTagName)) { MessageBox.Show("绑定名称不能为空"); return false; }
                bool isExportSucessfull = false;


                TranslationCalibModuTool CalibTool = VmSolution.Instance[VmTagName] as TranslationCalibModuTool;
                int result = CalibTool.ModuParams.DoSaveFile(PathName);



                if (result == 0)
                {
                    CalibTool.ModuParams.CalibPathName = PathName;
                    isExportSucessfull = true;


                }




                return isExportSucessfull;


            }
            catch (Exception ex)
            {

                throw ex;
            }



        }





        /// <summary>
        /// 设置单点抓取标定文件路径
        /// </summary>
        /// <param name="VmTagName"></param>
        /// <param name="CaliPath"></param>

        public static void SetSinglePointGrabModulCaliPath(string VmTagName, string CaliPath)
        {
            try
            {

                SinglePointGrabModuTool CalibTool2 = VmSolution.Instance[VmTagName] as SinglePointGrabModuTool;

                CalibTool2.ModuParams.FilePath = CaliPath;


                

            }
            catch (Exception ex)
            {

                throw new Exception($"单点抓取标定文件设置异常 来自  {VmTagName} ：{CaliPath}");
            }


        }

        #endregion


        #region 保存图片相关方法


        /// <summary>
        /// 保存Vm 流程源图
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="ImagePath"></param>
        public static void SaveOriginImage(string ProcedureName, string ImagePath)
        {
            bool cvColorFlag = true;
            //var imageBaseDataV2 = SaveImageTool.GetPrcOutputImage(ProcedureName);
            var imageBaseDataV2 = SaveImageTool.GetPrcOutputImage(ProcedureName, "ImageData2");
            if (imageBaseDataV2 == null)
            {
                GetLogHelper.VisionLog.Error("保存原图错误" + ProcedureName + ImagePath);
                return;
            }
            var renderDataIList = SaveImageTool.GetRenderDataList(ProcedureName);
            SaveImageTool.SaveOriginImage(ImagePath, SavePixelFormat.MONO_8, imageBaseDataV2, ref
            cvColorFlag, ratio: 1.0, jpgCompressRatio: 75);

        }



        /// <summary>
        /// 保存Vm 流程渲染图
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="ImagePath"></param>
        public static void SaveRenderImage(string ProcedureName, string ImagePath, int CompressRatio = 7)
        {
            bool cvColorFlag = true;
            //bool cvColorFlag = false;
            //var imageBaseDataV2 = SaveImageTool.GetPrcOutputImage(ProcedureName);
            var imageBaseDataV2 = SaveImageTool.GetPrcOutputImage(ProcedureName);

            if (imageBaseDataV2 == null)
            {
                GetLogHelper.VisionLog.Error("保存图错误" + ProcedureName + ImagePath);
                return;
            }
            var renderDataIList = SaveImageTool.GetRenderDataList(ProcedureName);
            SaveImageTool.SaveRenderImage(ImagePath, imageBaseDataV2, renderDataIList, ref  cvColorFlag, ratio: 1, jpgCompressRatio: CompressRatio, shapeRatioType: ShapeRatioType.ScreenRatio);

        }


        public static void SaveRenderImage2(string ProcedureName, string ImagePath, int CompressRatio = 7)
        {
            bool cvColorFlag = true;
            //bool cvColorFlag = false;
            //var imageBaseDataV2 = SaveImageTool.GetPrcOutputImage(ProcedureName);
            var imageBaseDataV2 = SaveImageTool.GetPrcOutputImage(ProcedureName, "ImageData2");

            if (imageBaseDataV2 == null)
            {
                GetLogHelper.VisionLog.Error("保存图错误" + ProcedureName + ImagePath);
                return;
            }
            var renderDataIList = SaveImageTool.GetRenderDataList(ProcedureName);
            SaveImageTool.SaveRenderImage(ImagePath, imageBaseDataV2, renderDataIList, ref cvColorFlag, ratio: 1, jpgCompressRatio: CompressRatio, shapeRatioType: ShapeRatioType.ScreenRatio);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProcedureName"></param>
        /// <param name="ImageName"></param>
        /// <param name="NgPath"></param>
        /// <param name="OKPath"></param>
        private static void SaveVmImage(string ProcedureName, string ImageName, string NgPath, string OKPath)
        {
            if (string.IsNullOrEmpty(NgPath) && string.IsNullOrEmpty(OKPath)) return;


            if (!string.IsNullOrEmpty(NgPath))
            {
                var NgSrcPath = System.IO.Path.Combine(NgPath, "src\\" + ImageName + ".jpg");
                var NgRenderPath = System.IO.Path.Combine(NgPath, ImageName + ".jpg");


                VmHelper.SaveOriginImage(ProcedureName, NgSrcPath);
                VmHelper.SaveRenderImage(ProcedureName, NgRenderPath);
                //Task SaveNGSrcImageTask = Task.Run(() => { VmHelper.SaveOriginImage(ProcedureName, NgSrcPath); });
                //Task SaveNgImageTask = Task.Run(() => { VmHelper.SaveRenderImage(ProcedureName, NgRenderPath); });
                //Task.WaitAll(SaveNGSrcImageTask, SaveNgImageTask);
            }
            else
            {
                if (string.IsNullOrEmpty(OKPath)) return;

                var OKSrcPath = System.IO.Path.Combine(OKPath, ImageName + ".jpg");
                var OkRenderPath = System.IO.Path.Combine(OKPath, "Render\\" + ImageName + ".jpg");

                VmHelper.SaveOriginImage(ProcedureName, OKSrcPath);
                VmHelper.SaveRenderImage(ProcedureName, OkRenderPath);

                //Task SaveOkSrcImageTask = Task.Run(() => { VmHelper.SaveOriginImage(ProcedureName, OKSrcPath); });
                //Task SaveOkRenderImageTask = Task.Run(() => { VmHelper.SaveRenderImage(ProcedureName, OkRenderPath); });
                //Task.WaitAll(SaveOkSrcImageTask, SaveOkRenderImageTask);
            }







        }


        private static void SaveVmImage2(string ProcedureName, string ImageName, string NgPath, string OKPath,bool isSaveRenderOnly=false)
        {
            if (string.IsNullOrEmpty(NgPath) && string.IsNullOrEmpty(OKPath)) return;


            if (!string.IsNullOrEmpty(NgPath))
            {
                var NgSrcPath = System.IO.Path.Combine(NgPath, "src\\" + ImageName + ".jpg");
                var NgRenderPath = System.IO.Path.Combine(NgPath, ImageName + ".jpg");


              VmHelper.SaveOriginImage(ProcedureName, NgSrcPath);
                VmHelper.SaveRenderImage2(ProcedureName, NgRenderPath);
                //Task SaveNGSrcImageTask = Task.Run(() => { VmHelper.SaveOriginImage(ProcedureName, NgSrcPath); });
                //Task SaveNgImageTask = Task.Run(() => { VmHelper.SaveRenderImage(ProcedureName, NgRenderPath); });
                //Task.WaitAll(SaveNGSrcImageTask, SaveNgImageTask);
            }
            else
            {
                if (string.IsNullOrEmpty(OKPath)) return;

                var OKSrcPath = System.IO.Path.Combine(OKPath, ImageName + ".jpg");
                var OkRenderPath = System.IO.Path.Combine(OKPath, "Render\\" + ImageName + ".jpg");

                if (isSaveRenderOnly == false)
              VmHelper.SaveOriginImage(ProcedureName, OKSrcPath);
                VmHelper.SaveRenderImage2(ProcedureName, OkRenderPath);

                //Task SaveOkSrcImageTask = Task.Run(() => { VmHelper.SaveOriginImage(ProcedureName, OKSrcPath); });
                //Task SaveOkRenderImageTask = Task.Run(() => { VmHelper.SaveRenderImage(ProcedureName, OkRenderPath); });
                //Task.WaitAll(SaveOkSrcImageTask, SaveOkRenderImageTask);
            }







        }



        /// </summary>
        /// <param name="isOKImage">流程NG 状态</param>
        /// <param name="CameraDirName">相机目录文件名</param>
        /// <param name="imageName">保存图片名字 建议用时间作为图像名字</param>
        /// <param name="ProcedureName">流程名字</param>
        /// <param name="config">UI系统配置</param>

        public static string SaveImage(int isOKImage, string CameraDirName, string imageName, string ProcedureName, Configuration config ,bool isForceSaveOK=false)
        {

            try
            {

               // FileHelper.DeletedirByDay(config.cameraSettings.ImagePath, CameraDirName);

                //var imageName = DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss");
                string NgPath = "";
                string OkPath = "";




                if (isOKImage > 0)//ok图片   
                {

                    if (config.cameraSettings.OkImageSave|| isForceSaveOK)
                    {

                        OkPath = FileHelper.GetSaveImagePathByTime(config.cameraSettings.ImagePath, CameraDirName, "OK");

                       SaveVmImage(ProcedureName, imageName, NgPath, OkPath);

                    }
                }
                else  //NG图片
                {
                    if (config.cameraSettings.NGImageSave)
                    {
                        NgPath = FileHelper.GetSaveImagePathByTime(config.cameraSettings.ImagePath, CameraDirName, "NG");

                        VmHelper.SaveVmImage(ProcedureName, imageName, NgPath, OkPath);

                    }
                }


                if(!string.IsNullOrEmpty(NgPath)) return NgPath;
                if(!string.IsNullOrEmpty(OkPath)) return OkPath;


                return "";


            }
            catch (Exception ex)
            {
              
                GetLogHelper.VisionLog.Error(ex.ToString());
                UILogMangerHelper.Instance.AddLog(LogLevel.Error, "图像保存失败");
                return "";
            }

        }



        public static string SaveImage2(int isOKImage, string CameraDirName, string imageName, string ProcedureName, Configuration config, bool isForceSaveOK = false)
        {

            try
            {

             //   FileHelper.DeletedirByDay(config.cameraSettings.ImagePath, CameraDirName);

                //var imageName = DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss");
                string NgPath = "";
                string OkPath = "";




                if (isOKImage > 0)//ok图片   
                {

                    if (config.cameraSettings.OkImageSave|| isForceSaveOK)
                    {

                        OkPath = FileHelper.GetSaveImagePathByTime(config.cameraSettings.ImagePath, CameraDirName, "OK");

                        SaveVmImage2(ProcedureName, imageName, NgPath, OkPath,config.cameraSettings.IsSaveRenderImageOnly);

                    }
                }
                else  //NG图片
                {
                    if (config.cameraSettings.NGImageSave)
                    {
                        NgPath = FileHelper.GetSaveImagePathByTime(config.cameraSettings.ImagePath, CameraDirName, "NG");

                        SaveVmImage2(ProcedureName, imageName, NgPath, OkPath);

                    }
                }


                if (!string.IsNullOrEmpty(NgPath)) return NgPath;
                if (!string.IsNullOrEmpty(OkPath)) return OkPath;


                return "";


            }
            catch (Exception ex)
            {

                GetLogHelper.VisionLog.Error(ex.ToString());
                UILogMangerHelper.Instance.AddLog(LogLevel.Error, "图像保存失败");
                return "";
            }

        }

        #endregion


        #region 获得标定文件路径方法


        public static string Get_Camera1_CalibFileName()
        {

            var Camera1CalibPath = FileHelper.GetCalibFileNameByPath(CurrentInfo.CalibDir.Camera1FirstCailPath);

            if (string.IsNullOrEmpty(Camera1CalibPath))
            {

                GetLogHelper.VisionLog.Error("相机1 加载标定路径为空" + Camera1CalibPath);

                UILogMangerHelper.Instance.AddLog(LogLevel.Error, "相机1 加载标定路径为空");
            }

            return Camera1CalibPath;

        }

        public static string Get_Camera2_CalibFileName()
        {


            var Camera1CalibPath = FileHelper.GetCalibFileNameByPath(CurrentInfo.CalibDir.Camera2FirstCailPath,"iwcal");
            if (string.IsNullOrEmpty(Camera1CalibPath))
            {

                GetLogHelper.VisionLog.Error("相机2 加载标定路径为空" + Camera1CalibPath);

                UILogMangerHelper.Instance.AddLog(LogLevel.Error, "相机2 加载标定路径为空");
            }
            return Camera1CalibPath;

        }

        public static string Get_Camera3_CalibFileName()
        {
            var Camera1CalibPath = FileHelper.GetCalibFileNameByPath(CurrentInfo.CalibDir.Camera3FirstCailPath);
            if (string.IsNullOrEmpty(Camera1CalibPath))
            {

                GetLogHelper.VisionLog.Error("相机3 加载标定路径为空" + Camera1CalibPath);
                UILogMangerHelper.Instance.AddLog(LogLevel.Error, "相机3 加载标定路径为空");
            }
            return Camera1CalibPath;

        }


        public static string Get_Camera3_Distortion_FileName()
        {
            var Camera1CalibPath = FileHelper.GetDistortionFileNameByPath(CurrentInfo.CalibDir.Camera3DistortionCailPath);

            if (string.IsNullOrEmpty(Camera1CalibPath))
            {

                GetLogHelper.VisionLog.Error("相机3 加载 畸变标定路径为空" + Camera1CalibPath);
                UILogMangerHelper.Instance.AddLog(LogLevel.Error, "相机3 加载 畸变标定路径为空");
            }
            return Camera1CalibPath;
        }

        public static string Get_Camera4_Distortion_FileName()
        {
            var Camera1CalibPath = FileHelper.GetDistortionFileNameByPath(CurrentInfo.CalibDir.Camera4DistortionCailPath);
            if (string.IsNullOrEmpty(Camera1CalibPath))
            {

                GetLogHelper.VisionLog.Error("相机4 加载 畸变标定路径为空" + Camera1CalibPath);
                UILogMangerHelper.Instance.AddLog(LogLevel.Error, "相机4 加载 畸变标定路径为空");
            }

            return Camera1CalibPath;
        }


        public static string Get_Camera4_CalibFileName()
        {
            var Camera1CalibPath = FileHelper.GetCalibFileNameByPath(CurrentInfo.CalibDir.Camera4FirstCailPath);
            if (string.IsNullOrEmpty(Camera1CalibPath))
            {

                GetLogHelper.VisionLog.Error("相机4 加载标定路径为空" + Camera1CalibPath);

                UILogMangerHelper.Instance.AddLog(LogLevel.Error, "相机4 加载 加载标定路径为空");
            }
            return Camera1CalibPath;

        }



        #endregion

      








    }





   

}

