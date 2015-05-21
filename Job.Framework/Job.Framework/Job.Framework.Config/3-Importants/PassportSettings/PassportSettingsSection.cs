using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Xml;

namespace Job.Framework.Config
{
    /// <summary>
    /// 表示配置文件中组节点
    /// </summary>
    public class PassportSettingsSection : ConfigurationSection
    {
        /// <summary>
        /// 获取或者连接字符串集合（空字符表示子节点未设置父节点，否则为父节点名称）
        /// </summary>
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public AppConfigElementCollection<PassportSettingsElement> PassportSettings
        {
            get
            {
                return this[""] as AppConfigElementCollection<PassportSettingsElement>;
            }
            set
            {
                this[""] = value;
            }
        }
    }
}
