using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace AppLauncher.Views
{
    public class BindingProxy : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private object _data;
        public object Data
        {
            get => _data;
            set
            {
                _data = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Data"));
            }
        }
    }
}
