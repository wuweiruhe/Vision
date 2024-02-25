using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision_NBB.Log;
using Vision_NBB.Toolkit;

namespace Vision_NBB.Model
{
    public static class CurrentInfo
    {
        public static string Machine { get; set; }
        public static string CurrentUser { get; set; }

        public static string MachineOrientation { get; set; } = "A";
        public static Dictionary<string, string> ParamDict = new Dictionary<string, string>();

        public static string ParamPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "\\Config\\ParamConfig.txt";



        public static string ModuleInfoPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "ModuleInfo\\";

        public static Configuration Config;

        public static int RobotCalibrationStep = 0;


        /// <summary>
        ///  vm 单个点胶 PLC最大数组长度
        /// </summary>
        public static int WeldingArrayLen = 30;

        public class CalibDir
        {
            public static string Camera1FirstCailPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "CailData\\Camera1\\";
            public static string Camera2FirstCailPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "CailData\\Camera2\\";
            public static string Camera3FirstCailPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "CailData\\Camera3\\";
            public static string Camera3DistortionCailPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "CailData\\Camera3\\Distortion\\";
            public static string Camera4FirstCailPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "CailData\\Camera4\\";
            public static string Camera4DistortionCailPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "CailData\\Camera4\\Distortion\\";


        }


        public class CameraDir
        {
            public static string Camera1_FileDirName = "camera1";
            public static string Camera2_FileDirName = "camera2";
            public static string Camera3_FileDirName = "camera3";
            public static string Camera4_FileDirName = "camera4";
        }


        static CurrentInfo()
        {
            Machine = " Stringer Vision";


        }



        public static string Gen_Camera1_CalibFileName()
        {
            return CalibDir.Camera1FirstCailPath + "Camera1_"+DateTime.Now.CaliFormat()+".xml";



        }

        public static string Gen_Camera2_CalibFileName()
        {
            return CalibDir.Camera2FirstCailPath +"Camera2_"+ DateTime.Now.CaliFormat() + ".iwcal";

        }

        public static string Gen_Camera3_CalibFileName()
        {
            return CalibDir.Camera3FirstCailPath + "Camera3_"+DateTime.Now.CaliFormat() + ".iwcal";

        }


        public static string Gen_Camera3_Distortion_FileName()
        {
            return CalibDir.Camera3DistortionCailPath + "Camera3_distortion_"+DateTime.Now.CaliFormat() + ".iccal";

        }

        public static string Gen_Camera4_Distortion_FileName()
        {
            return CalibDir.Camera4DistortionCailPath + "Camera4_distortion_" +DateTime.Now.CaliFormat() + ".iccal";

        }



        public static string Gen_Camera4_CalibFileName()
        {
            return CalibDir.Camera4FirstCailPath +"Camera4_"+ DateTime.Now.CaliFormat() + ".iwcal";

        }



       



    }
}
