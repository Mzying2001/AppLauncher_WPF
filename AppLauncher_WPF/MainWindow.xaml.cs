using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using App;
using Config;
using Launcher;

namespace AppLauncher_WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {


        private AppList al;
        readonly WindowConfig wc;


        public MainWindow()
        {
            InitializeComponent();

            try
            {//读取AppList

                string path_applist = AppDomain.CurrentDomain.BaseDirectory + "AppList.bin"; //AppList文件路径
                string path_appconf = AppDomain.CurrentDomain.BaseDirectory + "APPCONF";     //应用程序配置文件文件夹路径

                /*判断AppList文件是否存在，不存在则创建该文件*/
                if (!File.Exists(path_applist))
                    File.WriteAllText(path_applist, null);

                /*判断应用程序配置文件文件夹路径是否存在，不存在则创建该路径*/
                if (!Directory.Exists(path_appconf))
                    Directory.CreateDirectory(path_appconf);

                /*创建AppList*/
                al = new AppList(path_applist, path_appconf);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "错误");
                Close();
            }

            /*载入应用程序*/
            LoadApps();

            try
            {//读取窗口配置

                wc = new WindowConfig(AppDomain.CurrentDomain.BaseDirectory + "Window.ini");

                if (!wc.IsFileExists)
                {
                    return;
                }

                double x = wc.X; //左边
                double y = wc.Y; //顶边
                double w = Math.Abs(wc.Width);  //宽度
                double h = Math.Abs(wc.Height); //高度

                /*赋值*/
                Left   = x;
                Top    = y;
                Width  = w;
                Height = h;

                /*左边和顶边初始不能为0*/
                Left = (Left < 0) ? 0 : Left;
                Top  = (Top < 0) ? 0 : Top;

                /*获取屏幕宽度和高度*/
                double sWidth  = SystemParameters.PrimaryScreenWidth; //屏幕宽度
                double sHeight = SystemParameters.PrimaryScreenHeight;//屏幕高度

                /*窗口不能超出屏幕范围*/
                Left = (Left > sWidth - Width) ? sWidth - Width : Left;
                Top  = (Top > sHeight - Height) ? sHeight - Height : Top;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "窗口配置错误");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*保存窗口状态*/
            wc.X = Left;
            wc.Y = Top;
            wc.Width  = Width;
            wc.Height = Height;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Table.Width = Scroll.Width - Scroll.Width % 260;
            Table.Height = 110 * Table.Width / 260;
        }

        /// <summary>
        /// 菜单“编辑App列表”被单击
        /// </summary>
        private void EditItems(object sender, RoutedEventArgs e)
        {
            MainGrid.Effect = new BlurEffect() { Radius = 25 };
            al = new EditWindow(al).ShowDialog();
            LoadApps();
            MainGrid.Effect = null;
        }

        /// <summary>
        /// 菜单“关于”被单击
        /// </summary>
        private void About(object sender, RoutedEventArgs e)
        {
            MainGrid.Effect = new BlurEffect() { Radius = 25 };
            MessageBox.Show($"AppLauncher v{Application.ResourceAssembly.GetName().Version} by Mzying2001 (颖)", "关于");
            MainGrid.Effect = null;
        }

        /// <summary>
        /// 载入应用程序
        /// </summary>
        private void LoadApps()
        {
            Table.Children.Clear();
            foreach(Appconf ac in al)
            {
                Table.Children.Add(new Item(ac));
            }
        }
    }
}
