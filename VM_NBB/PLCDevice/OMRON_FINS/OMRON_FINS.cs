using CommunicationTookit;
using PLCInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PLCDevice.OMRON_FINS
{
   public class OMRON_FINS : BasePLC
    {

        private const int TIME_COUNT = 2;
        private object objLock = new object();
        private object objLock_base = new object();

        public List<PLCAddress> adrsRead = new List<PLCAddress>();
        public List<PLCAddress> adrsWrite = new List<PLCAddress>();
        public List<PLCAddress> adrsAll = new List<PLCAddress>();

        private PLCAddress adrsHeart = null;
        private bool isConnected = false;
        private bool isHeartBeatStarted = false;
        private bool isDataMontiorStarted = false;
        private bool flgHeartBeat;
        private bool flgHeartBeatResult;
        private bool flgReadResult;
        private bool flgReady = false;
        private int flgReset = 0;
        private int flgFirst = 0;

        private string localIP;
        private string targetIP;
        private int localPort = 8000;
        private int targetPort;
        private int readTimeOut = 15;
        private int writeTimeOut = 5;

 
        private PlcCommunicationByTCP clientDataMontior = null;
        public PlcCommunicationByTCP clientCommon = null;
   
        private PlcCommunicationByTCP udpDataMontior = null;
        private PlcCommunicationByTCP udpCommon = null;

        private Task tskDataMontior;
        private Task tskDataMontior_Camera1;
 
        private CancellationTokenSource ctsDataMontior;
        private CancellationTokenSource ctsDataMontior_Camera1;


        private byte[] btHandShake;
        private byte[] btHandShakeRep;
        private byte[] btNodeHeart;
        private byte[] btNodeData;
        private byte[] btNodeCommon;

        private DateTime dtHeartStart;
        private DateTime dtHeartEnd;


        public OMRON_FINS()
        {

        }

        public OMRON_FINS(PlcCommunicationByTCP Client)
        {



        }


        public override void Connection(string ip, int port)
        {
            throw new NotImplementedException();
        }

     

        public override T LoadData<T>()
        {
            throw new NotImplementedException();
        }

        public override T ReadValue<T>(string name)
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void WriteValue(string name, string value)
        {
            throw new NotImplementedException();
        }

        public override void WriteValue(string name, object value)
        {
            throw new NotImplementedException();
        }

        public override bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public override object ReadValue(string name)
        {
            throw new NotImplementedException();
        }

        public override Hashtable ReadVariableMultiple(string[] name)
        {
            throw new NotImplementedException();
        }

        public override void SetClock(string time)
        {
            throw new NotImplementedException();
        }
    }
}
