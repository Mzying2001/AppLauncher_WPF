using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App;

namespace Launcher
{
    public static class EXELauncher
    {
        /// <summary>
        /// 打开exe文件
        /// </summary>
        /// <param name="path"></param>
        public static void Start(string path)
        {
            string tmp = path.Trim().Replace('/', '\\');
            int index  = tmp.LastIndexOf('\\') + 1;

            Process.Start(new ProcessStartInfo()
            {
                WorkingDirectory = tmp.Substring(0, index),
                FileName = tmp.Substring(index)
            });
        }

        /// <summary>
        /// 打开exe文件
        /// </summary>
        /// <param name="ac"></param>
        public static void Start(Appconf ac)
        {
            Start(ac.AppPath);
        }
    }
}
