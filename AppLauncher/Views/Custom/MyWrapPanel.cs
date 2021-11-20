using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AppLauncher.Views.Custom
{
    public class MyWrapPanel : Panel
    {


        public double RowHeight
        {
            get { return (double)GetValue(RowHeightProperty); }
            set { SetValue(RowHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.Register("RowHeight", typeof(double), typeof(MyWrapPanel), new PropertyMetadata(100d));



        protected override Size MeasureOverride(Size availableSize)
        {
            var size = new Size(0, 0);
            var right = 0d;

            foreach (UIElement child in Children)
            {
                child.Measure(new Size(availableSize.Width, RowHeight));
                if (right + child.DesiredSize.Width <= availableSize.Width)
                {
                    right += child.DesiredSize.Width;
                }
                else
                {
                    right = child.DesiredSize.Width;
                    size.Width = Math.Max(right, size.Width);
                    size.Height += RowHeight;
                }
            }

            size.Width = Math.Min(Math.Max(right, size.Width), availableSize.Width);
            size.Height += RowHeight;

            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var row = 0;
            var right = 0d;
            var rowItems = new Queue<UIElement>();

            bool eachRowOnlyOneItem = true;

            foreach (UIElement child in Children)
            {
                if (right == 0 || (right + child.DesiredSize.Width) <= finalSize.Width)
                {
                    if (right != 0)
                        eachRowOnlyOneItem = false;
                    right += child.DesiredSize.Width;
                }
                else
                {
                    ArrangeRow(rowItems, row++, (finalSize.Width - right) / rowItems.Count);
                    right = child.DesiredSize.Width;
                }
                rowItems.Enqueue(child);
            }

            if (Children.Count != 1 && rowItems.Count == 1 && eachRowOnlyOneItem)
            {
                rowItems.Dequeue().Arrange(new Rect(0, row * RowHeight, finalSize.Width, RowHeight));
            }
            else if (rowItems.Count > 0)
            {
                ArrangeRow(rowItems, row, right <= finalSize.Width ? 0 : ((finalSize.Width - right) / rowItems.Count));
            }

            return finalSize;
        }

        private void ArrangeRow(Queue<UIElement> rowItems, int row, double supply)
        {
            var top = row * RowHeight;
            var left = 0d;

            while (rowItems.Count > 0)
            {
                var item = rowItems.Dequeue();
                item.Arrange(new Rect(left, top, item.DesiredSize.Width + supply, RowHeight));
                left += item.DesiredSize.Width + supply;
            }
        }
    }
}
