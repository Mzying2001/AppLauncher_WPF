using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config
{
    /// <summary>
    /// 储存窗口状态信息
    /// </summary>
    public class WindowConfig : IniFile
    {
        public WindowConfig(string path) : base(path)
        {
            Path = path;
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        public bool IsFileExists => File.Exists(Path);

        /// <summary>
        /// 窗口横坐标
        /// </summary>
        public double X
        {
            get
            {
                return double.Parse(this["WindowStatus", "X"]);
            }
            set
            {
                this["WindowStatus", "X"] = value.ToString();
            }
        }

        /// <summary>
        /// 窗口纵坐标
        /// </summary>
        public double Y
        {
            get
            {
                return double.Parse(this["WindowStatus", "Y"]);
            }
            set
            {
                this["WindowStatus", "Y"] = value.ToString();
            }
        }

        /// <summary>
        /// 窗口宽度
        /// </summary>
        public double Width
        {
            get
            {
                return double.Parse(this["WindowStatus", "Width"]);
            }
            set
            {
                this["WindowStatus", "Width"] = value.ToString();
            }
        }

        /// <summary>
        /// 窗口宽度
        /// </summary>
        public double Height
        {
            get
            {
                return double.Parse(this["WindowStatus", "Height"]);
            }
            set
            {
                this["WindowStatus", "Height"] = value.ToString();
            }
        }
    }
}
