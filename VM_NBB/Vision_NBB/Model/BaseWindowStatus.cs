using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision_NBB.Model
{
    public class BaseWindowStatus : INotifyPropertyChanged
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
