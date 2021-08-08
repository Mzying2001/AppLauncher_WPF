using AppLauncher.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AppLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var currentProcess = Process.GetCurrentProcess();
            var list = (from p
                        in Process.GetProcesses()
                        where p.ProcessName == currentProcess.ProcessName && p.Id != currentProcess.Id
                        select p).ToList();

            foreach (var p in list)
            {
                if (p.HasExited)
                    continue;

                bool flag;

                flag = p.CloseMainWindow();
                if (!flag)
                {
                    MsgBoxHelper.ShowError($"无法关闭线程（id：{p.Id}）。");
                    Shutdown();
                    return;
                }

                flag = p.WaitForExit(500);
                if (!flag)
                {
                    MsgBoxHelper.ShowError($"等待线程关闭超时（id：{p.Id}）。");
                    Shutdown();
                    return;
                }
            }

            base.OnStartup(e);
            StaticData.InitStaticData();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            StaticData.SaveStaticData();
        }
    }
}
