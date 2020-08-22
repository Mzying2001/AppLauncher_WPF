﻿using App;
using Microsoft.Win32;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppLauncher_WPF
{
    /// <summary>
    /// EditWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditWindow : Window
    {


        private readonly AppList AL;


        public EditWindow(AppList al)
        {
            InitializeComponent();

            AL = al;
            foreach(Appconf ac in AL)
            {
                Lb_Apps.Items.Add(ac.AppName);
            }
            Lb_Apps.SelectedIndex = 0;
            Lb_Apps_SelectionChanged(null, null);
        }

        public new AppList ShowDialog()
        {
            base.ShowDialog();
            return AL;
        }

        private void Lb_Apps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = Lb_Apps.SelectedIndex;
            if (index >= 0 && index < AL.Count)
            {
                Gb.IsEnabled = true;
                Tb_AppName.Text = AL[index].AppName;
                Tb_AppPath.Text = AL[index].AppPath;
            }
            else
            {
                Gb.IsEnabled = false;
                Tb_AppName.Text = "NULL";
                Tb_AppPath.Text = "NULL";
            }
        }

        private void AddItem(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog()
                {
                    Filter = "应用程序|*.exe",
                };
                if (ofd.ShowDialog() == true)
                {
                    string file = ofd.FileName;
                    string path = $"{AppDomain.CurrentDomain.BaseDirectory}APPCONF\\{DateTime.Now.Ticks}.appconf";

                    Appconf ac = Appconf.Create(path, file.Substring(file.LastIndexOf('\\') + 1, file.LastIndexOf('.') - file.LastIndexOf('\\') - 1), file);
                    AL.Add(ac);
                    Lb_Apps.Items.Add(ac.AppName);
                    Lb_Apps.SelectedIndex = Lb_Apps.Items.Count - 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }
        }

        private void DelItem(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = Lb_Apps.SelectedIndex;
                if (MessageBox.Show($"是否删除\"{AL[index].AppName}\"？", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Lb_Apps.Items.RemoveAt(index);
                    AL.RemoveAt(index);

                    if (Lb_Apps.Items.Count > 0)
                    {
                        index = index > 0 ? index - 1 : index;
                        Lb_Apps.SelectedIndex = index;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }
        }

        private void Browse(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "应用程序|*.exe"
            };
            if (ofd.ShowDialog() == true)
            {
                Tb_AppPath.Text = ofd.FileName;
            }
        }

        private void Change(object sender, RoutedEventArgs e)
        {
            try
            {
                Tb_AppName.Text = Tb_AppName.Text.Trim();
                Tb_AppPath.Text = Tb_AppPath.Text.Trim().Replace('/', '\\');

                if (!File.Exists(Tb_AppPath.Text))
                    throw new Exception($"路径\"{Tb_AppPath}\"不存在");

                if (!(Tb_AppPath.Text.EndsWith(".exe") || Tb_AppPath.Text.EndsWith(".EXE")))
                    throw new Exception($"路径\"{Tb_AppPath.Text}\"所指向的文件不是应用程序");

                if (string.IsNullOrEmpty(Tb_AppName.Text))
                    throw new Exception("程序名不能为空");

                int index = Lb_Apps.SelectedIndex;
                AL[index].AppName = Tb_AppName.Text;
                AL[index].AppPath = Tb_AppPath.Text;
                Lb_Apps.Items.RemoveAt(index);
                Lb_Apps.Items.Insert(index, AL[index].AppName);
                Lb_Apps.SelectedIndex = index;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }
        }

        private void Up(object sender, RoutedEventArgs e)
        {
            int index = Lb_Apps.SelectedIndex;
            if (index > 0)
            {
                Appconf tmp1 = AL[index];
                Appconf tmp2 = AL[index - 1];
                AL.RemoveAt(index, false);
                AL.Insert(index, tmp2);
                AL.RemoveAt(index - 1, false);
                AL.Insert(index - 1, tmp1);

                Lb_Apps.Items.RemoveAt(index);
                Lb_Apps.Items.Insert(index, AL[index].AppName);
                Lb_Apps.Items.RemoveAt(index - 1);
                Lb_Apps.Items.Insert(index - 1, AL[index - 1].AppName);

                Lb_Apps.SelectedIndex = index - 1;
            }
        }

        private void Down(object sender, RoutedEventArgs e)
        {
            int index = Lb_Apps.SelectedIndex;
            if (index < Lb_Apps.Items.Count - 1)
            {
                Appconf tmp1 = AL[index];
                Appconf tmp2 = AL[index + 1];
                AL.RemoveAt(index, false);
                AL.Insert(index, tmp2);
                AL.RemoveAt(index + 1, false);
                AL.Insert(index + 1, tmp1);

                Lb_Apps.Items.RemoveAt(index);
                Lb_Apps.Items.Insert(index, AL[index].AppName);
                Lb_Apps.Items.RemoveAt(index + 1);
                Lb_Apps.Items.Insert(index + 1, AL[index + 1].AppName);

                Lb_Apps.SelectedIndex = index + 1;
            }
        }

        private void Tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Change(null, null);

            TextBox tb = (TextBox)sender;
            tb.Select(tb.Text.Length, 0);
        }
    }
}
