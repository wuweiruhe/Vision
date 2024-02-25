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

namespace Vision_NBB.Controls
{
    /// <summary>
    /// TextBoxExtPic.xaml 的交互逻辑
    /// </summary>
    public partial class TextBoxExtPic : System.Windows.Controls.UserControl
    {
        public delegate void ButtonClickHandler();
        public TextBoxExtPic()
        {
            InitializeComponent();
        }

        public string Folder
        {
            get { return (string)GetValue(FolderProperty); }
            set { SetValue(FolderProperty, value); }
        }

        public static readonly DependencyProperty FolderProperty = DependencyProperty.Register("Folder", typeof(string), typeof(TextBoxExtPic), new PropertyMetadata("tuyan", onPropertyChange));

        private static void onPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxExtPic v = (TextBoxExtPic)d;
            v.txtFolder.Text = v.Folder;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dilog = new FolderBrowserDialog();
            dilog.Description = "请选择目录";
            DialogResult openFileRes = dilog.ShowDialog();
            if (DialogResult.OK == openFileRes)
            {
                var path = dilog.SelectedPath;
                  Folder = path;
            }

            //if (dilog.ShowDialog() == System.Windows.Forms.DialogResult.OK || dilog.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            //{
            //    var path = dilog.SelectedPath;
            //    Folder = path;
            //}      
        }

        private void txtFolder_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
