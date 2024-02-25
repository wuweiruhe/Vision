using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vision_NBB.Model;
using Vision_NBB.Utility;
using Vision_NBB.Views.Pages;

namespace Vision_NBB.Controls
{
    /// <summary>
    /// TextBoxExt.xaml 的交互逻辑
    /// </summary>
    public partial class TextBoxExt : System.Windows.Controls.UserControl
    {
        public delegate void ButtonClickHandler();
        string globalDir = AppDomain.CurrentDomain.BaseDirectory + "ConfigurationSetting\\GlobalSetting.json";
        public TextBoxExt()
        {
            InitializeComponent();
        }

 
        public string Folder
        {
            get { return (string)GetValue(FolderProperty); }
            set { SetValue(FolderProperty, value); }
        }

        public static readonly DependencyProperty FolderProperty = DependencyProperty.Register("Folder", typeof(string), typeof(TextBoxExt), new PropertyMetadata("tuyan", onPropertyChange));

        private static void onPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxExt v = (TextBoxExt)d;
            v.txtFolder.Text = v.Folder;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "exe,VM Sol File,csv|*.exe;*.sol*;*.csv";
            DialogResult openFileRes = openFileDialog.ShowDialog();
            if (DialogResult.OK == openFileRes)
            {
                Folder = openFileDialog.FileName;
                CurrentInfo.Config.SoluctionName = Folder;
            }      
        }
    }
}
