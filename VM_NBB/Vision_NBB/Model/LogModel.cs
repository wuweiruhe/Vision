using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision_NBB.Model
{
    public enum LogLevel
    {
        Trace = 5,
        Debug = 4,
        Info = 3,
        Warn = 2,
        Error = 1,

    }
    public class LogModel
    {
        private string _time;
        public string Time
        {
            get { return _time; }
            set { _time = value; }
        }

        private string _content;
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        private LogLevel _level;
        public LogLevel Level
        {
            get { return _level; }
            set { _level = value; }
        }

        private string _username;
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        private int _errorCode;
        public int ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }

        private UInt64 _rowIndex;
        public UInt64 RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }
        //构造方法
        public LogModel(string logContent, LogLevel logLevel, UInt64 index)
        {
            _time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            _content = logContent;
            _level = logLevel;
            _username = "Default";
            _errorCode = 0;
            _rowIndex = index;
        }

        public LogModel(string logContent, LogLevel logLevel, int errorCode, string userName, UInt64 index)
        {
            _time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            _content = logContent;
            _level = logLevel;
            this._errorCode = errorCode;
            if (string.IsNullOrWhiteSpace(userName))
            {
                this._username = "Default";
            }
            else
            {
                this._username = userName;
            }
            _rowIndex = index;
        }
    }
}
