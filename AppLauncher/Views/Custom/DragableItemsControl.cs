using AppLauncher.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AppLauncher.Views.Custom
{
    public class DragableItemsControl : ItemsControl
    {
        public ICommand OnDropCommand
        {
            get { return (ICommand)GetValue(OnDropCommandProperty); }
            set { SetValue(OnDropCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnDropCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnDropCommandProperty =
            DependencyProperty.Register("OnDropCommand", typeof(ICommand), typeof(DragableItemsControl), new PropertyMetadata(null));


        public ICommand OnPreviewDropCommand
        {
            get { return (ICommand)GetValue(OnPreviewDropCommandProperty); }
            set { SetValue(OnPreviewDropCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnPreviewDropCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnPreviewDropCommandProperty =
            DependencyProperty.Register("OnPreviewDropCommand", typeof(ICommand), typeof(DragableItemsControl), new PropertyMetadata(null));


        public ICommand OnDragOverCommand
        {
            get { return (ICommand)GetValue(OnDragOverCommandProperty); }
            set { SetValue(OnDragOverCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnDragOverCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnDragOverCommandProperty =
            DependencyProperty.Register("OnDragOverCommand", typeof(ICommand), typeof(DragableItemsControl), new PropertyMetadata(null));


        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(DragableItemsControl), new PropertyMetadata(null));


        public override void BeginInit()
        {
            base.BeginInit();
            AllowDrop = true;
            Background = Brushes.White;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            SelectedItem = (VisualTreeHelper.HitTest(this, e.GetPosition(this))?.VisualHit as FrameworkElement)?.DataContext;
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
            if (OnDragOverCommand != null)
                OnDragOverCommand.Execute(new EventHandlerParamProxy<ItemsControl, DragEventArgs>(this, e));

        }

        protected override void OnPreviewDrop(DragEventArgs e)
        {
            base.OnPreviewDrop(e);
            if (OnPreviewDropCommand != null)
                OnPreviewDropCommand.Execute(new EventHandlerParamProxy<ItemsControl, DragEventArgs>(this, e));
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            if (OnDropCommand != null)
                OnDropCommand.Execute(new EventHandlerParamProxy<ItemsControl, DragEventArgs>(this, e));
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

                var item = result.VisualHit as FrameworkElement;
                if (item == null)
                    return;

                DataObject data;
                if (item.DataContext == SelectedItem)
                {
                    data = new DataObject(item.DataContext);
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
