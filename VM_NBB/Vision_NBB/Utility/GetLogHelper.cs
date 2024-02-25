using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4Net.config", Watch = true)]
namespace Vision_NBB.Utility
{
    public class GetLogHelper
    {
        public static readonly ILog objLog = LogManager.GetLogger("logger1");
        public static readonly ILog VisionLog = LogManager.GetLogger("logger2");
      
    }
}
