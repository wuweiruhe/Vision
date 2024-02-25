using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCDevice
{
    public interface IPLC
    {
       
        T ReadValue<T>(string name);

        void WriteValue(string name, string value);
        //void WriteValue(string name, object value);

        void Connection(string ip, int port);

        T LoadData<T>();

        void Start();

         bool IsConnected();


     



    }
}
