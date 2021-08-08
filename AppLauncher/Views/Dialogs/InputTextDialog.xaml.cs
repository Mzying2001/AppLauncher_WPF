using AppLauncher.ViewModels.DialogViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppLauncher.Views.Dialogs
{
    /// <summary>
    /// InputTextDialog.xaml 的交互逻辑
    /// </summary>
    public partial class InputTextDialog : Window
    {


        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(InputTextDialog), new PropertyMetadata(null));


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(InputTextDialog), new PropertyMetadata(null));


        public InputTextDialogViewModel ViewModel => (InputTextDialogViewModel)DataContext;

        public InputTextDialog()
        {
            InitializeComponent();

            ViewModel.CloseWindowAction = Close;
            ViewModel.ClearTextBoxAction = () => Text = string.Empty;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tb.Focus();
            tb.SelectAll();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!ViewModel.Ok_Invoked)
                Text = string.Empty;
        }

        public new string ShowDialog()
        {
            base.ShowDialog();
            return string.IsNullOrEmpty(Text) ? null : Text;
        }
    }
}
