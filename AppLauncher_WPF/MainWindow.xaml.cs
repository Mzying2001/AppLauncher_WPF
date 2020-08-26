using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace AppLauncher_WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        struct Conf
        {
            public AppLauncherConfig alc;
            public WindowConfig      wc;
        }

        private AppList al;
        readonly Conf conf;

        /*支持切换的语言*/
        readonly string[] supported_language = { "ZH", "EN" };

        /*当前语言*/
        private string _current = "ZH"; //默认为中文
        private string CurrentLanguage
        {
            get => _current;

            set
            {
                if (!supported_language.Contains(value) || _current.Equals(value))
                    return;

                foreach (MenuItem mi in LangSwitch.Items)
                {
                    mi.IsChecked = false;
                }

                switch (value)
                {
                    case "ZH":
                        LangSwitch_ZH.IsChecked = true;
                        break;

                    case "EN":
                        LangSwitch_EN.IsChecked = true;
                        break;

                    default:
                        return;
                }

                Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary()
                {
                    Source = new Uri($@"Language\{value}.xaml", UriKind.Relative)
                };
                _current = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            conf = new Conf
            {
                alc = new AppLauncherConfig(AppDomain.CurrentDomain.BaseDirectory + "Config.ini"),
                wc  = new WindowConfig(AppDomain.CurrentDomain.BaseDirectory + "Window.ini")
            };

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
                MessageBox.Show(e.Message, FindResource("MessageBoxTitle_Error") as string);
                Close();
            }

            /*载入应用程序*/
            LoadApps();

            /*设置语言*/
            if (conf.alc.IsFileExists)
            {
                CurrentLanguage = conf.alc.Language;
            }
            else
            {
                CurrentLanguage = new SelectLangWindow().ShowDialog();
            }

            /*窗口是否要顶置*/
            Topmost = conf.alc.WindowTopmost;

            try
            {//读取窗口配置

                if (!conf.wc.IsFileExists)
                {
                    return;
                }

                double x = conf.wc.X; //左边
                double y = conf.wc.Y; //顶边
                double w = Math.Abs(conf.wc.Width);  //宽度
                double h = Math.Abs(conf.wc.Height); //高度

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
                MessageBox.Show(e.Message, FindResource("MessageBoxTitle_WindowConfError") as string);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*保存窗口状态*/
            conf.wc.X = Left;
            conf.wc.Y = Top;
            conf.wc.Width  = Width;
            conf.wc.Height = Height;

            /*保存当前语言*/
            conf.alc.Language = CurrentLanguage;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Table.Width = Scroll.Width - Scroll.Width % 260;
            Table.Height = 110 * Table.Width / 260;
        }

        /// <summary>
        /// 载入应用程序
        /// </summary>
        private void LoadApps()
        {
            Table.Children.Clear();
            foreach (Appconf ac in al)
            {
                Item i = new Item(ac);

                Table.Children.Add(i);
                i.Button_Start.Click += AppStarted;
            }
        }

        /// <summary>
        /// 单击启动按钮
        /// </summary>
        private void AppStarted(object sender, RoutedEventArgs e)
        {
            if (conf.alc.MinAfterLaunch)
                WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 操作时窗口模糊
        /// </summary>
        private void DoWithBlurEffect(Action func)
        {
            MainGrid.Effect = new BlurEffect() { Radius = 25 };
            func();
            MainGrid.Effect = null;
        }

        /// <summary>
        /// 菜单“编辑App列表”被单击
        /// </summary>
        private void EditItems_Click(object sender, RoutedEventArgs e)
        {
            DoWithBlurEffect(() =>
            {
                al = new EditWindow(al) { Owner = this }.ShowDialog();
                LoadApps();
            });
        }

        /// <summary>
        /// 查看源代码
        /// </summary>
        private void ViewSourceCode_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/Mzying2001/AppLauncher_WPF");
        }

        /// <summary>
        /// 菜单“关于”被单击
        /// </summary>
        private void About_Click(object sender, RoutedEventArgs e)
        {
            DoWithBlurEffect(() =>
            {
                MessageBox.Show($"AppLauncher v{Application.ResourceAssembly.GetName().Version} by Mzying2001 (颖)", FindResource("MessageBoxTitle_About") as string);
            });
        }

        /// <summary>
        /// 切换中文
        /// </summary>
        private void LangSwitch_ZH_Click(object sender, RoutedEventArgs e)
        {
            CurrentLanguage = "ZH";
        }

        /// <summary>
        /// 切换英文
        /// </summary>
        private void LangSwitch_EN_Click(object sender, RoutedEventArgs e)
        {
            CurrentLanguage = "EN";
        }

        /// <summary>
        /// 启动App后最小化窗口
        /// </summary>
        private void MinAfterLaunch_Click(object sender, RoutedEventArgs e)
        {
            conf.alc.MinAfterLaunch = !conf.alc.MinAfterLaunch;
        }

        /// <summary>
        /// 窗口置顶
        /// </summary>
        private void WindowTopmost_Click(object sender, RoutedEventArgs e)
        {
            Topmost = !conf.alc.WindowTopmost;
            conf.alc.WindowTopmost = Topmost;
        }

        /// <summary>
        /// 打开菜单“选项”
        /// </summary>
        private void MenuItem_Options_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            MinAfterLaunch.IsChecked = conf.alc.MinAfterLaunch;
            WindowTopmost.IsChecked = conf.alc.WindowTopmost;
        }
    }
}
