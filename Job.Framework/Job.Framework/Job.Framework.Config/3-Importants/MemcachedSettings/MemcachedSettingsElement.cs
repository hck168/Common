using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Job.Framework.Config
{
    /// <summary>
    /// 表示配置文件中单个节点
    /// </summary>
    public class MemcachedSettingsElement : ConfigurationElement
    {
        /// <summary>
        /// 获取或设置初始化池名称
        /// </summary>
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return this["name"] as string; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// 获取或设置（主）服务器列表
        /// </summary>
        [ConfigurationProperty("mainServer", IsRequired = true)]
        public string MainServer
        {
            get { return this["mainServer"] as string; }
            set { this["mainServer"] = value; }
        }

        /// <summary>
        /// 获取或设置（备）服务器列表
        /// </summary>
        [ConfigurationProperty("bakupServer")]
        public string BakupServer
        {
            get { return this["bakupServer"] as string; }
            set { this["bakupServer"] = value; }
        }
    }
}
