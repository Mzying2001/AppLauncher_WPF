using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace AppLauncher.Models
{
    public static class StaticData
    {
        public static ObservableCollection<AppList> AppLists { get; private set; }

        public static Config Config { get; private set; }

        public const string PATH_DATA = @"Data\";
        public const string PATH_APPLIST_JSON = PATH_DATA + "AppList.json";
        public const string PATH_CONFIG_JSON = PATH_DATA + "Config.json";

        private static void LoadAppLists()
        {
            if (File.Exists(PATH_APPLIST_JSON))
            {
                var json = File.ReadAllText(PATH_APPLIST_JSON);
                AppLists = JsonConvert.DeserializeObject<ObservableCollection<AppList>>(json);
            }
            if (AppLists == null)
            {
                AppLists = new ObservableCollection<AppList>();
            }
            foreach (var item in AppLists)
            {
                if (item.AppItems == null)
                    item.AppItems = new ObservableCollection<AppItem>();
            }
        }

        private static void SaveAppLists()
        {
            if (!Directory.Exists(PATH_DATA))
                Directory.CreateDirectory(PATH_DATA);
            var json = JsonConvert.SerializeObject(AppLists, Formatting.Indented);
            File.WriteAllText(PATH_APPLIST_JSON, json);
        }

        private static void LoadConfig()
        {
            if (File.Exists(PATH_CONFIG_JSON))
            {
                var json = File.ReadAllText(PATH_CONFIG_JSON);
                Config = JsonConvert.DeserializeObject<Config>(json);
            }
            if (Config == null)
            {
                Config = new Config();
            }
        }

        private static void SaveConfig()
        {
            if (!Directory.Exists(PATH_DATA))
                Directory.CreateDirectory(PATH_DATA);
            var json = JsonConvert.SerializeObject(Config, Formatting.Indented);
            File.WriteAllText(PATH_CONFIG_JSON, json);
        }

        public static void InitStaticData()
        {
            LoadAppLists();
            LoadConfig();
        }

        public static void SaveStaticData()
        {
            SaveAppLists();
            SaveConfig();
        }

        public static void AddAppList(string name)
        {
            if (AppLists == null)
                AppLists = new ObservableCollection<AppList>();
            AppLists.Add(new AppList
            {
                Name = name,
                AppItems = new ObservableCollection<AppItem>()
            });
        }
    }
}
