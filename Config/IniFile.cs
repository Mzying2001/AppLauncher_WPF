using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Config
{
    /// <summary>
    /// 表示一个ini文件
    /// </summary>
    public class IniFile
    {


        /// <summary>
        /// 配置文件路径
        /// </summary>
        public string Path;

        /// <summary>
        /// 文件是否存在
        /// </summary>
        public bool IsFileExists => File.Exists(Path);

        public IniFile(string path)
        {
            Path = path;
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// 向配置文件写入值
        /// </summary>
        public static void ProfileWriteValue(string section, string key, string value, string path)
        {
            WritePrivateProfileString(section, key, value, path);
        }

        /// <summary>
        /// 读取配置文件的值
        /// </summary>
        public static string ProfileReadValue(string section, string key, string path)
        {
            StringBuilder sb = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", sb, 255, path);
            return sb.ToString().Trim();
        }

        public string this[string section, string key]
        {
            get => ProfileReadValue(section, key, Path);

            set => ProfileWriteValue(section, key, value, Path);
        }
    }
}
