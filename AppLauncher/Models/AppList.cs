using AppLauncher.Utils;
using SimpleMvvm;
using System;
using System.Collections.ObjectModel;

namespace AppLauncher.Models
{
    public class AppList : NotificationObject
    {
        private ObservableCollection<AppItem> _appItems;
        public ObservableCollection<AppItem> AppItems
        {
            get => _appItems;
            set => UpdateValue(ref _appItems, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => UpdateValue(ref _name, value);
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
