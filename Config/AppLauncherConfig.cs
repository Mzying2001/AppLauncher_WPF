using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config
{
    public class AppLauncherConfig : IniFile
    {
        public AppLauncherConfig(string path) : base(path) { }

        /// <summary>
        /// 写入值
        /// </summary>
        private void WriteValue(string section, string key, object value)
        {
            this[section, key] = value.ToString();
        }

        /// <summary>
        /// 读取布尔值
        /// </summary>
        private bool GetBool(string section, string key)
        {
            try
            {
                return bool.Parse(this[section, key]);
            }
            catch (Exception)
            {
                return false;
            }
        }

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
            get => GetBool("MAIN", "MinAfterLaunch");

            set => WriteValue("MAIN", "MinAfterLaunch", value);
        }

        /// <summary>
        /// 窗口置顶
        /// </summary>
        public bool WindowTopmost
        {
            get => GetBool("MAIN", "Topmost");

            set => WriteValue("MAIN", "Topmost", value);
        }
    }
}
