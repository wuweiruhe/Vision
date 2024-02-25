using System;
using System.Collections.Generic;
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
using Vision_NBB.Views.UserPages;

namespace Vision_NBB.Views.Pages
{
    /// <summary>
    /// VM.xaml 的交互逻辑
    /// </summary>
    public partial class VM : Window
    {
        public Action<bool> showMainWin;
        public int ProgressBarVal;

        public VM()
        {
            Thread trd = new Thread(new ThreadStart(start_splash));
            trd.SetApartmentState(ApartmentState.STA);
            trd.Start();
            ProgressBarVal = 25;

            InitializeComponent();
           
            ProgressBarVal = 100;

        }

        #region 进度条相关方法

        private void start_splash()
        {

            var frm = new Loading();
            frm.getMessage += getProgressBarVal;
            frm.ShowDialog();


        }

        public int getProgressBarVal(bool flag)
        {
            return ProgressBarVal;
        }

        #endregion


        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            showMainWin(true);
            this.Close();
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
