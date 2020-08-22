using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AppLauncher_WPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        //此部分代码来自 https://blog.csdn.net/qq_43307934/article/details/87974919

        private static System.Threading.Mutex mutex;
        //系统能够识别有名称的互斥，因此可以使用它禁止应用程序启动两次 
        //第二个参数可以设置为产品的名称:Application.ProductName 
        //每次启动应用程序，都会验证名称为OnlyRun的互斥是否存在 
        protected override void OnStartup(StartupEventArgs e)
        {
            mutex = new System.Threading.Mutex(true, "OnlyRun");
            if (mutex.WaitOne(0, false))
            {
                base.OnStartup(e);
            }
            else
            {
                MessageBox.Show("程序已运行", "提示");
                Shutdown();
            }
        }

    }
}
