using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AppLauncher.Views.UserControls
{
    public class AppLauncherItemBase : UserControl
    {
        public string AppName
        {
            get { return (string)GetValue(AppNameProperty); }
            set { SetValue(AppNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AppName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AppNameProperty =
            DependencyProperty.Register("AppName", typeof(string), typeof(AppLauncherItemBase), new PropertyMetadata(null));


        public string Describe
        {
            get { return (string)GetValue(DescribeProperty); }
            set { SetValue(DescribeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Describe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescribeProperty =
            DependencyProperty.Register("Describe", typeof(string), typeof(AppLauncherItemBase), new PropertyMetadata(null));


        public ImageSource AppIconSource
        {
            get { return (ImageSource)GetValue(AppIconSourceProperty); }
            set { SetValue(AppIconSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AppIconSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AppIconSourceProperty =
            DependencyProperty.Register("AppIconSource", typeof(ImageSource), typeof(AppLauncherItemBase), new PropertyMetadata(null));


        public object OpenCommandParameter
        {
            get { return (object)GetValue(OpenCommandParameterProperty); }
            set { SetValue(OpenCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpenCommandParameterProperty =
            DependencyProperty.Register("OpenCommandParameter", typeof(object), typeof(AppLauncherItemBase), new PropertyMetadata(null));


        public ICommand OpenCommand
        {
            get { return (ICommand)GetValue(OpenCommandProperty); }
            set { SetValue(OpenCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpenCommandProperty =
            DependencyProperty.Register("OpenCommand", typeof(ICommand), typeof(AppLauncherItemBase), new PropertyMetadata(null));


        public object OnMouseDoubleClickCommandParameter
        {
            get { return (object)GetValue(OnMouseDoubleClickCommandParameterProperty); }
            set { SetValue(OnMouseDoubleClickCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnMouseDoubleClickCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnMouseDoubleClickCommandParameterProperty =
            DependencyProperty.Register("OnMouseDoubleClickCommandParameter", typeof(object), typeof(AppLauncherItemBase), new PropertyMetadata(null));


        public ICommand OnMouseDoubleClickCommand
        {
            get { return (ICommand)GetValue(OnMouseDoubleClickCommandProperty); }
            set { SetValue(OnMouseDoubleClickCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnMouseDoubleClickedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnMouseDoubleClickCommandProperty =
            DependencyProperty.Register("OnMouseDoubleClickCommand", typeof(ICommand), typeof(AppLauncherItemBase), new PropertyMetadata(null));


        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (OnMouseDoubleClickCommand != null)
                OnMouseDoubleClickCommand.Execute(OnMouseDoubleClickCommandParameter);
        }
    }
}
