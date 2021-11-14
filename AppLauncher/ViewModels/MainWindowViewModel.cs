using AppLauncher.Models;
using AppLauncher.Views;
using AppLauncher.Views.Dialogs;
using Microsoft.Win32;
using SimpleMvvm;
using SimpleMvvm.Command;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AppLauncher.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public DelegateCommand OpenAppCommand { get; set; }
        public DelegateCommand NewAppListCommand { get; set; }
        public DelegateCommand RenameAppListCommand { get; set; }
        public DelegateCommand RemoveAppListCommand { get; set; }
        public DelegateCommand ShowPreviousAppListCommand { get; set; }
        public DelegateCommand ShowNextAppListCommand { get; set; }
        public DelegateCommand AddAppCommand { get; set; }
        public DelegateCommand EditAppItemCommand { get; set; }
        public DelegateCommand RemoveAppItemCommand { get; set; }
        public DelegateCommand ShowAboutCommand { get; set; }
        public DelegateCommand AppItemListBoxOnDropCommand { get; set; }
        public DelegateCommand AppItemListBoxOnDragOverCommand { get; set; }
        public DelegateCommand AppListListBoxOnDragOverCommand { get; set; }
        public DelegateCommand AppListListBoxSelectionChangedCommand { get; set; }
        public DelegateCommand RenameAppItemCommand { get; set; }
        public DelegateCommand ViewSourceCommand { get; set; }
        public DelegateCommand ShowInExplorerCommand { get; set; }
        public DelegateCommand ToggleWindowTopmostCommand { get; set; }
        public DelegateCommand ToggleMinimizeWindowAfterOpeningCommand { get; set; }
        public DelegateCommand ToggleShowOpenErrorMsgCommand { get; set; }

        private WindowState _windowState;
        public WindowState WindowState
        {
            get => _windowState;
            set => UpdateValue(ref _windowState, value);
        }

        private int _appListListBoxSelectedIndex;
        public int AppListListBoxSelectedIndex
        {
            get => _appListListBoxSelectedIndex;
            set => UpdateValue(ref _appListListBoxSelectedIndex, value);
        }

        private bool _windowTopmost;
        public bool WindowTopmost
        {
            get => _windowTopmost;
            set => UpdateValue(ref _windowTopmost, value);
        }

        private bool _minimizeWindowAfterOpening;
        public bool MinimizeWindowAfterOpening
        {
            get => _minimizeWindowAfterOpening;
            set => UpdateValue(ref _minimizeWindowAfterOpening, value);
        }

        private bool _showOpenErrorMsg;
        public bool ShowOpenErrorMsg
        {
            get => _showOpenErrorMsg;
            set => UpdateValue(ref _showOpenErrorMsg, value);
        }

        private AppList CurrentSelectedAppList => StaticData.AppLists[AppListListBoxSelectedIndex];

        private void OpenApp(AppItem app)
        {
            var path = PathHelper.FormatPath(app.AppPath);
            var info = Executer.ShellExecute(IntPtr.Zero, "open", path, string.Empty,
                PathHelper.GetLocatedFolderPath(path), Executer.ShowCommands.SW_SHOWNORMAL);

            if ((int)info < 32)
            {
                if (ShowOpenErrorMsg)
                    MsgBoxHelper.ShowError($"启动时发生错误：{Executer.GetErrorStr(info)}");
            }
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
        }

        private void RemoveAppItem(AppItem app)
        {
            if (MsgBoxHelper.ShowQuestion($"确定要删除“{app.AppName}”吗？") == MessageBoxResult.Yes)
            {
                CurrentSelectedAppList.AppItems.Remove(app);
            }
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

        protected override void Init()
        {
            base.Init();



            var config = StaticData.Config;
            AppListListBoxSelectedIndex = config.AppListListBoxSelectedIndex;
            WindowTopmost = config.MainWindowTopmost;
            MinimizeWindowAfterOpening = config.MinimizeMainWindowAfterOpening;
            ShowOpenErrorMsg = config.ShowOpenErrorMessage;



            OpenAppCommand = new DelegateCommand<AppItem>(OpenApp);
            NewAppListCommand = new DelegateCommand(NewAppList);
            RenameAppListCommand = new DelegateCommand<AppList>(RenameAppList);
            RemoveAppListCommand = new DelegateCommand<AppList>(RemoveAppList);
            ShowPreviousAppListCommand = new DelegateCommand(ShowPreviousAppList);
            ShowNextAppListCommand = new DelegateCommand(ShowNextAppList);
            AddAppCommand = new DelegateCommand(AddApp);
            EditAppItemCommand = new DelegateCommand<AppItem>(EditAppItem);
            RemoveAppItemCommand = new DelegateCommand<AppItem>(RemoveAppItem);
            ShowAboutCommand = new DelegateCommand(ShowAbout);
            RenameAppItemCommand = new DelegateCommand<AppItem>(RenameAppItem);
            ViewSourceCommand = new DelegateCommand(ViewSource);
            ShowInExplorerCommand = new DelegateCommand<AppItem>(ShowInExplorer);
            ToggleWindowTopmostCommand = new DelegateCommand(ToggleWindowTopmost);
            ToggleMinimizeWindowAfterOpeningCommand = new DelegateCommand(ToggleMinimizeWindowAfterOpening);
            ToggleShowOpenErrorMsgCommand = new DelegateCommand(ToggleShowOpenErrorMsg);

            AppItemListBoxOnDropCommand
                = new DelegateCommand<EventHandlerParamProxy<ListBox, DragEventArgs>>(AppItemListBoxOnDrop);

            AppItemListBoxOnDragOverCommand
                = new DelegateCommand<EventHandlerParamProxy<ListBox, DragEventArgs>>(AppItemListBoxOnDragOver);

            AppListListBoxOnDragOverCommand
                = new DelegateCommand<EventHandlerParamProxy<ListBox, DragEventArgs>>(AppListListBoxOnDragOver);

            AppListListBoxSelectionChangedCommand
                = new DelegateCommand<EventHandlerParamProxy<ListBox, SelectionChangedEventArgs>>(AppListListBoxSelectionChanged);
        }
    }
}
