using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision_NBB.Model
{
    public class ModelBlock : INotifyPropertyChanged
    {
        public string id { get; set; }

        public string processID { get; set; }
        public string Icon { get; set; }
        public string DisplayName { get; set; }
        public string RealName { get; set; }
        public string Name { get; set; }
        public string fullName { get; set; }
        private string NgIcon { get; set; }
        public bool isChange { get; set; }



        public string NgIcons
        {
            get
            {
                return NgIcon;
            }
            set
            {
                NgIcon = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("NgIcons"));
                }
            }
        }



        public List<ModelBlock> Children { get; set; }
        public ModelBlock()
        {
            Children = new List<ModelBlock>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
