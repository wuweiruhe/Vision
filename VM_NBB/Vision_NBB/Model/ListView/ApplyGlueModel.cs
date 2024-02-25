using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision_NBB.Model.ListView
{
    /// <summary>
    /// 上点胶
    /// </summary>
    public class ApplyGlueModel : ViewModelBase
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


        private string _x1;
        public string X1
        {
            get { return _x1; }
            set
            {
                _x1 = value;
                RaisePropertyChanged();
            }

        }


        private string _x2;
        public string X2
        {
            get { return _x2; }
            set
            {
                _x2 = value;
                RaisePropertyChanged();
            }
        }


        private string _x3;
        public string X3
        {
            get { return _x3; }
            set
            {
                _x3 = value;
                RaisePropertyChanged();
            }
        }


        private string _x4;
        public string X4
        {
            get { return _x4; }
            set
            {
                _x4 = value;
                RaisePropertyChanged();
            }
        }


        private string _x5;
        public string X5
        {
            get { return _x5; }
            set
            {
                _x5 = value;
                RaisePropertyChanged();
            }
        }


        private string _x6;
        public string X6
        {
            get { return _x6; }
            set
            {
                _x6 = value;
                RaisePropertyChanged();
            }
        }


        private string _x7;
        public string X7
        {
            get { return _x7; }
            set
            {
                _x7 = value;
                RaisePropertyChanged();
            }
        }


        private string _x8;
        public string X8
        {
            get { return _x8; }
            set
            {
                _x8 = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        /// OK/NG状态
        /// </summary>
        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged();
            }
        }

    }
}
