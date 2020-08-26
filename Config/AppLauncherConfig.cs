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

        public string Language
        {
            get => this["MAIN", "Language"];

            set
            {
                this["MAIN", "Language"] = value;
            }
        }
    }
}
