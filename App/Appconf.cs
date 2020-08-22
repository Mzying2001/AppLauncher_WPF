using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Config;

namespace App
{
    /// <summary>
    /// 应用程序配置文件
    /// </summary>
    public class Appconf : IniFile
    {
        public Appconf(string path) : base(path)
        {
            Path = path;
        }

        /// <summary>
        /// 配置文件路径
        /// </summary>
        public new string Path
        {
            get
            {
                return base.Path;
            }
            private set
            {
                string path = value.Trim().Replace('/', '\\');

                if (string.IsNullOrEmpty(path))
                {//所给路径为空
                    throw new Exception("应用程序配置文件路径不能为空");
                }
                else if (!File.Exists(path))
                {//文件不存在
                    throw new Exception($"应用程序配置文件\"{path}\"不存在");
                }
                else if (!path.EndsWith(".appconf"))
                {//后缀名不正确
                    throw new Exception("应用程序配置文件后缀名错误，正确的后缀名应是\".appconf\"");
                }
                else
                {//正确
                    base.Path = path;
                }
            }
        }

        /// <summary>
        /// 配置文件名称
        /// </summary>
        public string ConfName
        {
            get
            {
                int index  = Path.LastIndexOf('\\') + 1;
                int length = Path.LastIndexOf('.') - index;

                return Path.Substring(index, length);
            }
        }

        /// <summary>
        /// App名称
        /// </summary>
        public string AppName
        {
            get
            {
                return base["APP_INFO", "AppName"];
            }
            set
            {
                base["APP_INFO", "AppName"] = value;
            }
        }

        /// <summary>
        /// App路径
        /// </summary>
        public string AppPath
        {
            get
            {
                return base["APP_INFO", "AppPath"];
            }
            set
            {
                base["APP_INFO", "AppPath"] = value;
            }
        }

        /// <summary>
        /// 创建appconf文件
        /// </summary>
        public static Appconf Create(string path, string AppName, string AppPath)
        {
            IniFile f = new IniFile(path);
            f["APP_INFO", "AppName"] = AppName;
            f["APP_INFO", "AppPath"] = AppPath;
            return new Appconf(path);
        }
    }
}
