using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision_NBB.Views.Controls;

namespace Vision_NBB.Model
{ 
    public class WindowStatus : INotifyPropertyChanged
    {
        string ng_color { get; set; }
        public string Ng_color
        {
            get
            {
                return ng_color;
            }
            set
            {
                ng_color = value;
                OnPropertyChanged("Ng_color");
            }
        }

        string ng_text { get; set; }
        public string Ng_text
        {
            get
            {
                return ng_text;
            }
            set
            {
                ng_text = value;
                OnPropertyChanged("Ng_text");
            }
        }

        int timeStatistic { get; set; }

        public int TimeStatistic
        {
            get
            {
                return timeStatistic;
            }
            set
            {
                timeStatistic = value;
                OnPropertyChanged("TimeStatistic");
            }
        }



        private Camera1Result result;

        public Camera1Result Result1
        {
            get { return result; }
            set
            {
                result = value;

                OnPropertyChanged("Result1");
            }
        }



        private Camera2Result result2;

        public Camera2Result Result2
        {
            get { return result2; }
            set
            {
                result2 = value;

                OnPropertyChanged("Result2");
            }
        }

        private Camera3Result result3;

        public Camera3Result Result3
        {
            get { return result3; }
            set
            {
                result3 = value;

                OnPropertyChanged("Result3");
            }
        }

        private Camera4Result result4;

        public Camera4Result Result4
        {
            get { return result4; }
            set
            {
                result4 = value;

                OnPropertyChanged("Result4");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
