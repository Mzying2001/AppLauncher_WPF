using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace AppLauncher.Views.Custom
{
    public class AppItemListBox : DragableListBox
    {
        public Action UpdateAppItemLayout { get; set; }
    }
}
