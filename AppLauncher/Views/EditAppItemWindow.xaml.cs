using AppLauncher.Models;
using AppLauncher.ViewModels;
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
        EditAppItemWindowViewModel ViewModel => (EditAppItemWindowViewModel)DataContext;

        public AppItem AppItem
        {
            set => ViewModel.AppItem = value;
        }

        public EditAppItemWindow()
        {
            InitializeComponent();
            ViewModel.CloseWindowAction = Close;
        }
    }
}
