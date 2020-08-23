using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class AppList : List<Appconf>
    {


        public readonly string path_applist;//AppList文件路径
        public readonly string path_appconf;//应用程序配置文件所在文件夹


        public AppList(string path_applist, string path_appconf)
        {
            this.path_appconf = path_appconf;
            this.path_applist = path_applist;

            /*格式化*/
            path_appconf = path_appconf.Trim().Replace('/', '\\');
            path_appconf += path_appconf.EndsWith("\\") ? null : "\\";

            /*AppList文件不存在*/
            if (!File.Exists(path_applist))
                throw new Exception($"找不到AppList文件\"{path_applist}\"");

            /*应用程序配置文件文件夹不存在*/
            if (!Directory.Exists(path_appconf))
                throw new Exception($"找不到路径\"{path_appconf}\"");

            /*这个List保存每个在AppList中的Appconf路径，这些文件不会被删除（会自动删除AppList中没有的项）*/
            List<string> acpaths = new List<string>();

            /*添加项*/
            foreach (string confName in File.ReadAllText(path_applist).Split('\n'))
            {
                try
                {
                    string acpath = $"{path_appconf}{confName}.appconf";

                    Add(new Appconf(acpath));
                    acpaths.Add(acpath);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            /*删除AppList中没有的appconf*/
            foreach (string file in Directory.GetFiles(path_appconf))
            {
                if (!acpaths.Contains(file))
                {
                    File.Delete(file);
                }
            }
        }

        public new Appconf this[int index] => base[index];

        /// <summary>
        /// 保存AppList
        /// </summary>
        public void Save()
        {
            string list = null;

            foreach (Appconf a in this)
            {
                list += a.ConfName + "\n";
            }

            File.WriteAllText(path_applist, list);
        }

        /// <summary>
        /// 添加App
        /// </summary>
        public new void Add(Appconf ac)
        {
            if (!File.Exists(ac.AppPath))
                throw new Exception($"找不到应用程序\"{ac.AppName}\"");
            else
                base.Add(ac);
        }

        /// <summary>
        /// 插入App
        /// </summary>
        public new void Insert(int index, Appconf ac)
        {
            if (!File.Exists(ac.AppPath))
                throw new Exception($"找不到应用程序\"{ac.AppName}\"");
            else
                base.Insert(index, ac);
        }

        public new void Remove(Appconf ac)
        {
            File.Delete(ac.Path);
            base.Remove(ac);
        }

        public new void RemoveAt(int index)
        {
            File.Delete(base[index].Path);
            base.Remove(base[index]);
        }

        public void RemoveAt(int index,bool delete)
        {
            if (delete)
                RemoveAt(index);
            else
                base.RemoveAt(index);
        }
    }
}
