using AppLauncher.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppLauncher.Views
{
    /// <summary>
    /// EditAppItemWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditAppItemWindow : Window
    {
        public AppItem AppItem
        {
            get { return (AppItem)GetValue(AppItemProperty); }
            set { SetValue(AppItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AppItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AppItemProperty =
            DependencyProperty.Register("AppItem", typeof(AppItem), typeof(EditAppItemWindow), new PropertyMetadata(null, (obj, e) =>
            {
                if (obj is EditAppItemWindow window && e.NewValue is AppItem app)
                {
                    window.tb_name.Text = app.AppName;
                    window.tb_path.Text = app.AppPath;
                }
            }));


        public EditAppItemWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            tb_name.Text = tb_name.Text.Trim();
            tb_path.Text = tb_path.Text.Trim();

            if (string.IsNullOrEmpty(tb_name.Text))
            {
                MsgBoxHelper.ShowError("名称不能为空。");
                return;
            }
            if (!File.Exists(tb_path.Text) || !tb_path.Text.ToUpper().EndsWith(".EXE"))
            {
                MsgBoxHelper.ShowError("文件不存在或不支持。");
                return;
            }

            AppItem.AppName = tb_name.Text;
            AppItem.AppPath = tb_path.Text;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
