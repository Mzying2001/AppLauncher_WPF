using AppLauncher.Commands;
using AppLauncher.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace AppLauncher.ViewModels
{
    public class EditAppItemWindowViewModel : ViewModelBase
    {
        public ICommand CancelCommand { get; set; }
        public ICommand OkCommand { get; set; }

        public Action CloseWindowAction { get; set; }

        private AppItem _appItem;
        public AppItem AppItem
        {
            get => _appItem;
            set
            {
                _appItem = value;
                Name = value.AppName;
                Path = value.AppPath;
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        private string _path;
        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                RaisePropertyChanged("Path");
            }
        }

        private void CloseWindow()
        {
            CloseWindowAction?.Invoke();
        }

        private void Cancel(object obj)
        {
            CloseWindow();
        }

        private void Ok(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MsgBoxHelper.ShowError("名称不能为空。");
                return;
            }
            if (!File.Exists(Path) || PathHelper.GetSuffix(Path).ToUpper() != "EXE")
            {
                MsgBoxHelper.ShowError("文件不存在或不支持。");
                return;
            }
            AppItem.AppName = Name;
            AppItem.AppPath = Path;
            CloseWindow();
        }

        public EditAppItemWindowViewModel()
        {
            CancelCommand = new DelegateCommand { Execute = Cancel };
            OkCommand = new DelegateCommand { Execute = Ok };
        }
    }
}
