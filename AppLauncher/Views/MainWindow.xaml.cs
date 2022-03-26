using AppLauncher.Models;
using AppLauncher.Utils;
using AppLauncher.ViewModels;
using AppLauncher.Views.Custom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppLauncher.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (StaticData.Config.MainWindowInfo != null)
                StaticData.Config.MainWindowInfo.ApplyWindowInfo(this);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            WindowState = WindowState.Normal;
            StaticData.Config.MainWindowInfo = WindowInfo.GetWindowInfo(this);
            StaticData.Config.AppListListBoxSelectedIndex = AppListListBox.SelectedIndex;
            base.OnClosing(e);
        }

        private void ShowOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            var menu = (ContextMenu)Resources["OptionsMenu"];
            menu.PlacementTarget = sender as UIElement;
            menu.IsOpen = true;
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(hwnd).AddHook(new HwndSourceHook(WndProc));
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == 0x0400)
            {
                if (WindowState == WindowState.Minimized)
                    WindowState = WindowState.Normal;
                WindowHelper.SetForeground(this);
                handled = true;
            }
            return IntPtr.Zero;
        }
    }
}
