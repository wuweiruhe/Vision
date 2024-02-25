using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Vision_NBB.Utility;

namespace Vision_NBB.Views.Pages
{
    /// <summary>
    /// DataHistoryWindow.xaml 的交互逻辑
    /// </summary>


    public partial class DataHistoryWindow : Window
    {
        int PreDataCounts = 0;
        int MaxDataSaveSize = 3;
        public ObservableCollection<Data> DataItems = new ObservableCollection<Data>();

        int cameraNo= 0;


        public DataHistoryWindow(int CameraNo, List<Data> dataHistoryItems)
        {
            InitializeComponent();
            cameraNo= CameraNo;
            foreach (var item in dataHistoryItems)
            {
                DataItems.Add(new Data() { CreateteTime = item.CreateteTime, Msg = item.Msg, DataImagePath = item.DataImagePath });
                //DataItems.Add(item);
            }

            PreDataCounts = DataItems.Count;
            list01.ItemsSource = DataItems;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(list01.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("CreateteTime", ListSortDirection.Descending));
        }

        public DataHistoryWindow()
        {
            InitializeComponent();
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



        public void AddData(Data data)
        {
            try
            {
                int currentDataCount = DataItems.Count;
                this.Dispatcher.Invoke(() =>
                {
                    DataItems.Add(data);
                });


                if (currentDataCount > 99)
                {
                    Thread.Sleep(10);
                    DataItems.RemoveAt(0);

                }
            }
            catch (Exception ex)
            {

                   GetLogHelper.VisionLog.Error(ex);
            }

           

        }



        private void list01_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListView;
            var selectItem = listView?.SelectedItem as Data;

            if (string.IsNullOrEmpty(selectItem.DataImagePath)) return;
     
            new ImageHistoryShow(selectItem).ShowDialog();
        }

   
    }


    public class Data
    {
        private DateTime _createTime;

        public DateTime CreateteTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }


        private string _msg;

        public string Msg
        {
            get { return _msg; }
            set { _msg = value; }
        }

        private string _dataImagePath;

        public string DataImagePath
        {
            get { return _dataImagePath; }
            set { _dataImagePath = value; }
        }


        public Data(DateTime dt,string msg,string path)
        {
            this.CreateteTime = dt;
            this.DataImagePath = path;
            this.Msg = msg;
        }
        public Data()
        {

        }
    }
}
