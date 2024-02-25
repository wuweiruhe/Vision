using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Vision_NBB.Model
{
    public class ImageItem
    {
        public string Name { get; set; }
        public ImageSource Image { get; set; }


        public string ImagePath { get; set; }
        public DateTime ImageCreateTime { get; set; }

        public ImageItem(string name, ImageSource image, string imagePath, DateTime imageCreateTime)
        {
            Name = name;
            Image = image;
            ImagePath = imagePath;
            ImageCreateTime = imageCreateTime;
        }
    }
}
