using AppLauncher.Models;
using SimpleMvvm;
using SimpleMvvm.Command;
using System;
using System.IO;

namespace AppLauncher.ViewModels
{
    public class EditAppItemWindowViewModel : ViewModelBase
    {
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand OkCommand { get; set; }

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
            set => UpdateValue(ref _name, value);
        }

        private string _path;
        public string Path
        {
            get => _path;
            set => UpdateValue(ref _path, value);
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

        protected override void Init()
        {
            base.Init();

            CancelCommand = new DelegateCommand(Cancel);
            OkCommand = new DelegateCommand(Ok);
        }
    }
}
