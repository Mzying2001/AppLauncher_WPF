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
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

namespace AppLauncher_WPF
{
    /// <summary>
    /// Item.xaml 的交互逻辑
    /// </summary>
    public partial class Item : UserControl
    {


        private readonly Appconf ac;

        public RoutedEventHandler AppStarted;


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
                MessageBox.Show(e.Message, FindResource("MessageBoxTitle_Error") as string);
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
                AppStarted?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, FindResource("MessageBoxTitle_Error") as string);
            }
        }

        public void ChangeAppName(string name)
        {
            ac.AppName = name;
            AppName = name;
        }

        private void MenuItem_Explore_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("Explorer.exe")
            {
                Arguments = "/e,/select," + ac.AppPath
            });
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowFileProperties(ac.AppPath);
        }

        #region 获取App图标
        private static ImageSource GetIcon(string fileName)
        {
            System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(fileName);
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                        icon.Handle,
                        new Int32Rect(0, 0, icon.Width, icon.Height),
                        BitmapSizeOptions.FromEmptyOptions());
        }
        #endregion

        #region 查看文件属性
        //来自：https://bbs.csdn.net/topics/370078839

        [StructLayout(LayoutKind.Sequential)]
        public struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }

        private const int SW_SHOW = 5;
        private const uint SEE_MASK_INVOKEIDLIST = 12;

        [DllImport("shell32.dll")]
        static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

        public static void ShowFileProperties(string Filename)
        {
            SHELLEXECUTEINFO info = new SHELLEXECUTEINFO();
            info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(info);
            info.lpVerb = "properties";
            info.lpFile = Filename;
            info.nShow = SW_SHOW;
            info.fMask = SEE_MASK_INVOKEIDLIST;
            ShellExecuteEx(ref info);
        }
        #endregion
    }
}
