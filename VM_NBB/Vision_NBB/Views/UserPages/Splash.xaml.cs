using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace Vision_NBB.Views.UserPages
{
    /// <summary>
    /// Splash.xaml 的交互逻辑
    /// </summary>
    public partial class Splash : Window
    {
        DispatcherTimer timer;

        public Func<bool, int> getMessage;

        public Splash()
        {
            InitializeComponent();

            timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromMilliseconds(200);

            timer.Tick += timer1_Tick;
            //启动
            timer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar.Value < progressBar.Maximum)
            {
                progressBar.Value = (double)getMessage(true);
            }
            else
            {
                timer.Stop();
                this.Close();
            }
        }
    }
}
