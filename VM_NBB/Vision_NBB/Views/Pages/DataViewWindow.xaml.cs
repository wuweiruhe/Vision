using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Vision_NBB.Model.ListView;

namespace Vision_NBB.Views.Pages
{
    /// <summary>
    /// DataViewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DataViewWindow : Window
    {

        public ObservableCollection<ManipulatorModel> manItem = new ObservableCollection<ManipulatorModel>();
        public ObservableCollection<ApplyGlueModel> applyItem = new ObservableCollection<ApplyGlueModel>();
        public ObservableCollection<BackDispensingModel> backItem = new ObservableCollection<BackDispensingModel>();

        public DataViewWindow()
        {

            InitializeComponent();

            ManipultorInit();
            ApplyGlueInit();
            BackDispensingInit();

            this.lsv_manipultor.ItemsSource = manItem;
            this.lsv_applyglue.ItemsSource = applyItem;
            this.lsv_backdispensing.ItemsSource = backItem;

        }

        /// <summary>
        /// 机械手Model初始化
        /// </summary>
        public void ManipultorInit()
        {
            manItem.Add(new ManipulatorModel
            {
                Time = DateTime.Now.ToString(),
                PointX = "testX",
                PointY = "testY",
                PointR = "testR"
            }
            );
        }

        /// <summary>
        /// 上点胶Model初始化
        /// </summary>
        public  void ApplyGlueInit()
        {
            applyItem.Add(new ApplyGlueModel
            {
                Time=DateTime.Now.ToString(),
                X1= "testX1",
                X2= "testX2",
                X3= "testX3",
                X4= "testX4",
                X5= "testX5",
                X6= "testX6",
                X7= "testX7",
                X8= "testX8",
                Status="OK"
            }
            );
        }

        /// <summary>
        /// 背点胶Model初始化
        /// </summary>
        public void BackDispensingInit()
        {
            backItem.Add(new BackDispensingModel
            {
                Time = DateTime.Now.ToString(),
                X1 = "testX1",
                X2 = "testX2",
                X3 = "testX3",
                X4 = "testX4",
                X5 = "testX5",
                X6 = "testX6",
                X7 = "testX7",
                X8 = "testX8",
                Status = "NG"
            });
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
