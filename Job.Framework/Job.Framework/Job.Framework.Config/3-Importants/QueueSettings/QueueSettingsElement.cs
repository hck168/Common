using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;

namespace Job.Framework.Config
{
    /// <summary>
    /// 表示配置文件中单个节点
    /// </summary>
    public class QueueSettingsElement : ConfigurationElement
    {
        /// <summary>
        /// 获取或设置连接字符串名称
        /// </summary>
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return this["name"] as string; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// 获取或设置连接字符串
        /// </summary>
        [ConfigurationProperty("iPAddress", IsRequired = true)]
        public IPAddress IPAddress
        {
            get { return IPAddress.Parse(this["iPAddress"] as string); }
            set { this["iPAddress"] = value; }
        }

        /// <summary>
        /// 获取或设置提供程序名称属性
        /// </summary>
        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get { return Convert.ToInt32(this["port"]); }
            set { this["port"] = value; }
        }
    }
}
