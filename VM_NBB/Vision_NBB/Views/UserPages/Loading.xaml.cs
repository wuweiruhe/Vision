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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Vision_NBB.Views.UserPages
{
    /// <summary>
    /// Loading.xaml 的交互逻辑
    /// </summary>
    public partial class Loading : Window
    {
        DispatcherTimer timer;

        public Func<bool, int> getMessage;

        public int CurrentValue;

        public Loading()
        {
            InitializeComponent();
            timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromMilliseconds(200);

            timer.Tick += timer1_Tick;
            //启动
            timer.Start();
        }

        public Loading(string message)
        {
            InitializeComponent();

            lbl_message.Content = message;


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (CurrentValue < 100)
            {
                CurrentValue = getMessage(true);
            }
            else
            {
                timer.Stop();
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var br = this.img;
            //指定坐标系中顺时针旋转对象
            RotateTransform rotate = new RotateTransform();
            br.RenderTransform = rotate;
            //X Y坐标
            br.RenderTransformOrigin = new Point(0.5, 0.5);
            Storyboard story = new Storyboard();
            DoubleAnimation da = new DoubleAnimation(0, 36000, new Duration(TimeSpan.FromSeconds(370)));
            Storyboard.SetTarget(da, br);
            Storyboard.SetTargetProperty(da, new PropertyPath("RenderTransform.Angle"));
            da.RepeatBehavior = RepeatBehavior.Forever;
            story.Children.Add(da);
            story.Begin();
        }
    }
}
