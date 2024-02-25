using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationTookit
{
    public class PLCDefine
    {
        #region const

        public const int HEART_BEAT_INTERVAL = /*60000*/1000;

        // ----------  Camera1  ----------
        public const string R_3000 = "3000";//触发信号

        public const string R_3002 = "3002";


        public const string W_3001 = "3001";//相机写入相机状态地址,如Ready信号
        public const string W_3002 = "3002"; //相机发送算法处理结果地址：NG/OK状态"
        public const string W_3004 = "3004";//数据
        public const string W_3006 = "3006";
        public const string W_3008 = "3008";
        public const string W_3010 = "3010";
        public const string W_3012 = "3012";
        public const string W_3014 = "3014";
        public const string W_3016 = "3016";
        public const string W_3018 = "3018";
        public const string W_3020 = "3020";
        public const string W_3022 = "3022";
        public const string W_3024 = "3024";
        public const string W_3026 = "3026";
        public const string W_3028 = "3028";
        public const string W_3030 = "3030";
        public const string W_3032 = "3032";
        public const string W_3034 = "3034";
        public const string W_3036 = "3036";
        public const string W_3038 = "3038";



        // ----------  Camera2  ----------

        public const string R_4000 = "4000";//触发信号

        public const string R_4002 = "4002";


        public const string W_4001 = "4001";//相机写入相机状态地址,如Ready信号
        public const string W_4002 = "4002"; //相机发送算法处理结果地址：NG/OK状态"
        public const string W_4004 = "4004";//数据
        public const string W_4006 = "4006";
        public const string W_4008 = "4008";
        public const string W_4010 = "4010";
        public const string W_4012 = "4012";
        public const string W_4014 = "4014";
        public const string W_4016 = "4016";
        public const string W_4018 = "4018";
        public const string W_4020 = "4020";
        public const string W_4022 = "4022";
        public const string W_4024 = "4024";
        public const string W_4026 = "4026";
        public const string W_4028 = "4028";
        public const string W_4030 = "4030";
        public const string W_4032 = "4032";
        public const string W_4034 = "4034";
        public const string W_4036 = "4036";
        public const string W_4038 = "4038";


        //   ------------Camera3----------------


        public const string R_5000 = "5000";//触发信号

        public const string R_5002 = "5002";



        public const string W_5001 = "5001";//相机写入相机状态地址,如Ready信号
        public const string W_5002 = "5002"; //相机发送算法处理结果地址：NG/OK状态"
        public const string W_5004 = "5004";//数据
        public const string W_5006 = "5006";
        public const string W_5008 = "5008";
        public const string W_5010 = "5010";
        public const string W_5012 = "5012";
        public const string W_5014 = "5014";
        public const string W_5016 = "5016";
        public const string W_5018 = "5018";
        public const string W_5020 = "5020";
        public const string W_5022 = "5022";
        public const string W_5024 = "5024";
        public const string W_5026 = "5026";
        public const string W_5028 = "5028";
        public const string W_5030 = "5030";
        public const string W_5032 = "5032";
        public const string W_5034 = "5034";
        public const string W_5036 = "5036";
        public const string W_5038 = "5038";


        //-----------------statau------------------------------------------
        //public const string W_3001_Ready  = "1";
        //public const string W_3001_Busy   = "2";
        //public const string W_3001_Error  = "4";
        //public const string W_3001_Result = "8";


        public const string W_Ready = "1";
        public const string W_Busy = "2";
        public const string W_Error = "4";
        public const string W_Result = "8";



        public const string TriggerString = "T1";
        public const string Space = " ";
        public const string CR = "\r";
        public const string LF = "\n";
        public const string CRLF = "\r\n";

        public const byte BT_F = 0x46;
        public const byte BT_I = 0x49;
        public const byte BT_N = 0x4E;
        public const byte BT_S = 0x53;

        public const byte BT_ICF_C = 0x80;
        public const byte BT_ICF_R = 0xC0;


        #endregion
    }


    public class CommunicationMessage
    {
        private readonly DateTime creationTime = new DateTime();

        public DateTime CreationTime
        {
            get { return creationTime; }
        }

        public string Message { get; set; }
        public string CommAddressName { get; set; }

        public CommunicationMessage()
        {
            creationTime = DateTime.Now;
        }

        public CommunicationMessage(string message, string adresName) : this()
        {
            Message = message;
            CommAddressName = adresName;
        }
    }

    public enum CellType
    {
        DM = 1,
        EM = 2,
        FM = 3,
        R = 6,
        MR = 7,
    }

    public enum MessageType
    {
        ASCII = 0,
        Binary = 1,
    }

    public enum CommandType
    {
        Read = 0,
        Write = 1,
    }

    public enum ValueType
    {
        INT16 = 1,
        UINT16 = 2,
        INT32 = 3,
        UINT32 = 4,
        HEX = 5
    }

    public enum TCPType
    {
        HeartBeat = 1,
        DataMontior = 2,
        AlignUp = 3,
        AlignDown = 4,
        Common = 5,
    }

}
