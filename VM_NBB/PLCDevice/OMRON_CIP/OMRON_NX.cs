
using OMRON.Compolet.CIPCompolet64;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PLCDevice.OMRON_CIP
{
    public class OMRON_NX : BasePLC
    {


        public NXCompolet compolet=null;
        //public NJCompolet compolet2=null;

        public OMRON_NX()
        {
            //compolet= new NXCompolet();
            this.compolet = new OMRON.Compolet.CIPCompolet64.NXCompolet(null);
            //this.compolet2 =  new OMRON.Compolet.CIPCompolet64.NJCompolet();
        }

        public override void Connection(string ip, int port)
        {
            compolet.PeerAddress = ip;
            compolet.LocalPort = 2;
            compolet.ConnectionType = ConnectionType.UCMM;
            compolet.ReceiveTimeLimit = 750;
            compolet.Active = true;
        }

        public override bool IsConnected()
        {
            return compolet.IsConnected;
        }

        

        public override T LoadData<T>()
        {
            throw new NotImplementedException();
        }

        public override T ReadValue<T>(string name)
        {
           
                lock (_locker)
                {
                    var obj = compolet.ReadVariable(name);
                    return (T)obj;
                }
           
   
         
        }

        public override object ReadValue(string name)
        {
            throw new NotImplementedException();
        }

        public override Hashtable ReadVariableMultiple(string[] name)
        {
            return compolet.ReadVariableMultiple(name);
        }

        public override void SetClock(string time)
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            
        }

        public override  void WriteValue(string name, object value)
        {

            try
            {
                lock (_locker)
                {
                    compolet.WriteVariable(name, value);

                }

            }
            catch (Exception ex)
            {


            }

        }

        public override void WriteValue(string name, string value)
        {

            try
            {
                lock (_locker)
                {
                    compolet.WriteVariable(name, value);

                }

            }
            catch (Exception ex)
            {


            }

        }


      

    }
}
