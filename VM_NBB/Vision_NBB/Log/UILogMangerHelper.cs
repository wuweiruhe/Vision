using FrontendUI.WPF.Editors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Vision_NBB.Model;

namespace Vision_NBB.Log
{
    public class UILogMangerHelper : ViewModelBase
    {
        static UILogMangerHelper()
        {
            instance = new UILogMangerHelper();
        }
        private UILogMangerHelper()
        {
        }
        public int MaxCount { get; set; }
        public UInt64 LogsCount
        {
            get
            {
                return (UInt64)logsAll.Count;
            }
        }
        public static string CurUserName { get; set; }
        public string AutoSavedLogPath { get; set; }
        //动态数据集合
        private ObservableCollection<LogModel> logsAll = new ObservableCollection<LogModel>();
        private ObservableCollection<LogModel> logsDebug = new ObservableCollection<LogModel>();
        private ObservableCollection<LogModel> logsInfo = new ObservableCollection<LogModel>();
        private ObservableCollection<LogModel> logsWarn = new ObservableCollection<LogModel>();
        private ObservableCollection<LogModel> logsError = new ObservableCollection<LogModel>();

        private static readonly UILogMangerHelper instance;
        private object _lock = new object();
        public static UILogMangerHelper Instance
        {
            get
            {
                return instance;
            }
        }
        public ObservableCollection<LogModel> LogsAll
        {
            get { return logsAll; }
        }

        public ObservableCollection<LogModel> LogsDebug
        {
            get { return logsDebug; }
        }

        public ObservableCollection<LogModel> LogsInfo
        {
            get { return logsInfo; }
        }

        public ObservableCollection<LogModel> LogsWarn
        {
            get { return logsWarn; }
        }

        public ObservableCollection<LogModel> LogsError
        {
            get { return logsError; }
        }
        private void DeleteAllLogs()
        {
            lock (_lock)
            {
                LogsAll.Clear();
                LogsDebug.Clear();
                LogsInfo.Clear();
                LogsWarn.Clear();
                LogsError.Clear();
            }
        }

        private void CheckLogsCapacity()
        {
            if (logsAll.Count > MaxCount)
            {
                DeleteAllLogs();
                logsAll.RemoveAt(0);
                logsAll.Clear();
                SaveLog(AutoSavedLogPath);
            }
        }

        private void AppendLog(LogLevel level, LogModel logEntity)
        {
            if (logEntity == null)
            {
                return;
            }
            lock (_lock)
            {
                LogsAll.Add(logEntity);
                switch (level)
                {
                    case LogLevel.Debug:
                        LogsDebug.Add(logEntity);
                        break;
                    case LogLevel.Info:
                        LogsInfo.Add(logEntity);
                        break;
                    case LogLevel.Warn:
                        LogsWarn.Add(logEntity);
                        break;
                    case LogLevel.Error:
                        LogsError.Add(logEntity);
                        break;
                    default:
                        break;
                }
            }
        }

        private void InsertLog(LogLevel level, LogModel logEntity, int index)
        {
            if (index < 0 || logEntity == null)
            {
                return;
            }
            lock (_lock)
            {
                LogsAll.Insert(index, logEntity);
                switch (level)
                {
                    case LogLevel.Debug:
                        LogsDebug.Insert(index, logEntity);
                        break;
                    case LogLevel.Info:
                        LogsInfo.Insert(index, logEntity);
                        break;
                    case LogLevel.Warn:
                        LogsWarn.Insert(index, logEntity);
                        break;
                    case LogLevel.Error:
                        LogsError.Insert(index, logEntity);
                        break;
                    default:
                        break;
                }
            }
        }

        public void AddLog(LogLevel level, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return;
           
            lock (_lock)
            {
                try
                {
                    CheckLogsCapacity();
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        logsAll?.Add(new LogModel(content, level, 0));
                    });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void AddLog(LogLevel level, string content, int errorCode, string userName)
        {
            if (string.IsNullOrWhiteSpace(content))
                return;
            LogModel logEntity = new LogModel(content, level, errorCode, userName, LogsCount);
            AppendLog(level, logEntity);
        }

        public void AddLog(LogLevel level, string content, int errorCode)
        {
            if (string.IsNullOrWhiteSpace(content))
                return;
            LogModel logEntity = new LogModel(content, level, errorCode, CurUserName, LogsCount);
            AppendLog(level, logEntity);
        }
        public void ClearLog()
        {
            DeleteAllLogs();
        }

        #region 本地保存日志
        //本地保存Warn日志
        public static void WarnLogSave(string strLog)
        {
            try
            {
                return;
                string basepath = AppDomain.CurrentDomain.BaseDirectory + "Log//Warn//";
                if (!Directory.Exists(basepath))
                {
                    Directory.CreateDirectory(basepath);
                }

                string path = basepath + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                StreamWriter sw = File.AppendText(path);
                sw.WriteLine(DateTime.Now.ToString("HH:mm:ss:fff") + " " + strLog);
                sw.Close();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //本地保存Debug日志
        public static void DebugLogSave(string strLog)
        {
            try
            {
                return;
                string basepath = AppDomain.CurrentDomain.BaseDirectory + "Log//Debug//";
                if (!Directory.Exists(basepath))
                {
                    Directory.CreateDirectory(basepath);
                }

                string path = basepath + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                StreamWriter sw = File.AppendText(path);
                sw.WriteLine(DateTime.Now.ToString("HH:mm:ss:fff") + " " + strLog);
                sw.Close();
            }
            catch
            {
            }
        }

        //本地保存Fatal日志
        public static void FatalLogSave(string strLog)
        {
            try
            {
                string basepath = AppDomain.CurrentDomain.BaseDirectory + "Log//Fatal//";
                if (!Directory.Exists(basepath))
                {
                    Directory.CreateDirectory(basepath);
                }

                string path = basepath + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                StreamWriter sw = File.AppendText(path);
                sw.WriteLine(DateTime.Now.ToString("HH:mm:ss:fff") + " " + strLog);
                sw.Close();
            }
            catch
            {
            }
        }

        //本地保存Info日志
        public static void InfoLogSave(string strLog)
        {
            try
            {
                string basepath = AppDomain.CurrentDomain.BaseDirectory + "Log//Info//";
                if (!Directory.Exists(basepath))
                {
                    Directory.CreateDirectory(basepath);
                }

                string path = basepath + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                StreamWriter sw = File.AppendText(path);
                sw.WriteLine(DateTime.Now.ToString("HH:mm:ss:fff") + " " + strLog);
                sw.Close();
            }
            catch
            {
            }
        }

        //本地保存Error日志
        public static void ErrorLogSave(string strLog)
        {
            try
            {
                string basepath = AppDomain.CurrentDomain.BaseDirectory + "Log//Error//";
                if (!Directory.Exists(basepath))
                {
                    Directory.CreateDirectory(basepath);
                }

                string path = basepath + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                StreamWriter sw = File.AppendText(path);
                sw.WriteLine(DateTime.Now.ToString("HH:mm:ss:fff") + " " + strLog);
                sw.Close();
            }
            catch
            {
            }
        }
        #endregion


        public bool SaveLog(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                StringBuilder sb = new StringBuilder();
                lock (_lock)
                {
                    //foreach ( var log in LogsAll )
                    //{
                    //    sb.Append(log.RowIndex.ToString() + "  " + log.Time.ToString() + "  " + log.Level.ToString() + "  " + log.Content + "\r\n");
                    //}
                }
                string fileName = filePath + "\\" + "RunningLog" + '_' + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".txt";
                if (SaveFile(sb.ToString(), fileName))
                {
                    DeleteAllLogs();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool SaveFile(string fileContent, string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileContent) && !string.IsNullOrWhiteSpace(fileName))
            {
                byte[] tempData = System.Text.UTF8Encoding.UTF8.GetBytes(fileContent);
                try
                {
                    using (FileStream SourceStream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                    {
                        SourceStream.SetLength(0);
                        SourceStream.Write(tempData, 0, tempData.Length);
                        SourceStream.Flush();
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
