using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config
{
    public class AppLauncherConfig : IniFile
    {
        public AppLauncherConfig(string path) : base(path) { }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        public bool IsFileExists => File.Exists(Path);

        /// <summary>
        /// 语言
        /// </summary>
        public string Language
        {
            get => this["MAIN", "Language"];

            set
            {
                this["MAIN", "Language"] = value;
            }
        }

        /// <summary>
        /// 启动App后最小化窗口
        /// </summary>
        public bool MinAfterLaunch
        {
            get => bool.Parse(this["MAIN", "MinAfterLaunch"]);

            set
            {
                this["MAIN", "MinAfterLaunch"] = value.ToString();
            }
        }
    }
}
