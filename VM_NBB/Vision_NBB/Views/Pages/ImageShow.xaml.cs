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
using Vision_NBB.Model;

namespace Vision_NBB.Views.Pages
{
    /// <summary>
    /// ImageShow.xaml 的交互逻辑
    /// </summary>
    public partial class ImageShow : Window
    {
        public ImageShow(ImageItem selectedItem)
        {
            InitializeComponent();

            DataContext = selectedItem;
        }



        public ImageShow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 鼠标缩放图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var point = e.GetPosition(image);
            var delta = e.Delta * 0.002;
            DoScale(point, delta);
        }


        /// <summary>
        /// 缩放图片。最小为0.1倍，最大为30倍
        /// </summary>
        /// <param name="point">相对于图片的点，以此点为中心缩放</param>
        /// <param name="delta">缩放的倍数增量</param>
        private void DoScale(Point point, double delta)
        {
            // 限制最大、最小缩放倍数
            if (scaler.ScaleX + delta < 0.1 || scaler.ScaleX + delta > 30) return;

            scaler.ScaleX += delta;
            scaler.ScaleY += delta;

            transer.X -= point.X * delta;
            transer.Y -= point.Y * delta;
        }


    }
}
