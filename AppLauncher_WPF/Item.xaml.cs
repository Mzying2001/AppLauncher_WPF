using App;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Launcher;

namespace AppLauncher_WPF
{
    /// <summary>
    /// Item.xaml 的交互逻辑
    /// </summary>
    public partial class Item : UserControl
    {


        private readonly Appconf ac;


        public Item(Appconf ac)
        {
            InitializeComponent();

            this.ac = ac;
            AppName = ac.AppName;
            FileName = ac.AppPath.Substring(ac.AppPath.LastIndexOf('\\') + 1);

            try
            {
                Image_AppIcon.Source = GetIcon(ac.AppPath);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "错误");
            }
        }

        public string AppName
        {
            get
            {
                return Label_AppName.Content.ToString();
            }
            private set
            {
                Label_AppName.Content = value.Replace("_", "__");
            }
        }

        public string FileName
        {
            get
            {
                return Label_FileName.Content.ToString();
            }
            private set
            {
                Label_FileName.Content = value.Replace("_", "__");
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EXELauncher.Start(ac);
            }
            catch (Exception)
            {
            }
        }

        private static ImageSource GetIcon(string fileName)
        {
            System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(fileName);
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                        icon.Handle,
                        new Int32Rect(0, 0, icon.Width, icon.Height),
                        BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
