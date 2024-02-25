using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision_NBB.Model
{




    public class CameraState
    {
        public static bool Camera1_IsRunning = false;
        public static bool Camera3_IsRunning = false;
        public static bool Camera4_IsRunning = false;



    }
    public class PLCTagName
    {
        /// <summary>
        /// PLC心跳
        /// </summary>
        public const string PLC_HeartBeat = "robot_CCD.heartbeat_PC";


      


        /// <summary>
        /// camera1
        /// </summary>
        public const string Camera1_Trigger = "";
        public const string Camera1_Result = "";
        public const string Camera1_Status = "CCD_Status_Robot";




        /// <summary>
        /// camera2
        /// </summary>
        public const string Camera2_Trigger = "";
        public const string Camera2_Result = "";
        public const string Camera2_Status = "";

        /// <summary>
        /// camera3
        /// </summary>
        public const string Camera3_Trigger = "";
        public const string Camera3_Result = "";
        public const string Camera3_Status = "CCD_Status_UpGlue";

        /// <summary>
        /// camera4
        /// </summary>
        public const string Camera4_Trigger = "";
        public const string Camera4_Result = "";
        public const string Camera4_Status = "CCD_Status_BackGlue";


        public const int Busy = 0;
        public const int Ready = 1;


        //////////////////////////////////////////////////////////////

        /// <summary>
        /// 标定触发信号
        /// </summary>
        public const string Robot_Trigger = "Robot_mark_Trig";//robot_CCD.data9（PLC->CCD）

        /// <summary>
        /// 标定数据标签 发送
        /// </summary>
        public const string Robot_calib_Senddata = "robot_CCD.data8"; //robot_CCD.data8（CCD->PLC）

        /// <summary>
        /// 标定数据标签 读取
        /// </summary>
        public const string Robot_calib_Readdata = "robot_CCD.data9";//"robot_CCD.TRIG"; //robot_CCD.data9（PLC->CCD）

        /// <summary>
        ///标定完成
        /// </summary>
        public const string Robot_Finished = "Robot_mark_done";


        public const string CamaraDoneRobotLable = "robot_CCD.done";//


        public const string FixtureDoneLable = "Fixture_CCD.done";//

        /// <summary>
        /// 标定数据标签 发送
        /// </summary>
        public const string FixtureCCD_Senddata = "Fixture_CCD.data8"; //robot_CCD.data8（CCD->PLC）




        //数据标签名：UpGlue_CCD.data0

        /// <summary>
        /// 标定数据标签 发送
        /// </summary>
        public const string UpGlueCCD_Senddata = "UpGlue_CCD.data0"; //


        //完成标签名：UpGlue_CCD.done

        public const string UpGlueCCD_Done = "UpGlue_CCD.done"; //

        //数据标签名：UpGlue_CCD.data0

        /// <summary>
        /// 标定数据标签 发送
        /// </summary>
        public const string BackGlueCCD_Senddata = "Back_A_CCD.data0"; //


        //完成标签名：UpGlue_CCD.done

        public const string BackGlueCCD_Done = "Back_A_CCD.done"; //


    }
}
