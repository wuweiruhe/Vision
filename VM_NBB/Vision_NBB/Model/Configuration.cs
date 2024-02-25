using Apps.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Vision_NBB.Utility;

namespace Vision_NBB.Model
{
    public class Configuration
    {
        public const string fileName = "Config.xml";
        public static string fileDir = AppDomain.CurrentDomain.BaseDirectory + "\\Config\\";
        public static string filePath;

        /// <summary>
        /// 方案路径
        /// </summary>
        public string SoluctionPath { get; set; }

        /// <summary>
        /// 方案名
        /// </summary>
        public string SoluctionName { get; set; }

        /// <summary>
        /// PLC IP地址
        /// </summary>
        public string PLC_IP { get; set; }

        /// <summary>
        /// PLC Port
        /// </summary>
        public string PLC_Port { get; set; }

        /// <summary>
        /// 当前登录名
        /// </summary>
        public string LoginUserName { get; set; }



     


        public CameraSettings cameraSettings;
        public WorkFlowSorts workFlowSorts;
        public NPointCalibParam nPointCalibParam;
      //  public VmGlobalDataModel vmGlobalDataModel;

        public Configuration()
        {
           CreateParams();
        }

        public static bool Load(out Configuration config)
        {
            filePath = fileDir + fileName;
            config = null;
            if (!File.Exists(filePath))
            {
                 GetLogHelper.VisionLog.Error("Config cannot be found");
                return false;
            }
            try
            {
                config = null;
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    fs.Position = 0;//重置流位置
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(Configuration));
                    config = xmlFormat.Deserialize(fs) as Configuration;
                }
                if (config == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                 GetLogHelper.VisionLog.Error(ex.Message);
                return false;
            }
        }

        public static bool Save(Configuration config)
        {
            if (!Directory.Exists(fileDir))
            {
                Directory.CreateDirectory(fileDir);
            }
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    Encoding = Encoding.UTF8,
                    OmitXmlDeclaration = false,
                    NewLineOnAttributes = false,
                    Indent = true,
                    IndentChars = "    ",
                    ConformanceLevel = ConformanceLevel.Document
                };
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    using (var writer = XmlWriter.Create(fs, settings))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
                        xmlSerializer.Serialize(writer, config, ns);
                    }
                }

                 GetLogHelper.VisionLog.Info("Save config file");
                return true;
            }
            catch (Exception ex)
            {
                 GetLogHelper.VisionLog.Error(ex.Message);
                return false;
            }
        }


        public void CreateParams()
        {           
            SoluctionPath = "C:\\Users\\LMJ\\Desktop\\NBB\\28BB新片子 - 副本.sol";
            SoluctionName = "C:\\Users\\LMJ\\Desktop\\NBB\\28BB新片子 - 副本.sol";
            PLC_IP = "192.168.250.1";
            PLC_Port = "2";
            LoginUserName = "Admin";
            cameraSettings = new CameraSettings();
            workFlowSorts = new WorkFlowSorts();
            nPointCalibParam = new NPointCalibParam();
       //     vmGlobalDataModel = new VmGlobalDataModel();
        }
    }


    /// <summary>
    /// 相机设置
    /// </summary>
    public class CameraSettings
    {
        /// <summary>
        /// 图片保存路径
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// OK图像是否保存
        /// </summary>

        public bool OkImageSave { get; set; }


        /// <summary>
        /// 只保存Ok 时的渲染图  不保存原图
        /// </summary>
        public bool IsSaveRenderImageOnly { get; set; }

        public bool IsForceSaveOK { get; set; }

        /// <summary>
        /// NG图像是否保存
        /// </summary>

        public bool NGImageSave { get; set; }


        /// <summary>
        /// 最大保存天数
        /// </summary>
        public int MaxSaveDays {get; set; }

        /// 1  line0  2  software
        /// </summary>
        public int TriggerMode { get; set; } = 1;

        public CameraSettings()
        {
            ImagePath = "C:\\Users\\LMJ\\Desktop\\NBB\\Image";
            OkImageSave = false;
            NGImageSave = false;
            MaxSaveDays = 30;
        }
    }


    /// <summary>
    /// 相机流程分组
    /// </summary>
    public class WorkFlowSorts
    {

        public string camera1_NO { get; set; }


        public string camera2_NO { get; set; }



        public string camera3_NO { get; set; }


        public string camera4_NO { get; set; }



        public string camera1_Sequence { get; set; }


        public string camera2_Sequence { get; set; }


        public string camera3_Sequence { get; set; }


        public string camera4_Sequence { get; set; }



        public WorkFlowSorts()
        {
            camera1_NO = "1";
            camera2_NO = "2";
            camera3_NO = "3";
            camera4_NO = "4";
            camera1_Sequence = "0,90,3,35,34,24,37,36,2,14,16,18,38,4,26,5,284,46";
            camera2_Sequence = "17,48,312,313,28,29,51";
            camera3_Sequence = "368,8,283,206,52";
            camera4_Sequence = "11,13,15,19,12,53";
        }


        public string getCameras()
        {

            return camera1_NO + ";" + camera2_NO + ";" + camera3_NO + ";" + camera4_NO;
        }



        public string getCamera_Sequence()
        {
            return camera1_Sequence + ";" + camera2_Sequence + ";" + camera3_Sequence + ";" + camera4_Sequence;
        }
    }



}
