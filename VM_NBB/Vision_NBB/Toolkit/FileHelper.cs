using Apps.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision_NBB.Log;
using Vision_NBB.Utility;
using Vision_NBB.Views.Controls;

namespace Vision_NBB.Toolkit
{
    public static class FileHelper
    {

        static long fileMaxSize = 1024 * 1024 * 32;

        static int fileMaxday = 7;

        static bool isDeteting = false;
        private static object DeletLock = new object();


        public static object Camera1_DeleteLock = new object();
        public static object Camera2_DeleteLock = new object();
        public static object Camera3_DeleteLock = new object();
        public static object Camera4_DeleteLock = new object();



        static StringBuilder sb = new StringBuilder();

        public static bool isExistFile(string fileName)
        {

            return File.Exists(fileName);



        }

        public static void WriteText(string Path, string txt, bool isAppend = true)
        {
            try
            {

                using (var sw = new StreamWriter(Path, isAppend))
                {

                    sw.WriteLine(txt);
                }



            }
            catch (Exception ex)
            {

                throw ex;
            }



        }

        public static void WriteTexts(string Path, string[] txt, bool isAppend = true, string split = " ")
        {
            try
            {
                var sb = new StringBuilder();
                foreach (var item in txt)
                {
                    sb.Append(item);
                    sb.Append(split);


                }

                using (var sw = new StreamWriter(Path, isAppend))
                {

                    sw.WriteLine(sb.ToString());
                }



            }
            catch (Exception ex)
            {
                 GetLogHelper.VisionLog.Warn(ex.ToString());

            }



        }

        public static string CreateFileNmaeByTime(string ExtendsName)
        {
            string path = "";
            try
            {
                if (String.IsNullOrWhiteSpace(ExtendsName)) ExtendsName = "";

                path = ExtendsName + DateTime.Now.ToString("yyyy_MM_dd: HH:mm:ss");



            }
            catch (Exception ex)
            {
                 GetLogHelper.VisionLog.Warn(ex.ToString());

            }

            return path;

        }


        public static void DeleteDir(string dir)
        {
            if (dir.Length == 0) return;
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
        }


        public static void DeletedirByDaysAll(string path, int day)
        {

            Dictionary<string, DateTime> dic = new Dictionary<string, DateTime>();

            if (Directory.Exists(path))
            {
                foreach (var item in Directory.GetDirectories(path))
                {
                    var dir = new DirectoryInfo(item);
                    dic[item] = dir.CreationTime;
                }
                var dic_ordy = dic.OrderBy(f => f.Value).ToDictionary(f => f.Key, f => f.Value);
                var count = dic_ordy.Count();

                if (count > day)
                {
                    var delete_objs = dic_ordy.Take(count - day);
                    foreach (var item in delete_objs)
                    {

                        DeleteDirAsyn(item.Key);
                    }

                }

            }

        }


        public static string CreateDirByTime(string basePath)
        {
            string path = "";

            try
            {
                if (Directory.Exists(basePath))
                {
                    path = System.IO.Path.Combine(basePath, DateTime.Now.ToString("yyyy_MM_dd").ToString());

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }



            }
            catch (Exception ex)
            {
                 GetLogHelper.VisionLog.Warn(ex.ToString());

            }

            return path;

        }

        public static string CreateDirByTime(string basePath, bool ishasExended)
        {
            string path = "";

            try
            {

                if (Directory.Exists(basePath))
                {
                    path = System.IO.Path.Combine(basePath, DateTime.Now.ToString("yyyy_MM_dd").ToString());

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }


                    if (ishasExended)
                    {
                        string curExendedName;

                        var fff = DateTime.Now.Hour;
                        if (DateTime.Now.Hour >= 0 || DateTime.Now.Hour <= 20)
                        {
                            curExendedName = "白班";

                        }
                        else
                        {
                            curExendedName = "夜班";

                        }


                        path = System.IO.Path.Combine(path, curExendedName);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                    }

                }



            }
            catch (Exception ex)
            {
                 GetLogHelper.VisionLog.Error(ex.ToString());
                UILogMangerHelper.Instance.AddLog(Model.LogLevel.Debug, "删除文件异常");
                //UILogManager.Instance.AddLog(UILogLevel.Error, "删除文件异常");

            }
            return path;
        }

        public static string CreateDirByTime(string basePath, string CameraDir, string ExtensionName, bool ishasExended = true)
        {
            string path = "";
     //       string curExendedName = "";
            try
            {
                basePath = System.IO.Path.Combine(basePath, CameraDir);
                if (Directory.Exists(basePath))
                {
                    if (ishasExended)
                    {
                        path = System.IO.Path.Combine(basePath, DateTime.Now.ToString("yyyy_MM_dd").ToString());
                        var Time = DateTime.Now.Hour;                      
                        path = System.IO.Path.Combine(path, ExtensionName);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return path;
        }


        //public static string CreateDirByTime(string basePath, string CameraDir, string ExtensionName, bool ishasExended = true)
        //{
        //含白夜班判断
        //    string path = "";
        //    string curExendedName = "";
        //    try
        //    {
        //        basePath = System.IO.Path.Combine(basePath, CameraDir);
        //        if (Directory.Exists(basePath))
        //        {
        //            if (ishasExended)
        //            {
        //                var Time = DateTime.Now.Hour;
        //                if (Time >= 8 && Time <= 20)
        //                {
        //                  curExendedName = "白班";
        //                    path = System.IO.Path.Combine(basePath, DateTime.Now.ToString("yyyy_MM_dd").ToString());


        //                }
        //                else if (Time > 20 && Time <= 24)
        //                {
        //                    curExendedName = "夜班";
        //                    path = System.IO.Path.Combine(basePath, DateTime.Now.ToString("yyyy_MM_dd").ToString());
        //                }
        //                else
        //                {
        //                    if (Time >= 0 && Time < 8)
        //                    {
        //                        curExendedName = "夜班";
        //                        path = System.IO.Path.Combine(basePath, DateTime.Now.AddDays(-1).ToString("yyyy_MM_dd").ToString());
        //                    }
        //                }

        //                curExendedName = System.IO.Path.Combine(curExendedName, ExtensionName);
        //                path = System.IO.Path.Combine(path, curExendedName);
        //                if (!Directory.Exists(path))
        //                {
        //                    Directory.CreateDirectory(path);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return path;
        //}

        public static string GetSaveImagePathByTime(string basePath, string CameraDir, string ExtensionName)
        {
            string path = "";
            string curExendedName = "";
            try
            {
                basePath = System.IO.Path.Combine(basePath, CameraDir);
                path = System.IO.Path.Combine(basePath, DateTime.Now.ToString("yyyy_MM_dd").ToString());
                curExendedName = System.IO.Path.Combine(curExendedName, ExtensionName);
                path = System.IO.Path.Combine(path, curExendedName);
            

            }
            catch (Exception ex)
            {
            }
            return path;


           
        }

        public static void DeletedirByDay(string path, string ExtensionNmae, int day = 30, bool isUseTimerJudge = true)
        {
            try
            {
                if (isUseTimerJudge == true)
                    if (IsTimeInRange() == false) return;
 
                Dictionary<string, DateTime> dic = new Dictionary<string, DateTime>();
                path = System.IO.Path.Combine(path, ExtensionNmae);

                if (Directory.Exists(path))
                {
                    if (Directory.GetDirectories(path).Count() <= day) return;

                    foreach (var item in Directory.GetDirectories(path))
                    {
                        var dir = new DirectoryInfo(item);
                        dic[item] = dir.CreationTime;
                    }
                    var dic_ordy = dic.OrderBy(f => f.Value).ToDictionary(f => f.Key, f => f.Value);
                    var count = dic_ordy.Count();


                    if (count > day)
                    {
                        var delete_objs = dic_ordy.First().Key;
                        DeleteDirAsyn(delete_objs);

                    }

                }

            }
            catch (Exception ex)
            {
                 GetLogHelper.VisionLog.Warn(ex.ToString());

            }



        }




        /// <summary>
        /// 带标志位锁  防止大文件 耗时久时 没完全删除时，又有线程进入 查找文件浪费资源
        /// </summary>
        /// <param name="ObjLock"></param>
        /// <param name="path"></param>
        /// <param name="ExtensionNmae"></param>
        /// <param name="day"></param>
        /// <param name="isUseTimerJudge"></param>
        public static void DeletedirByDay(object ObjLock , string path, string ExtensionNmae, int day = 30, bool isUseTimerJudge = true)
        {
            try
            {
               

                if (isUseTimerJudge == true)
                    if (IsTimeInRange() == false) return;

               

                Dictionary<string, DateTime> dic = new Dictionary<string, DateTime>();

                path = System.IO.Path.Combine(path, ExtensionNmae);
                if (Directory.Exists(path))
                {
                    if (Directory.GetDirectories(path).Count() <= day) return;

                    foreach (var item in Directory.GetDirectories(path))
                    {
                        var dir = new DirectoryInfo(item);
                        dic[item] = dir.CreationTime;
                    }
                    var dic_ordy = dic.OrderBy(f => f.Value).ToDictionary(f => f.Key, f => f.Value);
                    var count = dic_ordy.Count();


                    if (count > day)
                    {
                        var delete_objs = dic_ordy.First().Key;
                       
                        //DeleteDirAsyn(delete_objs);


                        if (Directory.Exists(delete_objs))
                        {
                            Task.Run(() =>
                            {


                                lock (ObjLock)
                                {
                                    try
                                    {

                                        if (Directory.GetDirectories(path).Count() > day && Directory.Exists(delete_objs))
                                        {
                                            GetLogHelper.VisionLog.Info("FILE IS DELETING");
                                            Directory.Delete(delete_objs, true);
                                            GetLogHelper.VisionLog.Info("FILE IS DELETED");
                                        }


                                    }
                                    catch (Exception ex)
                                    {

                                        GetLogHelper.VisionLog.Error(ex.ToString());
                                    }

                                }



                            });



                        }
                       
                    }

                }

            }
            catch (Exception ex)
            {
                GetLogHelper.VisionLog.Error(ex.ToString());
              
            }



        }




       
        public static bool IsTimeInRange(int StartH = 16, int StartM = 51, int StartS = 0, int EndH = 16, int EndM = 52, int EndS = 0)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan time = currentTime.TimeOfDay;

            TimeSpan startTime = new TimeSpan(StartH, StartM, StartS); // 12:00:00
            TimeSpan endTime = new TimeSpan(EndH, EndM, EndM); // 12:01:00

            return time > startTime && time < endTime;
        }
        public static void AddResultText(string name, object value)
        {
            sb.Append($"{name}: {value.ToString()}" + "\r\n");
        }

        public static string AddResultTextArray(string name, float[] value)
        {
            sb.Clear();
            sb.Append($"{name}:");
            for (int i = 0; i < value.Length; i++)
            {
                sb.Append($"{value[i].ToString()+", "}" );
            }
            sb.Append("\r\n");
            return sb.ToString();
        }


        public static string GetFirstFileNameByPath(string path, string EndName = ".mat")
        {
            try
            {
                Dictionary<string, DateTime> dic = new Dictionary<string, DateTime>();
                if (Directory.Exists(path))
                {
                    foreach (var item in Directory.GetFiles(path).Where(t => t.Contains(EndName)))
                    {
                        var dir = new FileInfo(item);
                        dic[item] = dir.LastWriteTime;
                    }
                    var dic_ordy = dic.OrderBy(f => f.Value).ToDictionary(f => f.Key, f => f.Value);
                    return dic_ordy.Last().Key;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;

            } 
          
        }


        public static string GetCalibFileNameByPath(string path, string EndName = ".xml")
        {
            try
            {
                Dictionary<string, DateTime> dic = new Dictionary<string, DateTime>();
                if (Directory.Exists(path))
                {
                    foreach (var item in Directory.GetFiles(path).Where(t => t.Contains(EndName)))
                    {
                        var dir = new FileInfo(item);
                        dic[item] = dir.LastWriteTime;
                    }
                    var dic_ordy = dic.OrderBy(f => f.Value).ToDictionary(f => f.Key, f => f.Value);
                    return dic_ordy.Last().Key;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;

            }

           
        }


        public static string GetDistortionFileNameByPath(string path, string EndName = ".iccal")
        {

            try
            {
                Dictionary<string, DateTime> dic = new Dictionary<string, DateTime>();
                if (Directory.Exists(path))
                {
                    foreach (var item in Directory.GetFiles(path).Where(t => t.Contains(EndName)))
                    {
                        var dir = new FileInfo(item);
                        dic[item] = dir.LastWriteTime;
                    }
                    var dic_ordy = dic.OrderBy(f => f.Value).ToDictionary(f => f.Key, f => f.Value);
                    return dic_ordy.Last().Key;
                }
                return null;
            }
            catch (Exception ex)
            {

                return null;
            }
           
        }




        public static void DeletedirByDay(string path)
        {
            try
            {
                Dictionary<string, DateTime> dic = new Dictionary<string, DateTime>();

                if (Directory.Exists(path))
                {
                    foreach (var item in Directory.GetDirectories(path))
                    {
                        var dir = new DirectoryInfo(item);
                        dic[item] = dir.CreationTime;
                    }
                    var dic_ordy = dic.OrderBy(f => f.Value).ToDictionary(f => f.Key, f => f.Value);
                    var count = dic_ordy.Count();



                    var delete_objs = dic_ordy.First().Key;


                    DeleteDirAsyn(delete_objs);
                }
            }
            catch (Exception ex)
            {
                 GetLogHelper.VisionLog.Warn(ex.ToString());
            }






        }

        public static void DeletedirByDay(string path, int day)
        {
            try
            {
                Dictionary<string, DateTime> dic = new Dictionary<string, DateTime>();

                if (Directory.Exists(path))
                {
                    foreach (var item in Directory.GetDirectories(path))
                    {
                        var dir = new DirectoryInfo(item);
                        dic[item] = dir.CreationTime;
                    }
                    var dic_ordy = dic.OrderBy(f => f.Value).ToDictionary(f => f.Key, f => f.Value);
                    var count = dic_ordy.Count();


                    if (count > day)
                    {
                        var delete_objs = dic_ordy.First().Key;
                        DeleteDirAsyn(delete_objs);

                    }

                }

            }
            catch (Exception ex)
            {
                 GetLogHelper.VisionLog.Warn(ex.ToString());

            }



        }

       


        public static void DeleteDirAsyn(string dir)
        {
            try
            {

                    if (dir.Length == 0) return;

                    if (Directory.Exists(dir))
                    {
                        Task.Run(() =>
                        {               
                            GetLogHelper.VisionLog.Warn("FILE IS DELETING");
                            Directory.Delete(dir, true);
                            GetLogHelper.VisionLog.Warn("FILE IS FINISHED");

                        });



                    }


            }
            catch (Exception ex)
            {
                 GetLogHelper.VisionLog.Warn(ex.ToString());
            }
        }


        //public static void DeleteDirAsyn(string dir)
        //{
        //    try
        //    {

        //        if (dir.Length == 0) return;

        //        if (Directory.Exists(dir))
        //        {
        //            Task.Run(() =>
        //            {
        //                GetLogHelper.VisionLog.Warn("FILE IS DELETING");
        //                Directory.Delete(dir, true);
        //                GetLogHelper.VisionLog.Warn("FILE IS FINISHED");

        //            });



        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        GetLogHelper.VisionLog.Warn(ex.ToString());
        //    }
        //}

    }






    public enum CreateMethod
    {
        FileSize = 0,
        FileTime = 1,
    }
}
