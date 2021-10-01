using SimpleMvvm;

namespace AppLauncher.Models
{
    public class AppItem : NotificationObject
    {
        private string _appName;
        public string AppName
        {
            get => _appName;
            set => UpdateValue(ref _appName, value);
        }

        private string _appPath;
        public string AppPath
        {
            get => _appPath;
            set => UpdateValue(ref _appPath, value);
        }
    }
}
