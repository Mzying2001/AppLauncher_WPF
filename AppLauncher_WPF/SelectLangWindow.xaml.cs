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
using System.Windows.Shapes;

namespace AppLauncher_WPF
{
    /// <summary>
    /// SelectLangWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SelectLangWindow : Window
    {

        string selected_language = "ZH"; //默认选中中文

        public SelectLangWindow()
        {
            InitializeComponent();
        }

        public new string ShowDialog()
        {
            base.ShowDialog();
            return selected_language;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ZH_Checked(object sender, RoutedEventArgs e)
        {
            selected_language = "ZH";
        }

        private void EN_Checked(object sender, RoutedEventArgs e)
        {
            selected_language = "EN";
        }
    }
}
