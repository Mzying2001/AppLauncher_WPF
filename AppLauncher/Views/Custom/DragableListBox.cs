using AppLauncher.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AppLauncher.Views.Custom
{
    public class DragableListBox : MyListBox
    {
        public ICommand OnDropCommand
        {
            get { return (ICommand)GetValue(OnDropCommandProperty); }
            set { SetValue(OnDropCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnDropCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnDropCommandProperty =
            DependencyProperty.Register("OnDropCommand", typeof(ICommand), typeof(DragableListBox), new PropertyMetadata(null));


        public ICommand OnPreviewDropCommand
        {
            get { return (ICommand)GetValue(OnPreviewDropCommandProperty); }
            set { SetValue(OnPreviewDropCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnPreviewDropCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnPreviewDropCommandProperty =
            DependencyProperty.Register("OnPreviewDropCommand", typeof(ICommand), typeof(DragableListBox), new PropertyMetadata(null));


        public ICommand OnDragOverCommand
        {
            get { return (ICommand)GetValue(OnDragOverCommandProperty); }
            set { SetValue(OnDragOverCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnDragOverCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnDragOverCommandProperty =
            DependencyProperty.Register("OnDragOverCommand", typeof(ICommand), typeof(DragableListBox), new PropertyMetadata(null));


        public override void BeginInit()
        {
            base.BeginInit();
            AllowDrop = true;
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
            if (OnDragOverCommand != null)
                OnDragOverCommand.Execute(new EventHandlerParamProxy<ListBox, DragEventArgs>(this, e));

        }

        protected override void OnPreviewDrop(DragEventArgs e)
        {
            base.OnPreviewDrop(e);
            if (OnPreviewDropCommand != null)
                OnPreviewDropCommand.Execute(new EventHandlerParamProxy<ListBox, DragEventArgs>(this, e));
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            if (OnDropCommand != null)
                OnDropCommand.Execute(new EventHandlerParamProxy<ListBox, DragEventArgs>(this, e));
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var pos = e.GetPosition(this);
                var result = VisualTreeHelper.HitTest(this, pos);
                if (result == null)
                    return;

                var item = VisualTreeUtils.FindParent<ListBoxItem>(result.VisualHit);
                if (item == null)
                    return;

                DataObject data;
                if (item.DataContext == SelectedItem)
                {
                    data = new DataObject(item.DataContext);
                }
                else if (item.Content == SelectedItem)
                {
                    data = new DataObject(item.Content);
                }
                else
                {
                    return;
                }

                DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
            }
        }
    }
}
