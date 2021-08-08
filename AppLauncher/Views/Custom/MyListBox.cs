using AppLauncher.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AppLauncher.Views.Custom
{
    public class MyListBox : ListBox
    {
        public ICommand SelectionChangedCommand
        {
            get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
            set { SetValue(SelectionChangedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionChangedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionChangedCommandProperty =
            DependencyProperty.Register("SelectionChangedCommand", typeof(ICommand), typeof(MyListBox), new PropertyMetadata(null));


        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if (SelectionChangedCommand != null)
                SelectionChangedCommand.Execute(new EventHandlerParamProxy<ListBox, SelectionChangedEventArgs>(this, e));
        }
    }
}
