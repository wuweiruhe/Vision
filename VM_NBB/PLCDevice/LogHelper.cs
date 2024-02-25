/***************************************************

***************************************************/

using log4net;
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4Net.config", Watch = true)]

namespace PLC
{
    public class LogHelper
    {
        public static readonly ILog objLog = LogManager.GetLogger("logger1");

        public static readonly ILog SecsLog = LogManager.GetLogger("logger2");
    }
}
