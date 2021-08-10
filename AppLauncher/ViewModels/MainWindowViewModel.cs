using AppLauncher.Commands;
using AppLauncher.Models;
using AppLauncher.Views;
using AppLauncher.Views.Custom;
using AppLauncher.Views.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AppLauncher.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand OpenAppCommand { get; set; }
        public ICommand NewAppListCommand { get; set; }
        public ICommand RenameAppListCommand { get; set; }
        public ICommand RemoveAppListCommand { get; set; }
        public ICommand ShowPreviousAppListCommand { get; set; }
        public ICommand ShowNextAppListCommand { get; set; }
        public ICommand AddAppCommand { get; set; }
        public ICommand EditAppItemCommand { get; set; }
        public ICommand RemoveAppItemCommand { get; set; }
        public ICommand ShowAboutCommand { get; set; }
        public ICommand AppItemListBoxOnDropCommand { get; set; }
        public ICommand AppItemListBoxOnDragOverCommand { get; set; }
        public ICommand AppListListBoxOnDragOverCommand { get; set; }
        public ICommand AppListListBoxSelectionChangedCommand { get; set; }
        public ICommand RenameAppItemCommand { get; set; }
        public ICommand ViewSourceCommand { get; set; }
        public ICommand ShowInExplorerCommand { get; set; }
        public ICommand ToggleWindowTopmostCommand { get; set; }
        public ICommand ToggleMinimizeWindowAfterOpeningCommand { get; set; }
        public ICommand ToggleShowOpenErrorMsgCommand { get; set; }

        public Action UpdateAppItemLayoutAction { get; set; }

        private WindowState _windowState;
        public WindowState WindowState
        {
            get => _windowState;
            set
            {
                _windowState = value;
                RaisePropertyChanged("WindowState");
            }
        }

        private int _appListListBoxSelectedIndex;
        public int AppListListBoxSelectedIndex
        {
            get => _appListListBoxSelectedIndex;
            set
            {
                _appListListBoxSelectedIndex = value;
                RaisePropertyChanged("AppListListBoxSelectedIndex");
            }
        }

        private bool _windowTopmost;
        public bool WindowTopmost
        {
            get => _windowTopmost;
            set
            {
                _windowTopmost = value;
                RaisePropertyChanged("WindowTopmost");
            }
        }

        private bool _minimizeWindowAfterOpening;
        public bool MinimizeWindowAfterOpening
        {
            get => _minimizeWindowAfterOpening;
            set
            {
                _minimizeWindowAfterOpening = value;
                RaisePropertyChanged("MinimizeWindowAfterOpening");
            }
        }

        private bool _showOpenErrorMsg;
        public bool ShowOpenErrorMsg
        {
            get => _showOpenErrorMsg;
            set
            {
                _showOpenErrorMsg = value;
                RaisePropertyChanged("ShowOpenErrorMsg");
            }
        }

        private AppList CurrentSelectedAppList => StaticData.AppLists[AppListListBoxSelectedIndex];

        private void OpenApp(AppItem app)
        {
            var path = PathHelper.FormatPath(app.AppPath);
            var info = Executer.ShellExecute(IntPtr.Zero, "open", path, string.Empty,
                PathHelper.GetLocatedFolderPath(path), Executer.ShowCommands.SW_SHOWNORMAL);

            if ((int)info < 32 && ShowOpenErrorMsg)
                MsgBoxHelper.ShowError($"启动时发生错误：{Executer.GetErrorStr(info)}");
            else
            {
                if (MinimizeWindowAfterOpening)
                    WindowState = WindowState.Minimized;
            }
        }

        private void NewAppList(object obj)
        {
            var name = new InputTextDialog
            {
                Title = "新分类",
                Message = "请输入新分类的名称：",
                Text = "新分类"
            }.ShowDialog();
            if (!string.IsNullOrEmpty(name))
            {
                StaticData.AddAppList(name);
                AppListListBoxSelectedIndex = StaticData.AppLists.Count - 1;
            }
        }

        private void RenameAppList(AppList appList)
        {
            var newName = new InputTextDialog
            {
                Title = $"重命名“{appList.Name}”",
                Message = "请输入新名称：",
                Text = appList.Name
            }.ShowDialog();
            if (!string.IsNullOrEmpty(newName))
                appList.Name = newName;
        }

        private void RemoveAppList(AppList appList)
        {
            var flag = MsgBoxHelper.ShowQuestion($"确实要删除“{appList.Name}”吗？", "提示");
            if (flag == MessageBoxResult.Yes)
            {
                int oldIndex = AppListListBoxSelectedIndex;
                StaticData.AppLists.Remove(appList);
                if (StaticData.AppLists.Count > 0)
                {
                    AppListListBoxSelectedIndex = oldIndex == 0 ? 0 : oldIndex - 1;
                    UpdateAppItemLayoutAction();
                }
            }
        }

        private void ShowPreviousAppList(object obj)
        {
            if (AppListListBoxSelectedIndex > 0)
                AppListListBoxSelectedIndex--;
        }

        private void ShowNextAppList(object obj)
        {
            if (AppListListBoxSelectedIndex < StaticData.AppLists.Count - 1)
                AppListListBoxSelectedIndex++;
        }

        private void AddApp(object obj)
        {
            try
            {
                if (StaticData.AppLists.Count == 0)
                    NewAppList(null);
                if (StaticData.AppLists.Count == 0)
                    return;

                var ofd = new OpenFileDialog
                {
                    Filter = "应用程序|*.exe"
                };
                if (ofd.ShowDialog() == true)
                {
                    CurrentSelectedAppList.AddItem(ofd.FileName);
                }
            }
            catch (Exception e)
            {
                MsgBoxHelper.ShowError(e.Message);
            }
        }

        private void EditAppItem(AppItem app)
        {
            new EditAppItemWindow { AppItem = app }.ShowDialog();
            UpdateAppItemLayoutAction();
        }

        private void RemoveAppItem(AppItem app)
        {
            CurrentSelectedAppList.AppItems.Remove(app);
            UpdateAppItemLayoutAction();
        }

        private void ShowAbout(object obj)
        {
            var assemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName();
            MsgBoxHelper.ShowMessage(
                $"{assemblyName.Name} v{assemblyName.Version} | by Mzying2001", "关于");
        }

        private void AppItemListBoxOnDrop(EventHandlerParamProxy<ListBox, DragEventArgs> proxy)
        {
            if (StaticData.AppLists.Count == 0)
                NewAppList(null);
            if (StaticData.AppLists.Count == 0)
                return;

            var data = proxy.EventArgs.Data.GetData(DataFormats.FileDrop);
            if (data is string[] arr)
            {
                foreach (var item in arr)
                {
                    try
                    {
                        CurrentSelectedAppList.AddItem(item);
                    }
                    catch (Exception e)
                    {
                        MsgBoxHelper.ShowError(e.Message);
                    }
                }
                UpdateAppItemLayoutAction();
            }
        }

        private void AppItemListBoxOnDragOver(EventHandlerParamProxy<ListBox, DragEventArgs> proxy)
        {
            var listbox = proxy.Sender;
            var args = proxy.EventArgs;

            var pos = args.GetPosition(listbox);
            var res = VisualTreeHelper.HitTest(listbox, pos);
            if (res == null)
                return;

            var item = VisualTreeUtils.FindParent<ListBoxItem>(res.VisualHit);
            if (item == null)
                return;

            var targetApp = item.DataContext as AppItem;
            if (targetApp == null)
                return;

            var sourceApp = args.Data.GetData(typeof(AppItem)) as AppItem;
            if (sourceApp == null)
                return;

            if (sourceApp != targetApp)
            {
                int index = CurrentSelectedAppList.AppItems.IndexOf(targetApp);
                CurrentSelectedAppList.AppItems.Remove(sourceApp);
                CurrentSelectedAppList.AppItems.Insert(index, sourceApp);
                UpdateAppItemLayoutAction();
            }
        }

        private void AppListListBoxOnDragOver(EventHandlerParamProxy<ListBox, DragEventArgs> proxy)
        {
            var listbox = proxy.Sender;
            var args = proxy.EventArgs;

            var pos = args.GetPosition(listbox);
            var res = VisualTreeHelper.HitTest(listbox, pos);
            if (res == null)
                return;

            var item = VisualTreeUtils.FindParent<ListBoxItem>(res.VisualHit);
            if (item == null)
                return;

            var targetAppList = item.DataContext as AppList;
            if (targetAppList == null)
                return;

            var sourceAppList = args.Data.GetData(typeof(AppList)) as AppList;
            if (sourceAppList == null)
                return;

            if (sourceAppList != targetAppList)
            {
                int index = StaticData.AppLists.IndexOf(targetAppList);
                StaticData.AppLists.Remove(sourceAppList);
                StaticData.AppLists.Insert(index, sourceAppList);
                AppListListBoxSelectedIndex = index;
            }
        }

        private void AppListListBoxSelectionChanged(EventHandlerParamProxy<ListBox, SelectionChangedEventArgs> proxy)
        {
            UpdateAppItemLayoutAction?.Invoke();
        }

        private void RenameAppItem(AppItem app)
        {
            var newName = new InputTextDialog
            {
                Title = $"重命名“{app.AppName}”",
                Message = "请输入新名称：",
                Text = app.AppName
            }.ShowDialog();
            if (!string.IsNullOrEmpty(newName))
            {
                app.AppName = newName;
                UpdateAppItemLayoutAction();
            }
        }

        private void ViewSource(object obj)
        {
            Process.Start(new ProcessStartInfo("https://github.com/Mzying2001/AppLauncher_WPF")
            {
                UseShellExecute = true
            }).Dispose();
        }

        private void ShowInExplorer(AppItem app)
        {
            Process.Start(new ProcessStartInfo("Explorer.exe")
            {
                Arguments = "/e,/select," + app.AppPath
            });
        }

        private void ToggleWindowTopmost(object obj)
        {
            WindowTopmost = !WindowTopmost;
            StaticData.Config.MainWindowTopmost = WindowTopmost;
        }

        private void ToggleMinimizeWindowAfterOpening(object obj)
        {
            MinimizeWindowAfterOpening = !MinimizeWindowAfterOpening;
            StaticData.Config.MinimizeMainWindowAfterOpening = MinimizeWindowAfterOpening;
        }

        private void ToggleShowOpenErrorMsg(object obj)
        {
            ShowOpenErrorMsg = !ShowOpenErrorMsg;
            StaticData.Config.ShowOpenErrorMessage = ShowOpenErrorMsg;
        }

        private void Init()
        {
            var config = StaticData.Config;

            AppListListBoxSelectedIndex = config.AppListListBoxSelectedIndex;
            WindowTopmost = config.MainWindowTopmost;
            MinimizeWindowAfterOpening = config.MinimizeMainWindowAfterOpening;
            ShowOpenErrorMsg = config.ShowOpenErrorMessage;
        }

        public MainWindowViewModel()
        {
            Init();

            OpenAppCommand = new DelegateCommand<AppItem> { Execute = OpenApp };
            NewAppListCommand = new DelegateCommand { Execute = NewAppList };
            RenameAppListCommand = new DelegateCommand<AppList> { Execute = RenameAppList };
            RemoveAppListCommand = new DelegateCommand<AppList> { Execute = RemoveAppList };
            ShowPreviousAppListCommand = new DelegateCommand { Execute = ShowPreviousAppList };
            ShowNextAppListCommand = new DelegateCommand { Execute = ShowNextAppList };
            AddAppCommand = new DelegateCommand { Execute = AddApp };
            EditAppItemCommand = new DelegateCommand<AppItem> { Execute = EditAppItem };
            RemoveAppItemCommand = new DelegateCommand<AppItem> { Execute = RemoveAppItem };
            ShowAboutCommand = new DelegateCommand { Execute = ShowAbout };
            RenameAppItemCommand = new DelegateCommand<AppItem> { Execute = RenameAppItem };
            ViewSourceCommand = new DelegateCommand { Execute = ViewSource };
            ShowInExplorerCommand = new DelegateCommand<AppItem> { Execute = ShowInExplorer };
            ToggleWindowTopmostCommand = new DelegateCommand { Execute = ToggleWindowTopmost };
            ToggleMinimizeWindowAfterOpeningCommand = new DelegateCommand { Execute = ToggleMinimizeWindowAfterOpening };
            ToggleShowOpenErrorMsgCommand = new DelegateCommand { Execute = ToggleShowOpenErrorMsg };

            AppItemListBoxOnDropCommand = new DelegateCommand<EventHandlerParamProxy<ListBox, DragEventArgs>>
            { Execute = AppItemListBoxOnDrop };

            AppItemListBoxOnDragOverCommand = new DelegateCommand<EventHandlerParamProxy<ListBox, DragEventArgs>>
            { Execute = AppItemListBoxOnDragOver };

            AppListListBoxOnDragOverCommand = new DelegateCommand<EventHandlerParamProxy<ListBox, DragEventArgs>>
            { Execute = AppListListBoxOnDragOver };

            AppListListBoxSelectionChangedCommand = new DelegateCommand<EventHandlerParamProxy<ListBox, SelectionChangedEventArgs>>
            { Execute = AppListListBoxSelectionChanged };
        }
    }
}
