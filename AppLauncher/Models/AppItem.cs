using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AppLauncher.Models
{
    public class AppItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _appName;
        public string AppName
        {
            get => _appName;
            set
            {
                _appName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AppName"));
            }
        }

        private string _appPath;
        public string AppPath
        {
            get => _appPath;
            set
            {
                _appPath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AppPath"));
            }
        }
    }
}
