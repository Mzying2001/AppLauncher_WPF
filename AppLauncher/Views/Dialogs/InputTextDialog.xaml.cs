using AppLauncher.Utils;
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
        private Action<string> _callback;


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


        public InputTextDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tb.Focus();
            tb.SelectAll();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                MsgBoxHelper.ShowMessage("请输入文本。");
            }
            else
            {
                _callback?.Invoke(Text);
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public static void ShowDialog(Action<string> callback, string title = null, string message = null, string defaultText = null)
        {
            new InputTextDialog
            {
                Text = defaultText,
                Title = title,
                Message = message,
                _callback = callback,
            }.ShowDialog();
        }
    }
}
