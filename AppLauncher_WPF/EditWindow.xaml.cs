using App;
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


        private readonly AppList al;


        public EditWindow(AppList al)
        {
            InitializeComponent();

            this.al = al;
            foreach(Appconf ac in this.al)
            {
                Lb_Apps.Items.Add(ac.AppName);
            }
            Lb_Apps.SelectedIndex = 0;
            Lb_Apps_SelectionChanged(null, null);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => al.Save();

        public new AppList ShowDialog()
        {
            base.ShowDialog();
            return al;
        }

        private void Lb_Apps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = Lb_Apps.SelectedIndex;
            if (index >= 0 && index < al.Count)
            {
                Gb.IsEnabled = true;
                B_Del.IsEnabled = true;
                Tb_AppName.Text = al[index].AppName;
                Tb_AppPath.Text = al[index].AppPath;
            }
            else
            {
                Gb.IsEnabled = false;
                B_Del.IsEnabled = false;
                Tb_AppName.Text = null;
                Tb_AppPath.Text = null;
            }
        }

        private void AddItem(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog()
                {
                    Filter = $"{FindResource("EditWindow_Filter_App")}|*.exe"
                };
                if (ofd.ShowDialog() == true)
                {
                    string file = ofd.FileName;
                    string path = $"{AppDomain.CurrentDomain.BaseDirectory}APPCONF\\{DateTime.Now.Ticks}.appconf";

                    Appconf ac = Appconf.Create(path, file.Substring(file.LastIndexOf('\\') + 1, file.LastIndexOf('.') - file.LastIndexOf('\\') - 1), file);
                    al.Add(ac);
                    Lb_Apps.Items.Add(ac.AppName);
                    Lb_Apps.SelectedIndex = Lb_Apps.Items.Count - 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, FindResource("MessageBoxTitle_Error") as string);
            }
        }

        private void RemoveItem(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = Lb_Apps.SelectedIndex;

                MessageBoxResult result = MessageBox.Show(
                    string.Format(FindResource("EditWindow_Message_AreYouSureToRemove") as string, al[index].AppName),
                    FindResource("MessageBoxTitle_Message") as string,
                    MessageBoxButton.YesNo
                    );

                if (result == MessageBoxResult.Yes)
                {
                    Lb_Apps.Items.RemoveAt(index);
                    al.RemoveAt(index);

                    if (Lb_Apps.Items.Count > 0)
                    {
                        index = index > 0 ? index - 1 : index;
                        Lb_Apps.SelectedIndex = index;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, FindResource("MessageBoxTitle_Error") as string);
            }
        }

        private void Browse(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = $"{FindResource("EditWindow_Filter_App")}|*.exe"
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

                if (string.IsNullOrEmpty(Tb_AppName.Text))
                    throw new Exception(FindResource("EditWindow_Message_AppNameIsEmpty") as string);

                if (!File.Exists(Tb_AppPath.Text))
                    throw new Exception(string.Format(FindResource("EditWindow_Message_PathInexist") as string, Tb_AppPath.Text));

                if (!(Tb_AppPath.Text.EndsWith(".exe") || Tb_AppPath.Text.EndsWith(".EXE")))
                    throw new Exception(string.Format(FindResource("EditWindow_Message_NoApplication") as string, Tb_AppPath.Text));

                int index = Lb_Apps.SelectedIndex;
                al[index].AppName = Tb_AppName.Text;
                al[index].AppPath = Tb_AppPath.Text;
                Lb_Apps.Items.RemoveAt(index);
                Lb_Apps.Items.Insert(index, al[index].AppName);
                Lb_Apps.SelectedIndex = index;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, FindResource("MessageBoxTitle_Error") as string);
            }
        }

        private void Up(object sender, RoutedEventArgs e)
        {
            int index = Lb_Apps.SelectedIndex;
            if (index > 0)
            {
                Appconf tmp1 = al[index];
                Appconf tmp2 = al[index - 1];
                al.RemoveAt(index, false);
                al.Insert(index, tmp2);
                al.RemoveAt(index - 1, false);
                al.Insert(index - 1, tmp1);

                Lb_Apps.Items.RemoveAt(index);
                Lb_Apps.Items.Insert(index, al[index].AppName);
                Lb_Apps.Items.RemoveAt(index - 1);
                Lb_Apps.Items.Insert(index - 1, al[index - 1].AppName);

                Lb_Apps.SelectedIndex = index - 1;
            }
        }

        private void Down(object sender, RoutedEventArgs e)
        {
            int index = Lb_Apps.SelectedIndex;
            if (index < Lb_Apps.Items.Count - 1)
            {
                Appconf tmp1 = al[index];
                Appconf tmp2 = al[index + 1];
                al.RemoveAt(index, false);
                al.Insert(index, tmp2);
                al.RemoveAt(index + 1, false);
                al.Insert(index + 1, tmp1);

                Lb_Apps.Items.RemoveAt(index);
                Lb_Apps.Items.Insert(index, al[index].AppName);
                Lb_Apps.Items.RemoveAt(index + 1);
                Lb_Apps.Items.Insert(index + 1, al[index + 1].AppName);

                Lb_Apps.SelectedIndex = index + 1;
            }
        }

        private void Tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Change(null, null);

                TextBox tb = (TextBox)sender;
                tb.Select(tb.Text.Length, 0);
            }
        }
    }
}
