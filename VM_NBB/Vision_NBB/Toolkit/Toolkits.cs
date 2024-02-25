using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Xml;
using System.Xml.Serialization;
using Vision_NBB.Model;
using Vision_NBB.Views.Pages;
using Vision_NBB.Views.UserPages;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Vision_NBB.Toolkit
{
    public class Toolkits
    {

        public static Dictionary<string, string> dic = new Dictionary<string, string>();
        public static Dictionary<string, string> dic2 = new Dictionary<string, string>();
        public static Dictionary<string, string> dic3 = new Dictionary<string, string>();
        public static Dictionary<string, string> dic4 = new Dictionary<string, string>();

        private object Lock = new object();



        public static Dictionary<string,string> CameraSequenceToDict(string CameraSequence)
        {


            // 去掉大括号，并使用逗号分割字符串，得到一个字符串数组
            string[] numberStrings = CameraSequence.Trim('{', '}').Split(',');

            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            for (int i = 0; i < numberStrings.Length; i++)
            {
                dictionary[numberStrings[i]] = numberStrings[i];
            }


            return dictionary;

        }



        public static void showMessage(string message = null)
        {
            //if(message==null)  message = Language.Instance.GetString("IDS_NO_RIGHT_TO_ACCESS");
            //var frm = new frm_message(message);
            //frm.ShowDialog();

           

            var frm = new MessageShow(message);
            frm.ShowDialog();

        }


        public static string getFileByName(string name)
        {
            string index = "";
            switch (name)
            {
                //图像源
                case "ImageSourceModule": index = "/ICON/toolbar_image_camera.png"; break;

                //条件检测
                case "IfModule": index = "/ICON/toolbar_logical_条件检测.png";  break;

                //直线查找
                case "IMVSLineFindModu":index = "/ICON/ic_toolbar_直线查找组合_normal.png"; break;


                //线线测量
                case "IMVSL2LMeasureModu":  index = "/ICON/toolbar_mea_线线测量.png"; break;

                //边缘交点
                case "IMVSCaliperCornerModu":  index = "/ICON/toolbar_popovers_ic_交点定位.png"; break;

                //变量计算
                case "CalculatorModule":index = "/ICON/toolbar_logical_caculate.png"; break;

                //快速匹配
                case "IMVSFastFeatureMatchModu": index = "/ICON/toolbar_popovers_ic_特征匹配.png"; break;

                case "IMVSScaleTransformModu": index = "/ICON/ic_数据中心_focus.png"; break;

                //位置修正
                case "IMVSFixtureModu": index = "/ICON/toolbar_locate_位置修正.png"; break;

                //形态学处理
                case "IMVSImageMorphModu": index = "/ICON/toolbar_imageprocess_形态学.png"; break;

                //脚本
                case "ShellModule": index = "/ICON/toolbar_logical_脚本.png"; break;

                //ic_直线边缘缺陷检测 
                case "IMVSLineEdgeInspModu": index = "/ICON/ic_直线边缘缺陷检测.png"; break;

                //ic_toolbar_四边形查找_normal
                case "IMVSQuadrangleFindModu": index = "/ICON/ic_toolbar_四边形查找_normal.png"; break;

                //toolbar_mea_几何创建_normal
                case "GeometryCreate": index = "/ICON/toolbar_mea_几何创建_normal.png"; break;

                //toolbar_locate_Blob 分析
                case "IMVSBlobFindModu": index = "/ICON/toolbar_locate_Blob.png"; break;

                default: index = "/ICON/toolbar_mea_线线测量.png";  break;
            }
            return index;

        }



        public static byte[] Serialize(object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, o);
            return ms.ToArray();
        }

        public static object Deserialize(object o)
        {
            return Deserialize(o as byte[]);
        }

        public static object Deserialize(byte[] bytes)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            ms.Write(bytes, 0, bytes.Length);
            ms.Position = 0;
            object o = formatter.Deserialize(ms);
            return o;
        }

        public static T Deserialize<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            Stream stream = new FileStream(xml, FileMode.Open, FileAccess.Read, FileShare.Read);
            object o = serializer.Deserialize(stream);
            stream.Close();

            return (T)o;
        }

        public static void Serialize<T>(T o, string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            xs.Serialize(stream, o);
            stream.Close();
        }

        public static string Serialize<T>(T o)
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, o);
            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Close();
            return Convert.ToBase64String(buffer);
        }

        public static T DeserializeString<T>(string str)
        {
            IFormatter formatter = new BinaryFormatter();
            byte[] buffer = Convert.FromBase64String(str);
            MemoryStream stream = new MemoryStream(buffer);
            object o = formatter.Deserialize(stream);
            stream.Flush();
            stream.Close();

            return (T)o;
        }


        public static Bitmap ToGrayBitmap(byte[] rawValues, int w, int h)
        {
            //// 申请目标位图的变量，并将其内存区域锁定  
            if (rawValues == null || w == 0 || h == 0)
                return null;
            Bitmap bmp = new Bitmap(w, h, PixelFormat.Format8bppIndexed);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            //// 获取图像参数  
            int stride = bmpData.Stride;  // 扫描线的宽度  
            int offset = stride - w;  // 显示宽度与扫描线宽度的间隙  
            IntPtr iptr = bmpData.Scan0;  // 获取bmpData的内存起始位置  
            int scanBytes = stride * h;// 用stride宽度，表示这是内存区域的大小   
            byte[] pixelValues = new byte[scanBytes];  //为目标数组分配内存     
            for (int x = 0; x < h; x++)
            {
                Array.Copy(rawValues, x * w, pixelValues, x * (offset + w), w);
            }
            System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, iptr, scanBytes);
            bmp.UnlockBits(bmpData); // 解锁内存区域    
            SetGrayscalePalette(ref bmp);
            return bmp;
        }


        private static void SetGrayscalePalette(ref Bitmap srcImg)
        {
            if (srcImg.PixelFormat != PixelFormat.Format8bppIndexed)
                throw new ArgumentException();
            ColorPalette cp = srcImg.Palette;
            for (int i = 0; i < 256; i++)
                cp.Entries[i] = Color.FromArgb(i, i, i);
            srcImg.Palette = cp;
        }


        public static void GetRealNameByKEey()
        {




        }

        public static List<List<string>> ParseCameraSequence(string cameraSequence)
        {
            MatchCollection matches = Regex.Matches(cameraSequence, @"\{([^\}]*)\}");
            List<List<string>> list = new List<List<string>>();
            foreach (Match match in matches)
            {
                string values = match.Groups[1].Value;
                var valueArray = values.Split(',').ToList();
                list.Add(valueArray);

            }

            return list;


        }




        public static void ShowPopup(string contentText)
        {
           
            var popupWindow = new Window
            {
                Width = 400,
                Height = 80,
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Background = System.Windows.Media.Brushes.LightGreen,
                Content = new TextBlock
                {
                    Text = contentText,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };

          
            popupWindow.Left = (SystemParameters.PrimaryScreenWidth - popupWindow.Width) / 2;
            popupWindow.Top = (SystemParameters.PrimaryScreenHeight - popupWindow.Height) / 2;

          
            var animation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(2.5),
                FillBehavior = FillBehavior.Stop
            };

         
            animation.Completed += (s, a) =>
            {
                popupWindow.Close();
            };

      
            popupWindow.BeginAnimation(Window.OpacityProperty, animation);

          
            popupWindow.ShowDialog();
        }






        public static string StartExternalProcess(string path, string arguments)
        {
            try
            {

                if (!File.Exists(path)) return "Can not Find Path EXE File";

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = path,
                    Arguments = arguments,
                    UseShellExecute = false, // 设置为false以便重定向输入/输出
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true // 不创建新窗口
                };


                using (Process process = new Process { StartInfo = startInfo })
                {

                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    return output;


                }

                //return "FAILED";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }








    }
}
