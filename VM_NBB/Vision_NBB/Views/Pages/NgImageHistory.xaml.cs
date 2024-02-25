using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Vision_NBB.Model;
using Vision_NBB.Toolkit;
using Vision_NBB.Utility;

namespace Vision_NBB.Views.Pages
{
    public partial class NgImageHistory : Window
    {
        public ObservableCollection<ImageItem> imageItems = new ObservableCollection<ImageItem>();
        string path;
        int PreImageCount = 0;
        int MaxImageSaveSize = 62;
        private Configuration config = new Configuration();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public NgImageHistory(int CameraNo)
        {
            InitializeComponent();
            this.config = CurrentInfo.Config;

            //考虑path没有配置的情况
            path = GetImageSavePathByNo(CameraNo);

            if (String.IsNullOrEmpty(path)|| !Directory.Exists(path))
            {
                MessageBox.Show("图片路径为空");
               
                return;
            }
            else
            {
                PreImageCount = GetCurrentFileCount(path); ;
                var FilesName = GetOrderFilesNameByPath(path);

                SetImagesData(FilesName);


                list01.ItemsSource = imageItems;

                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(list01.ItemsSource);
                view.SortDescriptions.Add(new SortDescription("ImageCreateTime", ListSortDirection.Descending));


                Task.Run(() =>
                {
                    while (!cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        try
                        {
                            Thread.Sleep(2000);
                            path = GetImageSavePathByNo(CameraNo);
                            var ImageCount = GetCurrentFileCount(path);
                            this.Dispatcher.Invoke(() =>
                            {
                                if (ImageCount > PreImageCount)
                                {
                                    var filesName = GetOrderFilesNameByPath(path);
                                    int diffValue = ImageCount - PreImageCount;
                                    var NewAddImgName = filesName.Skip(filesName.Count() - diffValue).ToList();

                                    for (int i = 0; i < diffValue; i++)
                                    {
                                        if (ImageCount > MaxImageSaveSize)
                                            imageItems.RemoveAt(0);
                                    }

                                    SetImagesData(NewAddImgName);
                                    PreImageCount = ImageCount;
                                }
                            });
                        }
                        catch(Exception ex)
                        {
                           GetLogHelper.VisionLog.Debug(ex);
                        }
                    }
                }, cancellationTokenSource.Token);
            }
        }




        public string GetImageSavePathByNo(int No)
        {
            string Path = "";
            switch (No)
            {
                case 1: Path = FileHelper.GetSaveImagePathByTime(config.cameraSettings.ImagePath, CurrentInfo.CameraDir.Camera1_FileDirName, "NG");break;
                case 2: Path = FileHelper.GetSaveImagePathByTime(config.cameraSettings.ImagePath, CurrentInfo.CameraDir.Camera2_FileDirName, "NG");break;
                case 3: Path = FileHelper.GetSaveImagePathByTime(config.cameraSettings.ImagePath, CurrentInfo.CameraDir.Camera3_FileDirName, "NG");break;
                case 4: Path = FileHelper.GetSaveImagePathByTime(config.cameraSettings.ImagePath, CurrentInfo.CameraDir.Camera4_FileDirName, "NG");break;
         
                default:
                    break;
            }

            return Path;

        }

        public void SetImagesData(List<string> PathList)
        {
            foreach (var item in PathList)
            {
                FileInfo info = new FileInfo(item);
                var baseName = System.IO.Path.GetFileNameWithoutExtension(info.FullName);

                imageItems.Add(new ImageItem(baseName, GetImage(item), info.FullName, info.CreationTime));
            }
        }


        private BitmapImage GetImage(string fileName)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fileName, UriKind.Absolute);
            bitmap.DecodePixelWidth = 100; //设置解码后图像的宽度，图像变小，解析变快
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            return bitmap;

        }


        public List<string> GetOrderFilesNameByPath(string path, string EndName = ".jpg", int TakeNum = 80)
        {
            Dictionary<string, DateTime> dict = new Dictionary<string, DateTime>();
            if (Directory.Exists(path))
            {

                foreach (var item in Directory.GetFiles(path).Where(t => t.EndsWith(EndName)))
                {
                    var dir = new FileInfo(item);
                    dict[item] = dir.CreationTime;
                }

                var dict_ordy = dict.OrderByDescending(f => f.Value).ToDictionary(f => f.Key, f => f.Value);
                var list = dict_ordy.Keys.Take(TakeNum).ToList();
                list.Reverse();

                return list;

            }

            return null;
        }



        public int GetCurrentFileCount(string path)
        {
            return Directory.GetFiles(path).Count();
        }









        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListView;
            var selectedItem = listView?.SelectedItem as ImageItem;
            new ImageShow(selectedItem).ShowDialog();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
                scrollviewer.LineUp();
            else
                scrollviewer.LineDown();
            e.Handled = true;

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource = null;
            }

            Thread.Sleep(150);

            //this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

            //if (cancellationTokenSource != null)
            //{
            //    cancellationTokenSource.Cancel();
            //    cancellationTokenSource = null;
            //}

            //Thread.Sleep(100);


        }

     
    }
}

