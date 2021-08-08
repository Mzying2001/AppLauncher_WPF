﻿using AppLauncher.Models;
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
        public MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();

            ViewModel.UpdateAppItemLayoutAction = () => AppItemsBox?.UpdateAppItemLayout();

            var vmProxy = (BindingProxy)Resources["VMProxy"];
            vmProxy.Data = ViewModel;

            if (StaticData.Config.MainWindowInfo != null)
                StaticData.Config.MainWindowInfo.ApplyWindowInfo(this);
            if (WindowState == WindowState.Minimized)
                WindowState = WindowState.Normal;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            StaticData.Config.MainWindowInfo = WindowInfo.GetWindowInfo(this);
            StaticData.Config.AppListListBoxSelectedIndex = AppListListBox.SelectedIndex;
            base.OnClosing(e);
        }

        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            var menu = (ContextMenu)Resources["OptionsMenu"];
            menu.PlacementTarget = sender as UIElement;
            menu.IsOpen = true;
        }
    }
}
