﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AppLauncher.Views.Custom
{
    public class AppItemsPanel : WrapPanel
    {
        public override void EndInit()
        {
            base.EndInit();

            DependencyObject parent = this;
            while (parent != null && !(parent is AppItemListBox))
                parent = VisualTreeHelper.GetParent(parent);
            if (parent is AppItemListBox listBox)
                listBox.UpdateAppItemLayout = AlignChildren;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            AlignChildren();
        }

        public void AlignChildren()
        {
            var list = (from object child in Children select child as FrameworkElement).ToList();
            if (list.Count == 0)
                return;

            foreach (var item in list)
            {
                item.Width = double.NaN;
                item.UpdateLayout();
            }

            var zeroPoint = new Point(0, 0);
            for (int i = 0; i < list.Count - 1; i++)
            {
                var childPoint = list[i].TranslatePoint(zeroPoint, this);
                var nextPoint = list[i + 1].TranslatePoint(zeroPoint, this);
                if (nextPoint.X < childPoint.X)
                {
                    var line = list.Where(item => item.TranslatePoint(zeroPoint, this).Y == childPoint.Y).ToList();
                    var surplusWidth = ActualWidth - childPoint.X - list[i].Margin.Right - list[i].ActualWidth;
                    var averageAddWidth = surplusWidth / line.Count;
                    foreach (var item in line)
                    {
                        item.Width = item.ActualWidth + averageAddWidth;
                        item.UpdateLayout();
                    }
                }
            }
        }
    }
}