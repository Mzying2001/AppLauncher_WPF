using System;
using System.Collections.Generic;
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


        /*声明变量*/
        public string Path;


        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

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

        public IniFile(string path)
        {
            Path = path;
        }

        public string this[string section, string key]
        {
            get
            {
                return ProfileReadValue(section, key, Path);
            }
            set
            {
                ProfileWriteValue(section, key, value, Path);
            }
        }
    }
}
