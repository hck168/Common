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
    public class ConnectionStringsSection : AppConfigSection
    {
        /// <summary>
        /// 获取读数据库
        /// </summary>
        [ConfigurationProperty("readDb", IsKey = true, IsRequired = true)]
        public string ReadDb
        {
            get
            {
                return this["readDb"] as string;
            }
        }

        /// <summary>
        /// 获取写数据库
        /// </summary>
        [ConfigurationProperty("writeDb", IsKey = true, IsRequired = true)]
        public string WriteDb
        {
            get
            {
                return this["writeDb"] as string;
            }
        }

        /// <summary>
        /// 获取外部配置文件引用路径
        /// </summary>
        [ConfigurationProperty("file", IsKey = true)]
        public override string File
        {
            get
            {
                return this["file"] as string;
            }
        }

        /// <summary>
        /// 获取或者连接字符串集合（空字符表示子节点未设置父节点，否则为父节点名称）
        /// </summary>
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public AppConfigElementCollection<ConnectionStringsElement> ConnectionStrings
        {
            get
            {
                return this[""] as AppConfigElementCollection<ConnectionStringsElement>;
            }
            set
            {
                this[""] = value;
            }
        }
    }
}
