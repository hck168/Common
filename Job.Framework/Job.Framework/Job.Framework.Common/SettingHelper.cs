using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace Job.Framework.Common
{
    public static class SettingHelper
    {
        public sealed class Group
        {
            public List<User> Users { get; internal set; }
            public string Description { get; internal set; }
        }

        public sealed class User
        {
            public string Account { get; internal set; }
            public string Description { get; internal set; }
        }

        public static bool IsDebug { get; private set; }
        public static bool IgnorePermission { get; private set; }
        public static string CssVersion { get; private set; }
        public static string ScriptVersion { get; private set; }
        public static List<Group> UserConfig { get; private set; }

        public static void Register()
        {
            var fileWatcher = new FileSystemWatcher
            {
                Path = HttpRuntime.AppDomainAppPath,
                Filter = "Setting.xml",
                IncludeSubdirectories = false,
                EnableRaisingEvents = true
            };

            fileWatcher.Changed += fileWatcher_Changed;
            fileWatcher.Created += fileWatcher_Changed;
            fileWatcher.Deleted += fileWatcher_Changed;

            Init(Path.Combine(fileWatcher.Path, fileWatcher.Filter));
        }

        private static void fileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                var fileWatcher = sender as FileSystemWatcher;

                if (fileWatcher != null)
                {
                    fileWatcher.EnableRaisingEvents = false;

                    Init(e.FullPath);

                    fileWatcher.EnableRaisingEvents = true;
                }
            }
            catch { }
        }

        private static void Init(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            var xmlDoc = XDocument.Load(filePath);

            if (xmlDoc == null)
            {
                return;
            }

            var isDebug = xmlDoc.Root.Element("IsDebug");

            if (isDebug != null)
            {
                IsDebug = Convert.ToBoolean(isDebug.Attribute("value").Value);
            }

            var ignorePermission = xmlDoc.Root.Element("IgnorePermission");

            if (ignorePermission != null)
            {
                IgnorePermission = Convert.ToBoolean(ignorePermission.Attribute("value").Value);
            }

            var cssVersion = xmlDoc.Root.Element("CssVersion");

            if (cssVersion != null)
            {
                CssVersion = cssVersion.Attribute("value").Value;
            }

            var scriptVersion = xmlDoc.Root.Element("ScriptVersion");

            if (scriptVersion != null)
            {
                ScriptVersion = scriptVersion.Attribute("value").Value;
            }

            UserConfig = new List<Group>();

            foreach (var group in xmlDoc.Root.Descendants("Group"))
            {
                var users = new List<User>();

                foreach (var user in group.Descendants("User"))
                {
                    users.Add(new User
                    {
                        Account = user.Attribute("Account").Value,
                        Description = user.Attribute("Description").Value
                    });
                }

                UserConfig.Add(new Group
                {
                    Users = users,
                    Description = group.Attribute("Description").Value
                });
            }
        }
    }
}
