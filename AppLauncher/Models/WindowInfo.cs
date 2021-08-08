using System;
using System.Collections.Generic;
using System.Text;

namespace AppLauncher.Models
{
    public class WindowInfo
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public System.Windows.WindowState WindowState { get; set; }

        public void ApplyWindowInfo(System.Windows.Window window)
        {
            window.Left = Left;
            window.Top = Top;
            window.Width = Width;
            window.Height = Height;
            window.WindowState = WindowState;
        }

        public static WindowInfo GetWindowInfo(System.Windows.Window window)
        {
            return new WindowInfo
            {
                Left = window.Left,
                Top = window.Top,
                Width = window.Width,
                Height = window.Height,
                WindowState = window.WindowState
            };
        }
    }
}
