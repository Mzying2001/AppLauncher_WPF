using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace AppLauncher.Models
{
    public class AppList : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<AppItem> _appItems;
        public ObservableCollection<AppItem> AppItems
        {
            get => _appItems;
            set
            {
                _appItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AppItems"));
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        public void AddItem(string name, string path)
        {
            if (AppItems == null)
                AppItems = new ObservableCollection<AppItem>();
            AppItems.Add(new AppItem { AppName = name, AppPath = path });
        }

        public void AddItem(string path)
        {
            path = PathHelper.FormatPath(path);
            if (!System.IO.File.Exists(path))
                throw new Exception("文件不存在。");

            var suffix = PathHelper.GetSuffix(path).ToUpper();
            switch (suffix)
            {
                case "EXE":
                    AddItem(PathHelper.GetFileNameWithoutSuffix(path), path);
                    break;

                case "LNK":
                    var lnk = LnkReader.Lnk.OpenLnk(path);
                    AddItem(lnk.BasePath);
                    break;

                default:
                    throw new Exception($"不支持的扩展名：“{suffix}”。");
            }
        }
    }
}
