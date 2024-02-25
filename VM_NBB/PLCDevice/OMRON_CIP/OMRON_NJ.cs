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
    public class OMRON_NJ : BasePLC
    {    
        public NJCompolet compolet = null;

        public OMRON_NJ()
        {
            this.compolet = new OMRON.Compolet.CIPCompolet64.NJCompolet(null);
        }

        public override void Connection(string ip, int port)
        {
            compolet.Active = false;

            Thread.Sleep(200);
            compolet.PeerAddress = ip;
            //Console.WriteLine(compolet.PeerAddress);
            compolet.LocalPort = 2;
            compolet.ConnectionType = ConnectionType.UCMM;
            compolet.ReceiveTimeLimit = 1000;
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
                return (T)compolet.ReadVariable(name);

            }
         



        }

        public override object ReadValue(string name)
        {
           
                return compolet.ReadVariable(name);
               
            
        }

        public override Hashtable ReadVariableMultiple(string[] name)
        {
            return compolet.ReadVariableMultiple(name);
        }

        public override void SetClock(string time)
        {
            compolet.Clock = DateTime.Parse(time);
        }

        public override void Start()
        {

        }

        public override void WriteValue(string name, object value)
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

                throw ex;

            }
        }

        public override void WriteValue(string name, string value)
        {

            try
            {
             
                    compolet.WriteVariable(name, value);

                

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}
