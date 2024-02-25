using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision_NBB.Model.ListView
{
    /// <summary>
    /// 机械手ListView实体类绑定
    /// </summary>
  public  class ManipulatorModel : ViewModelBase
    {
        /// <summary>
        /// 当前时间
        /// </summary>
        private string _time;

        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        /// X坐标
        /// </summary>
        private string _pointX;

        public string PointX
        {
            get { return _pointX; }
            set { _pointX = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        /// Y坐标
        /// </summary>
        private string _pointY;

        public string PointY
        {
            get { return _pointY; }
            set
            {
                _pointY = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// R坐标
        /// </summary>
        private string _pointR;

        public string PointR
        {
            get { return _pointR; }
            set
            {
                _pointR = value;
                RaisePropertyChanged();
            }
        }
    }
}
