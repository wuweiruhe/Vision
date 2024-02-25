using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision_NBB.Log
{
    public enum UILogLevel
    {
        Trace = 1,
        Debug,
        Infor,
        Warn,
        Error,
    }

    public class UILogEntity
    {
        private string content;
        private string time;
        private string userName;

        private int errorCode;
        private UInt64 rowIndex;

        private UILogLevel level;

        public string Content
        {
            get { return content; }
        }

        public string Time
        {
            get { return time; }
        }

        public UILogLevel Level
        {
            get { return level; }
        }

        //public string UserName
        //{
        //    get { return userName; }
        //}

        //public int ErrorCode
        //{
        //    get { return errorCode; }
        //}

        //public UInt64 RowIndex
        //{
        //    get
        //    {
        //        return rowIndex;
        //    }
        //}

        public UILogEntity(string logContent, UILogLevel logLevel, UInt64 index)
        {
            time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            content = logContent;
            level = logLevel;
            userName = "Default";
            errorCode = 0;
            rowIndex = index;
        }

        public UILogEntity(string logContent, UILogLevel logLevel, int errorCode, string userName, UInt64 index)
        {
            time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            content = logContent;
            level = logLevel;
            this.errorCode = errorCode;
            if (string.IsNullOrWhiteSpace(userName))
            {
                this.userName = "Default";
            }
            else
            {
                this.userName = userName;
            }
            rowIndex = index;
        }

        public override string ToString()
        {
            return userName + "_" + errorCode.ToString() + "_" + level.ToString() + "_" + content;
        }

    }
}
