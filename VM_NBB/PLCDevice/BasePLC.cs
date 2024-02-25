using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCDevice
{
    public abstract class BasePLC : IPLC
    {
        public delegate void PlcDataReceivedHandler<T>(T data);
        private bool isConnected = false;
        //public event PlcDataReceivedHandler PlcDataReceived;
        public object _locker = new object();
        private string targetIP;
     
        private int targetPort;
        public string TargetIP
        {
            get { return targetIP; }
            set { targetIP = value; }
        }
        public int TargetPort
        {
            get { return targetPort; }
            set { targetPort = value; }
        }
        public abstract void Connection(string ip, int port);
     
        public abstract T LoadData<T>();
        public abstract T ReadValue<T>(string name);
        public abstract object ReadValue(string name);
        public abstract Hashtable ReadVariableMultiple(string[] name);
        public abstract void Start();
        public abstract void WriteValue(string name, string value);
        public abstract bool IsConnected();

        public abstract void WriteValue(string name, object value);

       public abstract void SetClock(string time);

    }
}
